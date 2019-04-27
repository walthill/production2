using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISceneRelay : MonoBehaviour
{
    public static UISceneRelay instance;
    UISpeedCellsManager speedCells;
    UIMusicManager musicStuff;
    public bool allowMusicUIToWork;

    [SerializeField] GameObject canvas;

    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        speedCells = GameObject.Find("SpeedChannels").GetComponent<UISpeedCellsManager>();
        musicStuff = GameObject.Find("Music").GetComponent<UIMusicManager>();
        
        if (!allowMusicUIToWork)
        {
            musicStuff.gameObject.SetActive(false);
        }
    }

    public void setCurrentChannel(SpeedChannel currentChannel)
    {
        speedCells.SetCurrentChannel(currentChannel);
    }
    public void setCurrentSpeed(float speedValueNormalized)
    {
        speedCells.SetCurrentSpeed(speedValueNormalized);
    }

    public void setCurrentSpeedAndChannel(SpeedChannel currentChannel, float speedValueNormalized)
    {
        speedCells.SetCurrentChannelAndSpeed(currentChannel, speedValueNormalized);
    }

    public void HideUI()
    {
        canvas.SetActive(false);
    }
    public void ShowUI()
    {
        canvas.SetActive(true);
    }

    public void MusicGoBack()
    {
        if (musicStuff.enabled)
        {
            musicStuff.Rewind();
        }
    }

    public void MusicGoForward()
    {
        if (musicStuff.enabled)
        {
            musicStuff.FastFoward();
        }
    }

    public void MusicPlay()
    {
        if (musicStuff.enabled)
        {
            musicStuff.PlayMusic();
        }
    }

    public void setOnSpeedSurfaceNoX(bool xNotPressed)
    {
        //set when player is on speed surface
    }

    public void xHold(float distance, bool xHeld)
    {
        //set when player is holding X on a speed surface w/ the distance
    }

    public void onReleaseSurfaceX(bool xIsPressed)
    {
        //set when player is on the release surface and still holding X
    }

    public void correctRelease()
    {
        //called when the player correctly releases X
        SpeedChannel currentChannel = PlayerSceneRelay.instance.getSpeedChannel();
       //Debug.Log("Correct Release");
    }

    public void earlyRelease()
    {
       //Debug.Log("Early Release");
        //called when the player releases X before the release surface
    }

    public void lateRelease()
    {
       //Debug.Log("late release");
        //called when the player releases X after the release surface
    }

}
