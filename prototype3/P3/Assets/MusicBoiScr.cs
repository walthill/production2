﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Networking;


public class MusicBoiScr : MonoBehaviour
{
    //temp have audio sources on this object attatched to this script.
    //clips are pulled from folders then loaded into sources then played
    //Start TEMP ####################################
    public const string audioName = "Bass.wav";


    [Header("audio stuff")]
    public AudioSource ChordSource;
    public string SoundPath;
    public string SongPath;
    public int SongNum = 0;
    //public List<GameObject> CapturePoints = new List<GameObject>();
    //public List<AudioClip> songParts = new List<AudioClip>();
    public AudioClip[] songParts;
    public AudioClip ChordClip;
    

    

    //end TEMP ###############################
    // Start is called before the first frame update
    void Start()
    {
        //SoundPath = "file://" + Application.streamingAssetsPath + "/audio/music/Song0/";
        SoundPath = "Assets/StreamingAssets/audio/music/Song2/Bass.wav";
        ChordClip = (AudioClip)AssetDatabase.LoadAssetAtPath("" + SoundPath,typeof(AudioClip));
        Debug.Log("chord clip is loaded as: " + gameObject.GetComponent<MusicBoiScr>().ChordClip.name);
        SongPath = "Assets/StreamingAssets/audio/music/Song" + SongNum;

        // check out the following links in order to add sound files to a list
        // https://docs.unity3d.com/530/Documentation/ScriptReference/AssetDatabase.GetAssetPath.html
        // https://docs.unity3d.com/530/Documentation/ScriptReference/AssetDatabase.FindAssets.html
        // https://docs.unity3d.com/530/Documentation/ScriptReference/AssetDatabase.LoadAllAssetsAtPath.html
        // https://docs.unity3d.com/530/Documentation/ScriptReference/AssetDatabase.LoadAssetAtPath.html

        //TODO: TRY USING RESOURCE FOLDER
        //THESE MIGHT BE HELPFUL:
        // https://answers.unity.com/questions/1018079/audioclip-array-error-1.html
        // https://answers.unity.com/questions/449659/convert-type-unityengineobject-to-unityengineaudio.html
        addThingsToList();
    }


    void addThingsToList()
    {
        
        //this doesn't work for some reason
        songParts = (AudioClip)AssetDatabase.LoadAllAssetsAtPath("" + SongPath, typeof(AudioClip));
        //ChordClip = (AudioClip)AssetDatabase.LoadAssetAtPath("" + SoundPath,typeof(AudioClip));
        foreach(AudioClip part in songParts)
        {
            Debug.Log(part);
        }
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

    // Update is called once per frame
    void Update()
    {
        
    }
}