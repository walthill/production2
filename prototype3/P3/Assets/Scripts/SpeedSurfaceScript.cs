using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedSurfaceScript : MonoBehaviour
{
    [SerializeField]
    SpeedChannel speedRequired = SpeedChannel.NUM_SPEEDS;
    bool isEnabled = false;

    void Start()
    {
        gameObject.GetComponentInChildren<SpeedGate>().speedRequired = speedRequired;
    }

    public void setActive(SpeedChannel maxSpeed)
    {
        if(maxSpeed >= speedRequired)
        {
            isEnabled = true;
        }
        else
        {
            isEnabled = false;
        }
    }
}
