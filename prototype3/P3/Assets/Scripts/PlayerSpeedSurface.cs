using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSpeedSurface : MonoBehaviour
{
    //TODO: move player indicator UI into its own class. Singleton?
    //TODO: add anims to player ui

    const string CHARGE_SURFACE = "ChargeSurface", RELEASE_SURFACE = "ReleaseSurface";

    public SpeedChannel currentSpeedChannel;

    [SerializeField]
    float boostTimer = 0; //how long the button has been held down for
    [SerializeField]
    float boostAcceleration = 0, boostVelocityValue = 0, maxVelocityIncrease = 0;

    PlayerSkateMovement playerMove;

    [SerializeField]
    public Image speedIndicator;
    [SerializeField]
    public Text speedText;

    bool perfectRelease, speedChange;


    //My Vars
    [SerializeField]
    bool isCharging = false;
    [SerializeField]

    bool isTouchingCharge = false;
    bool isTouchingRelease = false;

    
    void Start()
    {
        speedIndicator = gameObject.GetComponentInChildren<Image>();
        speedText = gameObject.GetComponentInChildren<Text>();

        speedIndicator.gameObject.SetActive(false);
        speedText.gameObject.SetActive(false);

        playerMove = gameObject.GetComponent<PlayerSkateMovement>();

        ParticleScript.instance.SpeedColor1();
    }

    // Update is called once per frame
    void Update()
    {
        if (isCharging) //maybe add velocity check?
        {
            boostTimer += Time.deltaTime;

            //if not on speed surface
            if(!isTouchingRelease && !isTouchingCharge)
            {
                stopCharging();
            }
        }
        //when player presses A
        if (Input.GetButtonDown("JoyJump"))
        {
            boostTimer = 0;
            if (isTouchingCharge)
            {
                startCharging();
            }
        }

        //when player releases A
        if (Input.GetButtonUp("JoyJump"))
        {
            if (isTouchingRelease && isCharging)
            {
                speedBoost();
                stopCharging();
                SoundBoi.instance.ReleaseSound();
            }
        }

    }
    private void startCharging()
    {
        isCharging = true;
        SoundBoi.instance.chargingSound();
    }
    private void stopCharging()
    {
        isCharging = false;
        SoundBoi.instance.stopChargingSound();
    }
    private void speedBoost()
    {
        //speed player up in proportion to how big the boost time is up to max boost
        boostVelocityValue = boostTimer*100;
        maxVelocityIncrease = boostTimer*100;
        playerMove.Boost(boostVelocityValue, maxVelocityIncrease);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == CHARGE_SURFACE)
        {
            if (isTouchingCharge)
            {
                Debug.LogError("Was already touching Charge surface");
            }
            isTouchingCharge = true;
        }
        if(other.tag == RELEASE_SURFACE)
        {
            if (isTouchingRelease)
            {
                Debug.LogError("Was already touching release surface");
            }
            isTouchingRelease = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == CHARGE_SURFACE)
        {
            if (!isTouchingCharge)
            {
                Debug.LogError("Was already not touching Charge surface");
            }
            isTouchingCharge = false;
        }
        if (other.tag == RELEASE_SURFACE)
        {
            if (!isTouchingRelease)
            {
                Debug.LogError("Was already not touching release surface");
            }
            isTouchingRelease = false;
        }
    }
}
