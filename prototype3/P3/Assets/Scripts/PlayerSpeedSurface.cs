using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpeedSurface : MonoBehaviour
{
    //TODO: move player indicator UI into its own class. Singleton?
    //TODO: add anims to player ui

    //Current: Player speeds up based on distance between button press and release point
    //TODO: perfect release based on distance to perfect release point

    const string CHARGE_SURFACE = "ChargeSurface", RELEASE_SURFACE = "ReleaseSurface";

    [SerializeField]
    [Tooltip("Multiplier applied to the boost effect")]
    float boostMultiplier = 2f;

    PlayerSkateMovement playerMove;
    SpeedThresholdBoi speedBoi;
    [SerializeField]
    bool isTouchingCharge = false;
    [SerializeField]
    bool isTouchingRelease = false;
    Vector3 startPosit, endPosit; //where the player presses and releases the button
    
    //[Header("Displays boost effects")]
    [SerializeField]
    bool isCharging = false;
    float boostVelocityValue = 0;
    float boostLength; //distance the button was held down for.
    SpeedChannel surfaceChannel;
    private void Start()
    {
        

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
        if (Input.GetButtonDown("JoyCharge"))
        {
            ////Debug.Log("a is registrered");
            if (isTouchingCharge)
            {
                ////Debug.Log("charging is registered");
                startPosit = transform.position;
                startCharging();
            }
        }

        //when player releases A
        if (Input.GetButtonUp("JoyCharge"))
        {
            if (isTouchingRelease && isCharging)
            {
                endPosit = transform.position;
                boostLength = Vector3.Distance(startPosit, endPosit);
                speedBoost();
                stopCharging();
				if(speedBoi.canUseSpeedSurface(surfaceChannel))
					SoundBoi.instance.ReleaseSound();
            }
        }

    }
    private void startCharging()
    {
        isCharging = true;
		if(speedBoi.canUseSpeedSurface(surfaceChannel))
			SoundBoi.instance.makeChargeSound = true;
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
        speedBoi.speedBoost(boostVelocityValue, surfaceChannel);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == CHARGE_SURFACE)
        {
            isTouchingCharge = true;
            surfaceChannel = other.gameObject.GetComponent<SpeedGate>().speedRequired;
        }
        if(other.tag == RELEASE_SURFACE)
        {
            isTouchingRelease = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == CHARGE_SURFACE)
        {
            isTouchingCharge = false;
        }
        if (other.tag == RELEASE_SURFACE)
        {
            isTouchingRelease = false;
        }
    }
}
