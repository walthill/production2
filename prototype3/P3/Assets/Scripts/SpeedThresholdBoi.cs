using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SpeedChannel { QUICK, SPEEDY, FAST, BLUR, LIGHTNING, WOW_SO_FAST, NUM_SPEEDS }

public class SpeedThresholdBoi : MonoBehaviour
{
    //base speed = 50
    [SerializeField]
    [Tooltip("Speed thresholds")]
    int[] speeds = new int[(int)SpeedChannel.NUM_SPEEDS];

    [SerializeField]
    SpeedChannel currentSpeedChannel = SpeedChannel.QUICK;
    PlayerSkateMovement playerMovement;
    PlayerSkateMovement.ArcadeMoveData moveData;
    [SerializeField]
    public Image speedIndicator;
    [SerializeField]
    public Text speedText;
    // Start is called before the first frame update
    void Start()
    {
        speedIndicator = gameObject.GetComponentInChildren<Image>();
        speedText = gameObject.GetComponentInChildren<Text>();

        speedIndicator.gameObject.SetActive(false);
        speedText.gameObject.SetActive(false);
        playerMovement = gameObject.GetComponent<PlayerSkateMovement>();
        setSpeedThreshold();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void speedBoost(float boostAmount)
    {
        float speedBoost = boostAmount;
        float maxSpeedBoost = boostAmount;
        playerMovement.Boost(speedBoost, maxSpeedBoost);
        setSpeedThreshold();
        setSoundAndUI();
    }

    void setSoundAndUI()
    {
        switch (currentSpeedChannel)
        {
            case SpeedChannel.QUICK:
                ParticleScript.instance.SpeedColor1();
                DisplaySpeed(true, "100%");
                break;
            case SpeedChannel.SPEEDY:
                ParticleScript.instance.SpeedColor2();
                SoundBoi.instance.VolumeMusic1();
                DisplaySpeed(true, "200%");
                break;
            case SpeedChannel.FAST:
                ParticleScript.instance.SpeedColor3();
                SoundBoi.instance.VolumeMusic2();
                DisplaySpeed(true, "300%");
                break;
            case SpeedChannel.BLUR:
                ParticleScript.instance.SpeedColor4();
                //SoundBoi.instance.VolumeMusic2();
                SoundBoi.instance.VolumeMusic3();
                DisplaySpeed(true, "400%");
                break;
            case SpeedChannel.LIGHTNING:
                ParticleScript.instance.SpeedColor5();
                SoundBoi.instance.VolumeMusic4();
                DisplaySpeed(true, "500%");
                break;
            case SpeedChannel.WOW_SO_FAST:
                ParticleScript.instance.SpeedColor6();
                SoundBoi.instance.VolumeMusic5();
                DisplaySpeed(true, "MAX");
                break;
            default:
                break;
        }
    }
    void setSpeedThreshold()
    {
        moveData = playerMovement.GetArcadeMoveData();
        int i = 0;
        //find current threshold
        while (speeds[i] < moveData.maxVelocity
            && i < (int)SpeedChannel.NUM_SPEEDS-1)
        {
            i++;
        }
        currentSpeedChannel = (SpeedChannel)i;
    }
    public bool checkSpeedThreshold(SpeedChannel speedRequired)
    {
        if(speedRequired == currentSpeedChannel)
        {
            return true;
        }
        return false;
    }
    public SpeedChannel getCurrentSpeedChannel()
    {
        return currentSpeedChannel;
    }

    void DisplaySpeed(bool showUI, string textToDisplay)
    {
        speedIndicator.gameObject.SetActive(showUI);
        speedText.gameObject.SetActive(showUI);
        speedText.text = textToDisplay;
    }
}
