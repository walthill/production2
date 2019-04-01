using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

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
    public AudioClip StaticChangeSongSnd;

    [Header("Music Sounds")]
    public AudioClip musicSnd0;
    public AudioClip musicSnd1;
    public AudioClip musicSnd2;
    public AudioClip musicSnd3;
    public AudioClip musicSnd4;
    public AudioClip musicSnd5;


    //[Header("Music Sounds")]
    public AudioClip[] musicPartsArray;

    


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
    public AudioSource ChangeSongStaticSource;

    [Header("Music Sources")]
    public AudioSource musicSlot0;
    public AudioSource musicSlot1;
    public AudioSource musicSlot2;
    public AudioSource musicSlot3;
    public AudioSource musicSlot4;
    public AudioSource musicSlot5;

    public AudioSource[] MusicSlotArray;

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

    bool isTimerGoing = false;
    float timerMax = 1;
    float timer = 0;

    public AudioMixerGroup[] groupArray;
    public AudioLowPassFilter[] lowPassArray;
    public AudioMixer mixer;

    bool Part1LowPass = false;
    bool Part2LowPass = false;
    bool Part3LowPass = false;
    bool Part4LowPass = false;
    bool Part5LowPass = false;



    public bool makeChargeSound = false;

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
        //SetMusic();
        LoadGeneralSounds();
        
    }


    public void PlayMusic()
    {
        /*
        musicSlot0.Play();
        musicSlot1.Play();
        musicSlot2.Play();
        musicSlot3.Play();
        musicSlot4.Play();
        musicSlot5.Play();
        */
        foreach(AudioSource slot in MusicSlotArray)
        {
            slot.Play();
        }
        
    }

    public void ClearMusic()
    {
        /*
        musicSlot0.clip = null;
        musicSlot1.clip = null;
        musicSlot2.clip = null;
        musicSlot3.clip = null;
        musicSlot4.clip = null;
        musicSlot5.clip = null;
        */
       
        foreach(AudioSource slot in MusicSlotArray)
        {
            slot.clip = null;
        }
    }

    //this is called from MusicBoi
    public void AssignTracks()
    {
        SetMusic();
        ChangeSongStaticSource.Play();
        isTimerGoing = true;
        ClearMusic();
        int partIndex = 0;
        int slotIndex = 0;
        foreach (AudioSource slot in MusicSlotArray)
        {
            if (partIndex <= musicPartsArray.Length && slotIndex <= MusicSlotArray.Length)
            {
                slot.clip = musicPartsArray[partIndex];
                //Debug.Log(musicPartsArray[partIndex].name + " assigned");
                partIndex++;
                slotIndex++;
            }
            
            
        }
        
    }

    //plays a static sound for 1 second and then plays music
    public void WaitForStatic()
    {
        timer = timer + Time.deltaTime;
        if (timer >= timerMax)
        {
            //Debug.Log("timer is done");
            PlayMusic();
            timer = 0;
            isTimerGoing = false;
        }

    }

    public void SetMusic()
    {

        
        foreach(AudioSource slot in MusicSlotArray)
        {
            slot.volume = .6f;
        }
        //MusicSlotArray[0].volume = .6f;


        /*
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
        ChangeSongStaticSource.clip = StaticChangeSongSnd;

        //MusicSlotArray[1].volume = .6f;

    }
    public void ReleaseSound()
    {
        ReleaseFeedBackSource.Play();
    }
    public void PlaywheelSound()
    {
        WheelSource.volume = .1f;
        WheelSource.Play();
        Debug.Log("playing wheels");
        Debug.Log("volume is" + WheelSource.volume);
    }

    public void WheelPitchUp()
    {
        WheelSource.volume = WheelSource.volume + .02f;
        WheelSource.pitch = pitchNumber;
        pitchNumber = pitchNumber + .02f;
        Debug.Log("pitch is: " + pitchNumber);
    }

    public void WheelPitchDown()
    {
        WheelSource.volume = WheelSource.volume - .02f;
        WheelSource.pitch = pitchNumber;
        pitchNumber = pitchNumber - .02f;
        Debug.Log("pitch is: " + pitchNumber);
    }


    public void stopWheelSound()
    {
        WheelSource.Stop();
    }

    //thisssssssssssssssssssssss

    public void chargingSound()
    {
        chargingSource.Play();
        chargingSource.pitch = ChargingpitchNumber;
        ChargingpitchNumber = ChargingpitchNumber+.05f;
    }

    public void stopChargingSound()
    {
        makeChargeSound = false;
        chargingSource.Stop();
        ChargingpitchNumber = .5f;
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            VolumeMusic1();
            //MusicSlotArray[1].volume = .6f;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            VolumeMusic2();

            //MusicSlotArray[2].volume = .6f;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            VolumeMusic3();

            //MusicSlotArray[3].volume = .6f;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            VolumeMusic4();

            //MusicSlotArray[4].volume = .6f;
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            VolumeMusic5();

            //MusicSlotArray[5].volume = .6f;
        }


        if (makeChargeSound)
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

        if (isTimerGoing)
        {
            WaitForStatic();
        }

        //////////////////////////////////////////////////////////
        //                                                      //
        //                                                      //
        //   these bois draw the parts out of a low cut         //
        //                                                      //
        //                                                      //
        //////////////////////////////////////////////////////////

        if (Part1LowPass)
        {
            //mixer.GetFloat("lowPassPart1",MixerFloat);
            
            //float MixerFloat = 0;
            float MixerFloat = 0;
            mixer.GetFloat("lowPassPart1", out MixerFloat);
            mixer.SetFloat("lowPassPart1", MixerFloat*1.1f);
            //float MixerFloat = mixer.GetFloat("lowPassPart1", MixerFloat);
            //Debug.Log(MixerFloat);
            if (MixerFloat >= 22000)
            {
                Part1LowPass = false;
            }
        }

        if (Part2LowPass)
        {
            //mixer.GetFloat("lowPassPart1",MixerFloat);

            //float MixerFloat = 0;
            float MixerFloat = 0;
            mixer.GetFloat("lowPassPart2", out MixerFloat);
            mixer.SetFloat("lowPassPart2", MixerFloat * 1.1f);
            //float MixerFloat = mixer.GetFloat("lowPassPart1", MixerFloat);
            //Debug.Log(MixerFloat);
            if (MixerFloat >= 22000)
            {
                Part2LowPass = false;
            }
        }

        if (Part3LowPass)
        {
            //mixer.GetFloat("lowPassPart1",MixerFloat);

            //float MixerFloat = 0;
            float MixerFloat = 0;
            mixer.GetFloat("lowPassPart3", out MixerFloat);
            mixer.SetFloat("lowPassPart3", MixerFloat * 1.1f);
            //float MixerFloat = mixer.GetFloat("lowPassPart1", MixerFloat);
            //Debug.Log(MixerFloat);
            if (MixerFloat >= 22000)
            {
                Part3LowPass = false;
            }
        }

        if (Part4LowPass)
        {
            //mixer.GetFloat("lowPassPart1",MixerFloat);

            //float MixerFloat = 0;
            float MixerFloat = 0;
            mixer.GetFloat("lowPassPart4", out MixerFloat);
            mixer.SetFloat("lowPassPart4", MixerFloat * 1.1f);
            //float MixerFloat = mixer.GetFloat("lowPassPart1", MixerFloat);
            //Debug.Log(MixerFloat);
            if (MixerFloat >= 22000)
            {
                Part4LowPass = false;
            }
        }

        if (Part5LowPass)
        {
            //mixer.GetFloat("lowPassPart1",MixerFloat);

            //float MixerFloat = 0;
            float MixerFloat = 0;
            mixer.GetFloat("lowPassPart5", out MixerFloat);
            mixer.SetFloat("lowPassPart5", MixerFloat * 1.1f);
            //float MixerFloat = mixer.GetFloat("lowPassPart1", MixerFloat);
            //Debug.Log(MixerFloat);
            if (MixerFloat >= 22000)
            {
                Part5LowPass = false;
            }
        }

    }

    //these raise the volume of the sounds as speed inceases
    //they will be called from a different script in the final version
    //20:00 2/7/19
    //right now they are triggered from update
    public void VolumeMusic1()
    {
        //musicSlot1.volume = heldVolume1;
        //MusicSlotArray[1].volume = .6f;
        //MusicSlotArray[1].outputAudioMixerGroup.audioMixer 
        //outputAudioMixerGroup.
        //MusicSlotArray[1].GetComponent<AudioMixerGroup>()    //<AudioLowPassFilter>().cutoffFrequency = (Mathf.Sin(Time.time) * 11010 + 11000);
        //groupArray[1].AudioLowPassFilter
        //lowPassArray[0] =
        //.GetComponent<AudioLowPassFilter>().cutoffFrequency = (Mathf.Sin(Time.time) * 11010 + 11000);

        //MusicSlotArray[1].GetComponent<AudioLowPassFilter>().cutoffFrequency =+ 11000;
        //AudioLowPassFilter audioLow = MusicSlotArray[1].GetComponent<AudioLowPassFilter>();
        //audioLow.cutoffFrequency = 11000;
        Part1LowPass = true;
        
    }
    public void VolumeMusic2()
    {
        //musicSlot2.volume = heldVolume2;
        //MusicSlotArray[2].volume = .6f;
        Part2LowPass = true;
    }
    public void VolumeMusic3()
    {
        //musicSlot3.volume = heldVolume3;\
        //MusicSlotArray[3].volume = .6f;
        Part3LowPass = true;
    }
    public void VolumeMusic4()
    {
        //musicSlot4.volume = heldVolume4;
        //MusicSlotArray[4].volume = .6f;
        Part4LowPass = true;
    }
    public void VolumeMusic5()
    {
        //musicSlot5.volume = heldVolume5;
        //MusicSlotArray[5].volume = .6f;
        Part5LowPass = true;
    }

}
