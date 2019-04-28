using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
//using UnityEditor;
//using UnityEngine.Networking;


public class MusicBoiScr : MonoBehaviour
{
    public static MusicBoiScr instance;
    [Header("what songs are unlocked?")]

    public bool DuckMaster;
    public bool[] songsToUnlock;
    public AudioMixer mixer;
    public float UnmutedMaster;
    
    


    //temp have audio sources on this object attatched to this script.
    //clips are pulled from folders then loaded into sources then played
    //Start TEMP ####################################
    public const string audioName = "Bass.wav";


    [Header("audio stuff")]
    public AudioSource ChordSource;
    //public string SoundPath;
    public string SongPath;
    public int SongNum = 0;
    //public List<GameObject> CapturePoints = new List<GameObject>();
    //public List<AudioClip> songParts = new List<AudioClip>();
    //public List<Object> songParts = new List<Object>();
    //public AudioClip[] songParts;
    public AudioClip[] songParts;
    public AudioClip ChordClip;
    public AudioClip EmptySound;
    public AudioSource EmptySource;
    public float[] trackVol;

    public bool changingSong = false;


    //end TEMP ###############################
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        EmptySource = gameObject.AddComponent<AudioSource>();
        EmptySource.clip = EmptySound;
        EmptySource.playOnAwake = true;
        EmptySource.Play();
        EmptySource.volume = .2f;
        EmptySource.mute = true;
        EmptySource.loop = true;
        //EmptySource.volume = (EmptySource.volume) / 2;
        //SoundPath = "file://" + Application.Resources + "/audio/music/Song0/";
        
        
        //SoundPath = "Assets/StreamingAssets/audio/music/Song"+SongNum;
        //ChordClip = (AudioClip)AssetDatabase.LoadAssetAtPath("" + SoundPath,typeof(AudioClip));



        ////Debug.Log("chord clip is loaded as: " + gameObject.GetComponent<MusicBoiScr>().ChordClip.name);
        SongPath = "audio/music/Song" + SongNum;

        // check out the following links in order to add sound files to a list
        // https://docs.unity3d.com/530/Documentation/ScriptReference/AssetDatabase.GetAssetPath.html
        // https://docs.unity3d.com/530/Documentation/ScriptReference/AssetDatabase.FindAssets.html
        // https://docs.unity3d.com/530/Documentation/ScriptReference/AssetDatabase.LoadAllAssetsAtPath.html
        // https://docs.unity3d.com/530/Documentation/ScriptReference/AssetDatabase.LoadAssetAtPath.html

        //TODO: TRY USING RESOURCE FOLDER
        //THESE MIGHT BE HELPFUL:
        // https://answers.unity.com/questions/1018079/audioclip-array-error-1.html
        // https://answers.unity.com/questions/449659/convert-type-unityengineobject-to-unityengineaudio.html
        //THIS BITCH WORKS (SHOULD)
        //https://answers.unity.com/questions/1095437/is-there-a-way-to-populate-an-array-of-audio-clips.html <-------
        //HOW TO USE RESOURCE FOLDER:
        //https://www.youtube.com/watch?v=mTDy-A7gfSc
        addThingsToList();

        mixer.GetFloat("MasterVol", out UnmutedMaster);
        
    }





    void addThingsToList()
    {

        //this doesn't work for some reason
        songParts = Resources.LoadAll<AudioClip>(SongPath);
        /*
        foreach (AudioClip part in songParts)
        {
            //Debug.Log(part);
        }
        updateSoundBoi();
        //ChordClip = (AudioClip)AssetDatabase.LoadAssetAtPath("" + SoundPath,typeof(AudioClip));
        /*
        foreach(object part in songParts)
        {
            //Debug.Log(part);
        }
        */
        updateSoundBoi();
    }

    public void nextSong()
    {

        if (SongNum < 4)
        {


            
            UISceneRelay.instance.MusicGoForward();

            changingSong = true;
            //adjust the number of the song 
            SongNum++;
            //debug the number of the song so I can make sure it's working right
            //Debug.Log("You're listening to song number: " + SongNum);
            //adjust the string representing the path with the new song number
            SongPath = "audio/music/Song" + SongNum;
            //update the array
            songParts = Resources.LoadAll<AudioClip>(SongPath);
            updateSoundBoi();

            if (!songsToUnlock[SongNum])
            {

                float mute = -80;

                mixer.SetFloat("MasterVol", mute);
                Debug.Log("songsToUnlock is false");

                EmptySource.Play();
                EmptySource.mute = false;
            }
            if (songsToUnlock[SongNum])
            {
                UpdateSongVol();
                Debug.Log("songsToUnlock is false");
                
                EmptySource.mute = true;
                EmptySource.Stop();
            }


        }
        
        
    }

    public void UpdateSongVol()
    {
        mixer.SetFloat("MasterVol", trackVol[SongNum]);
    }

    public void prevSong()
    {
        if (SongNum > 0)
        {
            
            UISceneRelay.instance.MusicGoBack();

            changingSong = true;
            //adjust the number of the song 
            SongNum--;
            //debug the number of the song so I can make sure it's working right
            //Debug.Log("You're listening to song number: " + SongNum);
            //adjust the string representing the path with the new song number
            SongPath = "audio/music/Song" + SongNum;
            //update the array
            songParts = Resources.LoadAll<AudioClip>(SongPath);
            updateSoundBoi();
            if (!songsToUnlock[SongNum])
            {
                float mute = -80;
                mixer.SetFloat("MasterVol", mute);
                Debug.Log("songsToUnlock is false");
                EmptySource.Play();
                EmptySource.mute = false;
            }
            if (songsToUnlock[SongNum])
            {
                UpdateSongVol();
                EmptySource.mute = true;
                EmptySource.Stop();
            }
        }
        
    }


    public void updateSoundBoi()
    {
        SoundBoi.instance.musicPartsArray = songParts;
        AssignSoundBoiTracks();
    }

    public void AssignSoundBoiTracks()
    {
        SoundBoi.instance.AssignTracks();
        //play the tracks
        
    }


    // Update is called once per frame
    void Update()
    {

        if (Input.GetAxis("JoyDPH") == 0)
        {
            changingSong = false;
        }
        if (Input.GetAxis("JoyDPH") > 0 && !changingSong)
        {
            nextSong();
        }
        if (Input.GetAxis("JoyDPH") < 0 && !changingSong)
        {
            prevSong();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            UpdateSongVol();
        }
        if (Input.GetKeyDown(KeyCode.Z)&&SongNum>=0)
        {

            prevSong();
        }
        
        if (Input.GetKeyDown(KeyCode.C))
        {

            SoundBoi.instance.PlayMusic();
        }
        
    }



    public void UnlockTracks(int unlockNum)
    {
        songsToUnlock[unlockNum] = true;


        
    }




    /*
    private IEnumerator LoadAudio()
    {

    }
    /*
    private UnityWebRequest (string path, string filename)
    {
        string audioToLoad = string.Format(path + filename);
        UnityWebRequest request = new UnityWebRequest(audioToLoad);
        return request;
    }
    
    


    
    private WWWAudioExtensions(string path, string filename)
    {
        string audioToLoad = string.Format(path + filename);
        WWW request = new WWW(audioToLoad);
        return request;
    }


    void addThingsToList()
    {
        foreach(GameObject CapPoint in GameObject.FindGameObjectsWithTag("Point"))
        {
            CapturePoints.Add(CapPoint);
            numberOfPoints++;

        }
    }

    */

   
}
