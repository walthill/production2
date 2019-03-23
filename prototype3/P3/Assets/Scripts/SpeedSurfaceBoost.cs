using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedSurfaceBoost : MonoBehaviour
{
    //TODO: move player indicator UI into its own class. Singleton?
    //TODO: add anims to player ui

    const string CHARGE_SURFACE = "ChargeSurface", RELEASE_SURFACE = "ReleaseSurface";

    public SpeedChannel currentSpeedChannel;

    [SerializeField]
    float buildUpTime=0, timer=0;
    [SerializeField]
    float boostAcceleration=0, boostVelocityValue=0, maxVelocityIncrease=0;
    bool aButtonHold, aButtonReleased, isHeldDown;

    PlayerSkateMovement playerMove;

    [SerializeField]
    public Image speedIndicator;
    [SerializeField]
    public Text speedText;

    bool perfectRelease, speedChange;

    private void Awake()
    {
        speedIndicator = gameObject.GetComponentInChildren<Image>();
        speedText = gameObject.GetComponentInChildren<Text>();

        speedIndicator.gameObject.SetActive(false);
        speedText.gameObject.SetActive(false);

        playerMove = gameObject.GetComponent<PlayerSkateMovement>();

        ParticleScript.instance.SpeedColor1();
    }

    void Update()
    {
        aButtonHold = Input.GetButton("JoyJump");
        aButtonReleased = Input.GetButtonUp("JoyJump");

        if(isHeldDown)
        {
            if(timer < buildUpTime) //build up speed
            {
                timer += Time.deltaTime;
            }

            //==Move this to speed Levels==
            if (timer >= buildUpTime)
            {
                if (!speedChange)
                {
                    currentSpeedChannel = (SpeedChannel)((int)(currentSpeedChannel + 1));
                    Debug.Log(currentSpeedChannel);

                    if (currentSpeedChannel == SpeedChannel.WOW_SO_FAST)
                        currentSpeedChannel = (SpeedChannel)(0);

                    if (currentSpeedChannel == SpeedChannel.SPEEDY)
                    {
                        //temp
                        ParticleScript.instance.SpeedColor6();
                        SoundBoi.instance.VolumeMusic1();
                        SoundBoi.instance.VolumeMusic2();
                        SoundBoi.instance.VolumeMusic3();
                    }
                    else if (currentSpeedChannel == SpeedChannel.FAST)
                    { //temp
                       // ParticleScript.instance.SpeedColor6();
                       // SoundBoi.instance.VolumeMusic1();
                       // SoundBoi.instance.VolumeMusic2();
                       // SoundBoi.instance.VolumeMusic3();
                        //ParticleScript.instance.SpeedColor3();
                    }
                    else if (currentSpeedChannel == SpeedChannel.BLUR)
                    {
                        ParticleScript.instance.SpeedColor4();
                    }
                    else if (currentSpeedChannel == SpeedChannel.LIGHTNING)
                    {
                        ParticleScript.instance.SpeedColor5();
                    }
                    else if (currentSpeedChannel == SpeedChannel.WOW_SO_FAST)
                    {
                        ParticleScript.instance.SpeedColor6();
                    }

                    DisplaySpeed(true, "100%");
                    speedChange = true;
                }
            }
        }
    }
   
    void SpeedSurfaceInteraction()
    {
        if (aButtonReleased && timer >= buildUpTime) // once speed is built up, give quick boost
        {
            Debug.Log("BOOST");
            SoundBoi.instance.stopChargingSound();
            SoundBoi.instance.ReleaseSound();
            playerMove.Boost(boostVelocityValue, maxVelocityIncrease);
            timer = 0;
            perfectRelease = true;
        }

        if (aButtonHold) //build up speed
        {
            SoundBoi.instance.chargingSound();
            perfectRelease = false;
            Debug.Log("building speed...");
            playerMove.IncreaseSpeed(boostAcceleration);
            isHeldDown = true;
        }
        else
        {
            speedChange = false;
            isHeldDown = false;
            timer = 0;
            Camera.main.GetComponent<FollowCamera>().ToggleSpeedUI();
        }
    }

    void DisplaySpeed(bool showUI, string textToDisplay)
    {
        speedIndicator.gameObject.SetActive(showUI);
        speedText.gameObject.SetActive(showUI);
        speedText.text = textToDisplay;
    }

    #region Collision Handling
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponentInParent<SpeedGate>() != null)
        {
            if ((int)other.gameObject.GetComponentInParent<SpeedGate>().speedChannel == (int)currentSpeedChannel + 1)
            {
                if (other.tag == CHARGE_SURFACE)
                {
                    SpeedSurfaceInteraction();
                }
            }

            if ((int)other.gameObject.GetComponentInParent<SpeedGate>().speedChannel == (int)currentSpeedChannel)
            {
                if (other.tag == RELEASE_SURFACE)
                {
                    SpeedSurfaceInteraction();
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == RELEASE_SURFACE)
        {
            if(perfectRelease)
                DisplaySpeed(true, "200%");

            if (!speedChange)
            {
                currentSpeedChannel = (SpeedChannel)(int)currentSpeedChannel + 1;
                speedChange = true;
            }

            SoundBoi.instance.stopChargingSound();
        }
        if (other.tag == CHARGE_SURFACE)
        {
            SoundBoi.instance.stopChargingSound();
        }
    }
    #endregion
}
