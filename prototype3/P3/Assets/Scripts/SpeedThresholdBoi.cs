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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
