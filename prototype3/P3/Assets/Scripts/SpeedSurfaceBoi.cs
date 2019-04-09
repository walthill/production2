using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedSurfaceBoi : MonoBehaviour
{
    public static SpeedSurfaceBoi instance;
    SpeedSurfaceScript[] speedSurfaces;
    SpeedChannel maxSpeed;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        speedSurfaces = GameObject.FindObjectsOfType<SpeedSurfaceScript>();
    }

    public void updateSpeedLevel(SpeedChannel newSpeed)
    {
        if(newSpeed > maxSpeed)
        {
            maxSpeed = newSpeed;
            setSpeedSurfaces();
        }
    }
    
    void setSpeedSurfaces()
    {
        foreach(SpeedSurfaceScript script in speedSurfaces)
        {
            script.setActive(maxSpeed);
        }
    }
}
