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

    [Header("Music Sounds")]
    public AudioClip drumsSnd0;
    public AudioClip drumsSnd1;
    public AudioClip drumsSnd2;
    public AudioClip chordsSnd0;
    public AudioClip chordsSnd1;
    public AudioClip droneOrBaseSnd;
    public AudioClip melodySnd0;
    public AudioClip melodySnd1;
    public AudioClip arpSnd0;
    public AudioClip arpSnd1;


    [Header("General Feedback Sources")]
    public AudioSource WheelSource;
    public AudioSource sparkSource;
    public AudioSource chargingSource;
    public AudioSource ReleaseFeedBackSource;

    [Header("Music Sources")]
    public AudioSource drumsSource0;
    public AudioSource drumsSource1;
    public AudioSource drumsSource2;
    public AudioSource chordsSource0;
    public AudioSource chordsSource1;
    public AudioSource droneOrBaseSource;
    public AudioSource melodySource0;
    public AudioSource melodySource1;
    public AudioSource arpSource0;
    public AudioSource arpSource1;


    // Start is called before the first frame update
    void Start()
    {

        
    }

    public void LoadMusic()
    {
        //loads the clips into
        drumsSource0.clip = drumsSnd0;
        drumsSource1.clip = drumsSnd1;
        drumsSource2.clip = drumsSnd2;
        chordsSource0.clip = chordsSnd0;
        chordsSource1.clip = chordsSnd1;
        droneOrBaseSource.clip = droneOrBaseSnd;
        melodySource0.clip = melodySnd0;
        melodySource1.clip = melodySnd1;
        arpSource0.clip = arpSnd0;
        arpSource1.clip = arpSnd1;
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
