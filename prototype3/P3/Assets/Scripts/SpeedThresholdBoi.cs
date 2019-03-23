using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    // Start is called before the first frame update
    void Start()
    {
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
    }

    void setSpeedThreshold()
    {
        moveData = playerMovement.GetArcadeMoveData();
        int i = 0;
        //find current threshold
        while (speeds[i] < moveData.maxVelocity
            && i < (int)SpeedChannel.NUM_SPEEDS)
        {
            i++;
        }
        currentSpeedChannel = (SpeedChannel)i;
    }
}
