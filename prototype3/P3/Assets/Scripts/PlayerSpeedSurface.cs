using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSpeedSurface : MonoBehaviour
{
    //TODO: move player indicator UI into its own class. Singleton?
    //TODO: add anims to player ui

    //Current: Player speeds up based on distance between button press and release point
    //TODO: perfect release based on distance to perfect release point

    const string CHARGE_SURFACE = "ChargeSurface", RELEASE_SURFACE = "ReleaseSurface";
    public SpeedChannel currentSpeedChannel;

    [SerializeField]
    [Tooltip("Multiplier applied to the boost effect")]
    float boostMultiplier = 2f;

    PlayerSkateMovement playerMove;
    SpeedThresholdBoi speedBoi;
    [SerializeField]
    public Image speedIndicator;
    [SerializeField]
    public Text speedText;

    bool isTouchingCharge = false;
    bool isTouchingRelease = false;
    Vector3 startPosit, endPosit; //where the player presses and releases the button
    
    [Header("Displays boost effects")]
    [SerializeField]
    bool isCharging = false;
    [SerializeField]
    float boostVelocityValue = 0, maxVelocityIncrease = 0;
    [SerializeField]
    float boostLength; //distance the button was held down for.
    private void Awake()
    {
        speedIndicator = gameObject.GetComponentInChildren<Image>();
        speedText = gameObject.GetComponentInChildren<Text>();

        speedIndicator.gameObject.SetActive(false);
        speedText.gameObject.SetActive(false);

        playerMove = gameObject.GetComponent<PlayerSkateMovement>();
        speedBoi = gameObject.GetComponent<SpeedThresholdBoi>();

        ParticleScript.instance.SpeedColor1();
    }

    void Update()
    {
        if (isCharging)
        {
            //if not on speed surface
            if(!isTouchingRelease && !isTouchingCharge)
            {
                stopCharging();
            }
        }
        //when player presses A
        if (Input.GetButtonDown("JoyJump"))
        {
            if (isTouchingCharge)
            {
                startPosit = transform.position;
                startCharging();
            }
        }

        //when player releases A
        if (Input.GetButtonUp("JoyJump"))
        {
            if (isTouchingRelease && isCharging)
            {
                endPosit = transform.position;
                boostLength = Vector3.Distance(startPosit, endPosit);
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
        boostVelocityValue = boostLength*boostMultiplier;
        maxVelocityIncrease = boostLength*boostMultiplier;
        speedBoi.speedBoost(boostVelocityValue);
        //playerMove.Boost(boostVelocityValue, maxVelocityIncrease);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == CHARGE_SURFACE)
        {
            if (isTouchingCharge)
            {
                Debug.LogError("Was already touching Charge surface");
            }
            bool validSurface = speedBoi.checkSpeedThreshold(other.GetComponent<SpeedSurfaceScript>().speedRequired);
            if (validSurface)
            {
                isTouchingCharge = true;
            }
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
            bool validSurface = speedBoi.checkSpeedThreshold(other.GetComponent<SpeedSurfaceScript>().speedRequired);
            if (validSurface)
            {
                isTouchingCharge = false;
            }
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
