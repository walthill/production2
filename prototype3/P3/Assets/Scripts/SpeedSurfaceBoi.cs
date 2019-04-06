using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedSurfaceBoi : MonoBehaviour
{
    SpeedSurfaceScript[] speedSurfaces;
    // Start is called before the first frame update
    void Start()
    {
        speedSurfaces = GameObject.FindObjectsOfType<SpeedSurfaceScript>();
        Debug.Log(speedSurfaces.Length);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
