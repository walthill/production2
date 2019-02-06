using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBoi : MonoBehaviour
{
    [Header("General Feedback Sounds")]
    public AudioClip WheelSoundSlowSnd;
    public AudioClip WheelSoundFastSnd;
    public AudioClip SparksSnd;
    public AudioClip ChargingSnd;
    public AudioClip ReleaseSnd;
    public AudioClip PerfectReleaseSnd;
    [Header("General Feedback Sounds")]
    public AudioSource WheelSource;
    public AudioSource sparkSource;
    public AudioSource chargingSource;
    public AudioSource ReleaseFeedBackSource;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
