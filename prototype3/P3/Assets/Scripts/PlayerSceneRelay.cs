using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSceneRelay : MonoBehaviour
{
    public static PlayerSceneRelay instance;
    SpeedThresholdBoi speedBoi;

    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        speedBoi = GameObject.Find("Player").GetComponent<SpeedThresholdBoi>();
    }

    public SpeedChannel getSpeedChannel()
    {
        return speedBoi.getCurrentSpeedChannel();
    }
}
