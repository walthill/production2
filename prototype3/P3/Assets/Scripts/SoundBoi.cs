using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBoi : MonoBehaviour
{
    public static SoundBoi instance;

    [Header("General Feedback Sounds")]
    public AudioClip WheelSoundSlowSnd;
    public AudioClip WheelSoundFastSnd;
    public AudioClip SparksSnd;
    public AudioClip ChargingSnd;
    public AudioClip ReleaseSnd;
    public AudioClip PerfectReleaseSnd;

    [Header("Music Sounds")]
    public AudioClip musicSnd0;
    public AudioClip musicSnd1;
    public AudioClip musicSnd2;
    public AudioClip musicSnd3;
    public AudioClip musicSnd4;
    public AudioClip musicSnd5;

    /*
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
    */


    [Header("General Feedback Sources")]
    public AudioSource WheelSource;
    public AudioSource sparkSource;
    public AudioSource chargingSource;
    public AudioSource ReleaseFeedBackSource;

    [Header("Music Sources")]
    public AudioSource musicSlot0;
    public AudioSource musicSlot1;
    public AudioSource musicSlot2;
    public AudioSource musicSlot3;
    public AudioSource musicSlot4;
    public AudioSource musicSlot5;
    /*
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
    */
    float heldVolume0;
    float heldVolume1;
    float heldVolume2;
    float heldVolume3;
    float heldVolume4;
    float heldVolume5;

    float pitchNumber = 1;
    float ChargingpitchNumber = 1;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;

        heldVolume0 = musicSlot0.volume;
        heldVolume1 = musicSlot1.volume;
        heldVolume2 = musicSlot2.volume;
        heldVolume3 = musicSlot3.volume;
        heldVolume4 = musicSlot4.volume;
        heldVolume5 = musicSlot5.volume;
        SetMusic();
        LoadGeneralSounds();
    }

    public void SetMusic()
    {

        

        musicSlot1.volume = 0;
        musicSlot2.volume = 0;
        musicSlot3.volume = 0;
        musicSlot4.volume = 0;
        musicSlot5.volume = 0;

        musicSlot0.clip = musicSnd0;
        musicSlot1.clip = musicSnd1;
        musicSlot2.clip = musicSnd2;
        musicSlot3.clip = musicSnd3;
        musicSlot4.clip = musicSnd4;
        musicSlot5.clip = musicSnd5;
        musicSlot0.Play();
        musicSlot1.Play();
        musicSlot2.Play();
        musicSlot3.Play();
        musicSlot4.Play();
        musicSlot5.Play();

        //loads the clips into
        /*
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
        */

    }





    public void LoadGeneralSounds()
    {
        //loads the clips into
        WheelSource.clip = WheelSoundSlowSnd;
        sparkSource.clip = SparksSnd;
        chargingSource.clip = ChargingSnd;
        ReleaseFeedBackSource.clip = ReleaseSnd;
    }
    public void ReleaseSound()
    {
        ReleaseFeedBackSource.Play();
    }
    public void PlaywheelSound()
    {
        WheelSource.Play();
        Debug.Log("playing wheels");
        Debug.Log("volume is" + WheelSource.volume);
    }

    public void WheelPitchUp()
    {
        WheelSource.pitch = pitchNumber;
        pitchNumber = pitchNumber + .1f;
    }

    public void WheelPitchDown()
    {
        WheelSource.pitch = pitchNumber;
        pitchNumber = pitchNumber - .1f;
    }


    public void stopWheelSound()
    {
        WheelSource.Stop();
    }

    public void chargingSound()
    {
        chargingSource.Play();
        chargingSource.pitch = ChargingpitchNumber;
        ChargingpitchNumber = ChargingpitchNumber+.1f;
    }

    public void stopChargingSound()
    {

        chargingSource.Stop();
        ChargingpitchNumber = 1;
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            VolumeMusic1();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            VolumeMusic2();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            VolumeMusic3();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            VolumeMusic4();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            VolumeMusic5();
        }
        if (Input.GetKey(KeyCode.Space))
        {
            //PlaywheelSound();
            chargingSound();

        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            //stopWheelSound();
            stopChargingSound();
            ReleaseSound();

        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            PlaywheelSound();
        }

        var d = Input.GetAxis("Mouse ScrollWheel");
        if (d > 0f)
        {
            WheelPitchUp();
        }
        else if (d < 0f)
        {
            WheelPitchDown();
        }

    }

    //these raise the volume of the sounds as speed inceases
    //they will be called from a different script in the final version
    //20:00 2/7/19
    //right now they are triggered from update
    public void VolumeMusic1()
    {
        musicSlot1.volume = heldVolume1;
    }
    public void VolumeMusic2()
    {
        musicSlot2.volume = heldVolume2;
    }
    public void VolumeMusic3()
    {
        musicSlot3.volume = heldVolume3;
    }
    public void VolumeMusic4()
    {
        musicSlot4.volume = heldVolume4;
    }
    public void VolumeMusic5()
    {
        musicSlot5.volume = heldVolume5;
    }

}
