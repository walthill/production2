using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SpeedChannel { QUICK, SPEEDY, FAST, BLUR, LIGHTNING, WOW_SO_FAST, LIVE_IN_DARKNESS, NUM_SPEEDS }

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
    [SerializeField]
    [Tooltip("How far you have to drop below a speed threshold to drop out of it")]
    float speedChannelOffset = 0.3f;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        speedIndicator = gameObject.GetComponentInChildren<Image>();
        speedText = gameObject.GetComponentInChildren<Text>();
        rb = gameObject.GetComponent<Rigidbody>();

//        speedIndicator.gameObject.SetActive(false);
 //       speedText.gameObject.SetActive(false);
        playerMovement = gameObject.GetComponent<PlayerSkateMovement>();
        playerMovement.setAccelCap(speeds[0]);//set accel cap to lowest speed threshold
        playerMovement.setMaxVelocity(speeds[0]);
        setCurrentSpeedChannel();
        setSound();
    }

    private void Update()
    {
        setCurrentSpeedChannel();
        sendCurrentSpeedChannel();
    }
    void sendCurrentSpeedChannel()
    {
        float speed = rb.velocity.magnitude;
        float channelMin = 0;
        float channelMax = speeds[(int)currentSpeedChannel];
        if (currentSpeedChannel > 0)
        {
            channelMin = speeds[(int)currentSpeedChannel - 1];
        }
        //normalize speed within channel from 0-1
        speed -= channelMin;
        speed = speed < 0 ? 0 : speed; 
        channelMax -= channelMin;
        speed /= channelMax;
        UISceneRelay.instance.setCurrentSpeedAndChannel(currentSpeedChannel, speed);
    }

    public void speedBoost(float boostAmount, SpeedChannel surfaceChannel)
    {
        if(maxSpeedChannel == surfaceChannel 
            && currentSpeedChannel == maxSpeedChannel
            && maxSpeedChannel != SpeedChannel.NUM_SPEEDS-1)
        {
            float speed = speeds[(int)maxSpeedChannel + 1];
            playerMovement.setMaxVelocity(speed);
            //set speed to % of channel max.
            speed -= (speeds[(int)maxSpeedChannel + 1] - speeds[(int)maxSpeedChannel]) * (1.0f - newChannelStartPercent);
            playerMovement.setSpeed(speed);
            maxSpeedChannel++;
            setSound();
        }
        else if (maxSpeedChannel >= surfaceChannel)
        {
            float speedBoost = boostAmount;
            playerMovement.Boost(speedBoost, 0f);
            setCurrentSpeedChannel();
        }
        setCurrentSpeedChannel();
    }

    void setSound()
    {
        switch (maxSpeedChannel)
        {
            case SpeedChannel.QUICK:
                ParticleScript.instance.SpeedColor1();
                break;
            case SpeedChannel.SPEEDY:
                ParticleScript.instance.SpeedColor2();
                SoundBoi.instance.VolumeMusic1();
                break;
            case SpeedChannel.FAST:
                ParticleScript.instance.SpeedColor3();
                SoundBoi.instance.VolumeMusic2();
                break;
            case SpeedChannel.BLUR:
                ParticleScript.instance.SpeedColor4();
                SoundBoi.instance.VolumeMusic3();
                break;
            case SpeedChannel.LIGHTNING:
                ParticleScript.instance.SpeedColor5();
                SoundBoi.instance.VolumeMusic4();
                break;
            case SpeedChannel.WOW_SO_FAST:
                ParticleScript.instance.SpeedColor6();
                SoundBoi.instance.VolumeMusic5();
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
        float velocity = rb.velocity.magnitude;

        while (speeds[i] < velocity
            && i < (int)maxSpeedChannel)
        {
            i++;
        }
        SpeedChannel newSpeedChannel = (SpeedChannel)i;

        //prevent dropping out of speed channel unless speed is lower than offset
        if(newSpeedChannel < currentSpeedChannel)
        {
            float diff = speeds[i] - velocity;
            if(diff > speedChannelOffset)
            {
                currentSpeedChannel = newSpeedChannel;
            }
        }
        else
        {
            currentSpeedChannel = newSpeedChannel;
        }
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

    public SpeedChannel getMaxSpeedChannel()
    {
        return maxSpeedChannel;
    }

    void DisplaySpeed(bool showUI, string textToDisplay)
    {
        speedIndicator.gameObject.SetActive(showUI);
        speedText.gameObject.SetActive(showUI);
        speedText.text = textToDisplay;
    }
}
