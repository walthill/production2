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
    bool isTouchingCharge = false;
    [SerializeField]
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
        //when player presses A
        if (Input.GetButtonDown("JoyJump"))
        {

        }

        //when player releases A
        if (Input.GetButtonUp("JoyJump"))
        {

        }

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
