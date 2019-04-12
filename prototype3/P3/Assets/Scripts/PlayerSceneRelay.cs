using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSceneRelay : MonoBehaviour
{
    public static PlayerSceneRelay instance;
    PlayerSkateMovement playerMove;
    SpeedThresholdBoi speedBoi;

    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        speedBoi = GameObject.Find("Player").GetComponent<SpeedThresholdBoi>();
        playerMove = speedBoi.gameObject.GetComponent<PlayerSkateMovement>();
    }

    public SpeedChannel getSpeedChannel()
    {
        return speedBoi.getCurrentSpeedChannel();
    }

    public SpeedChannel getMaxSpeedChannel()
    {
        return speedBoi.getMaxSpeedChannel();
    }

    public void ResetPlayer(Transform t)
    {
        playerMove.ResetPlayer(t);
    }
}
