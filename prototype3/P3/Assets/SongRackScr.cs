using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongRackScr : MonoBehaviour
{
    public int CurrentSong = 0;
    public int numberOfSongs = 1;
    public static SongRackScr instance;
    
    //each array is a different song
    public AudioClip[] SongParts0;
    public AudioClip[] SongParts1;


    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        AssignClipsToTracks();
        //SoundBoi.instance.musicSnd0 = SongParts0[0];
        //loads the sound clips into sound sources on the Sound Boi
        SoundBoi.instance.SetMusic();
        SoundBoi.instance.PlayMusic();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
        CurrentSong++;
        AssignClipsToTracks();
        Debug.Log("trying to change music");
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
        CurrentSong--;
        AssignClipsToTracks();
        Debug.Log("trying to change music");
        }

    }

    public void AssignClipsToTracks()
    {

    
        if (CurrentSong == 0)
        {
            //musicSlot0.clip = SongRackScr.instance.SongParts0[0];
            //musicSlot1.clip = SongRackScr.instance.SongParts0[1];
            //musicSlot2.clip = SongRackScr.instance.SongParts0[2];
            //musicSlot3.clip = SongRackScr.instance.SongParts0[3];
            SoundBoi.instance.musicSnd0 = SongParts0[0];
            SoundBoi.instance.musicSnd1= SongParts0[1];
            SoundBoi.instance.musicSnd2 = SongParts0[2];
            SoundBoi.instance.musicSnd3 = SongParts0[3];
            //SoundBoi.instance.musicSnd4 = SongParts0[4];
            //SoundBoi.instance.musicSnd5 = SongParts0[5];

        }

        if (CurrentSong == 1)
        {

            SoundBoi.instance.musicSnd0 = SongParts1[0];
            SoundBoi.instance.musicSnd1 = SongParts1[1];
            SoundBoi.instance.musicSnd2 = SongParts1[2];
            SoundBoi.instance.musicSnd3 = SongParts1[3];
            SoundBoi.instance.musicSnd4 = SongParts1[4];
            //SoundBoi.instance.musicSnd5 = SongParts1[5];

        }
        if (CurrentSong > 1)
        {
            CurrentSong = 0;
        }
        if (CurrentSong < 0)
        {
            CurrentSong = 0;
        }
        SoundBoi.instance.SetMusic();
    }
}
