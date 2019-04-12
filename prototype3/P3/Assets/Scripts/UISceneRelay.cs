using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISceneRelay : MonoBehaviour
{
    public static UISceneRelay instance;
    UISpeedCellsManager speedCells;

    [SerializeField] GameObject canvas;

    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        speedCells = GameObject.Find("SpeedChannels").GetComponent<UISpeedCellsManager>();
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
}
