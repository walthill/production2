using System.Collections;
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
    public List<AudioClip> songParts;
    public AudioClip ChordClip;
    

    

    //end TEMP ###############################
    // Start is called before the first frame update
    void Start()
    {
        //SoundPath = "file://" + Application.streamingAssetsPath + "/audio/music/Song0/";
        SoundPath = "Assets/StreamingAssets/audio/music/Song2/Bass.wav";
        ChordClip = (AudioClip)AssetDatabase.LoadAssetAtPath("" + SoundPath,typeof(AudioClip));
        Debug.Log("chord clip is loaded as: " + gameObject.GetComponent<MusicBoiScr>().ChordClip.name);

        // check out the following links in order to add sound files to a list
        // https://docs.unity3d.com/530/Documentation/ScriptReference/AssetDatabase.GetAssetPath.html
        // https://docs.unity3d.com/530/Documentation/ScriptReference/AssetDatabase.FindAssets.html
        // https://docs.unity3d.com/530/Documentation/ScriptReference/AssetDatabase.LoadAllAssetsAtPath.html
        // https://docs.unity3d.com/530/Documentation/ScriptReference/AssetDatabase.LoadAssetAtPath.html

        //TODO LOOK AT STRATAGY PROGECT AND FIGURE OUT HOW TO ADD ALL THE AUDIO CLIPS TO A LIST USING
        // LOAD ALL ASSETS AT PATH

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
    

    */

    // Update is called once per frame
    void Update()
    {
        
    }
}
