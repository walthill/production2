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
    [SerializeField]
    SpeedChannel maxSpeedChannel = SpeedChannel.QUICK;
    PlayerSkateMovement playerMovement;
    PlayerSkateMovement.ArcadeMoveData moveData;
    [SerializeField]
    public Image speedIndicator;
    [SerializeField]
    public Text speedText;
    [SerializeField]
    [Tooltip("What percent of the speed threshold the player starts at when breaking into a new threshold")]
    float newChannelStartPercent = 0.5f;

    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        speedIndicator = gameObject.GetComponentInChildren<Image>();
        speedText = gameObject.GetComponentInChildren<Text>();
        rb = gameObject.GetComponent<Rigidbody>();

        speedIndicator.gameObject.SetActive(false);
        speedText.gameObject.SetActive(false);
        playerMovement = gameObject.GetComponent<PlayerSkateMovement>();
        playerMovement.setAccelCap(speeds[0]);//set accel cap to lowest speed threshold
        playerMovement.setMaxVelocity(speeds[0]);
        setCurrentSpeedChannel();
        setSoundAndUI();
    }

    private void Update()
    {
        setCurrentSpeedChannel();
        //TODO: send leo's UI stuff//
    }


    public void speedBoost(float boostAmount, SpeedChannel surfaceChannel)
    {
        if(maxSpeedChannel == surfaceChannel && maxSpeedChannel != SpeedChannel.NUM_SPEEDS-1)
        {
            float speed = speeds[(int)maxSpeedChannel + 1];
            playerMovement.setMaxVelocity(speed);
            //set speed to % of channel max.
            speed -= (speeds[(int)maxSpeedChannel + 1] - speeds[(int)maxSpeedChannel]) * (1.0f - newChannelStartPercent);
            playerMovement.setSpeed(speed);
            maxSpeedChannel++;
            setSoundAndUI();
        }
        else if (maxSpeedChannel >= surfaceChannel)
        {
            float speedBoost = boostAmount;
            playerMovement.Boost(speedBoost, 0f);
            setCurrentSpeedChannel();
        }
        setCurrentSpeedChannel();
    }

    void setSoundAndUI()
    {
        switch (maxSpeedChannel)
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
                DisplaySpeed(true, "300%");
                break;
            case SpeedChannel.BLUR:
                ParticleScript.instance.SpeedColor4();
                SoundBoi.instance.VolumeMusic2();
                DisplaySpeed(true, "400%");
                break;
            case SpeedChannel.LIGHTNING:
                ParticleScript.instance.SpeedColor5();
                DisplaySpeed(true, "500%");
                break;
            case SpeedChannel.WOW_SO_FAST:
                ParticleScript.instance.SpeedColor6();
                SoundBoi.instance.VolumeMusic3();
                DisplaySpeed(true, "MAX");
                break;
            default:
                break;
        }
    }
    void setCurrentSpeedChannel()
    {
        moveData = playerMovement.GetArcadeMoveData();
        int i = 0;
        //find current threshold
        while (speeds[i] < rb.velocity.magnitude
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
