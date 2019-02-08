using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedSurfaceBoost : MonoBehaviour
{
    const string CHARGE_SURFACE = "ChargeSurface", RELEASE_SURFACE = "ReleaseSurface";

    [SerializeField]
    float buildUpTime=0, timer=0;
    [SerializeField]
    float boostAcceleration, boostVelocityValue, maxVelocityIncrease;
    bool aButtonHold, aButtonReleased, isHeldDown;

    PlayerSkateMovement playerMove;

    private void Awake()
    {
        playerMove = gameObject.GetComponent<PlayerSkateMovement>();
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
        }
    }

    void SpeedSurfaceInteraction()
    {
        if (aButtonReleased && timer >= buildUpTime) // once speed is built up, give quick boost
        {
            Debug.Log("BOOST");
            playerMove.Boost(boostVelocityValue, maxVelocityIncrease);
            timer = 0;
        }

        if (aButtonHold)
        {
            Debug.Log("building speed...");
            playerMove.IncreaseSpeed(boostAcceleration);
            isHeldDown = true;
        }
        else
        {
            isHeldDown = false;
            timer = 0;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == CHARGE_SURFACE)
        {
            SpeedSurfaceInteraction();
        }
        else if(other.tag == RELEASE_SURFACE)
        {
            SpeedSurfaceInteraction();
        }
    }
}
