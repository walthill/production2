using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedSurfaceBoost : MonoBehaviour
{
    //TODO: move player indicator UI into its own class. Singleton?
    //TODO: add anims to player ui

    const string CHARGE_SURFACE = "ChargeSurface", RELEASE_SURFACE = "ReleaseSurface";

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

    bool perfectRelease;

    private void Awake()
    {
        speedIndicator = gameObject.GetComponentInChildren<Image>();
        speedText = gameObject.GetComponentInChildren<Text>();

        speedIndicator.gameObject.SetActive(false);
        speedText.gameObject.SetActive(false);

        playerMove = gameObject.GetComponent<PlayerSkateMovement>();
    }

    void Update()
    {
       ProcessInput();

       if (isHeldDown)
        {
            if(timer < buildUpTime) //build up speed
            {
                timer += Time.deltaTime;
            }

            if(timer >= buildUpTime)
            {
                DisplaySpeed(true, "100%");
            }
        }
    }

    void ProcessInput()
    {
        aButtonHold = Input.GetButton("JoyJump");
        aButtonReleased = Input.GetButtonUp("JoyJump");
    }


    void SpeedSurfaceInteraction()
    {
        if (aButtonReleased && timer >= buildUpTime) // once speed is built up, give quick boost
        {
            Debug.Log("BOOST");
            playerMove.Boost(boostVelocityValue, maxVelocityIncrease);
            timer = 0;

            perfectRelease = true;
        }

        if (aButtonHold)
        {
            perfectRelease = false;
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

    void DisplaySpeed(bool showUI, string textToDisplay)
    {
        speedIndicator.gameObject.SetActive(showUI);
        speedText.gameObject.SetActive(showUI);

        speedText.text = textToDisplay;
    }

    #region Collision Handling
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

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == RELEASE_SURFACE)
        {
            if(perfectRelease)
                DisplaySpeed(true, "200%");
        }
    }
    #endregion
}
