using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISpeedCellsManager : MonoBehaviour
{
    [Header("Leo's Secret Lab")]
    public bool leosBool;
    public float speedBarChange = .01f;

    [Header ("Speed Channel Cells")]
    public SpeedChannel currentSpeedChannel;
    public GameObject[] uiCells;

    [Header("Cell Sprites")]
    public Sprite inactiveCellSprite;
    public Sprite activeCellSprite;

    [Header("Speed Bar")]
    public Slider speedBar;

    [Header("Other Moving Parts")]
    public GameObject activeCellBorder;
    public GameObject cellLockDivider;
    private int highestSpeedChannel;
    public Text textThatJustSaysCH;
    public Text channelText;

    [Header("smooth transition vars")]
    [SerializeField]
    float speedToGoUp = 0.9f;
    [SerializeField]
    float speedToGoDown = 0.0f;

    // This just makes sure that the "Debug mode" doesnt ever launch during runtime //
    private void Awake()
    {
        leosBool = false;
    }

    private void Update()
    {   
        //if (Input.GetKey(KeyCode.A))
        //{
        //    speedBar.value -= .01f;
        //
        //    if (speedBar.value == 0f && (int)(currentSpeedChannel) > 0)
        //    {
        //        speedBar.value = 1f;
        //
        //        currentSpeedChannel = (SpeedChannel)(int)(currentSpeedChannel - 1);
        //
        //        ChangeCellSprites();
        //        ChangeChannelText();
        //
        //        MoveCellLockDivider();
        //    }
        //}
        //else if (Input.GetKey(KeyCode.D))
        //{
        //    speedBar.value += .01f;
        //
        //    if (speedBar.value == 1f && (int)(currentSpeedChannel) < 6)
        //    {
        //        speedBar.value = 0f;
        //
        //        currentSpeedChannel = (SpeedChannel)(int)(currentSpeedChannel + 1);
        //
        //        ChangeCellSprites();
        //        ChangeChannelText();
        //
        //        MoveCellLockDivider();
        //    }
        //}

        // This is for testing purposes //
        if (leosBool)
        {
            if (Input.GetKey(KeyCode.A))
            {
                SetCurrentSpeed(speedBar.value - speedBarChange);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                SetCurrentSpeed(speedBar.value + speedBarChange);
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow) && (int)(currentSpeedChannel) > 0)
            {
                SetCurrentChannel((SpeedChannel)(int)(currentSpeedChannel - 1));
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) && (int)(currentSpeedChannel) < 6)
            {
                SetCurrentChannel((SpeedChannel)(int)(currentSpeedChannel + 1));
            }
        }

    }

    public void SetCurrentChannel(SpeedChannel currentChannel)
    { //deprecated
        currentSpeedChannel = currentChannel;

        ChangeCellSprites();
        ChangeChannelText();
        MoveCellLockDivider();
    }

    public void SetCurrentChannelAndSpeed(SpeedChannel currentChannel, float speedBarValue)
    {
        if(currentSpeedChannel == currentChannel)
        {
            speedBar.value = speedBarValue;
        }
        else if(currentSpeedChannel < currentChannel)
        {
            if(speedBarValue > speedToGoUp)
            {
                Debug.Log(speedBarValue);
                Debug.Log(currentChannel);
                Debug.Log(currentSpeedChannel);
                currentSpeedChannel = currentChannel;
                speedBar.value = speedBarValue;
            }
        }
        else
        {
            speedBar.value = speedBarValue;
            currentSpeedChannel = currentChannel;
        }

        ChangeCellSprites();
        ChangeChannelText();
        MoveCellLockDivider();
    }

    public void SetCurrentSpeed(float speedBarValue)
    { //deprecated
        speedBar.value = speedBarValue;
    }

    // Changes all the cell sprites to be active or inactive //
    void ChangeCellSprites()
    {
        for (int i = 0; i < uiCells.Length; i++)
        {
            // Every cell that isn't the current speed channel becomes inactive //
            if (i != (int)(currentSpeedChannel))
            {
                uiCells[i].GetComponent<UISpeedCellInfo>().SetAsInactiveCell(inactiveCellSprite);
            }
            // The one cell that is the current speed channel becomes active //
            else
            {
                uiCells[i].GetComponent<UISpeedCellInfo>().SetAsActiveCell(activeCellSprite);

                // Moves the active cell border //
                activeCellBorder.transform.position = uiCells[i].transform.position;

                // Changes the color of the text //
                channelText.color = uiCells[i].GetComponent<UISpeedCellInfo>().unlockedColor;

                // Changes the color of the bar //
                speedBar.fillRect.GetComponent<Image>().color = uiCells[i].GetComponent<UISpeedCellInfo>().unlockedColor;
            }
        }
    }

    // Changes the text in the top left of the screen //
    void ChangeChannelText()
    {
        channelText.text = "0" + (int)(currentSpeedChannel + 1);
    }

    // Moves the cell lock divider to differentiate between what cells are unlocked or not //
    void MoveCellLockDivider()
    {
        if ((int)(currentSpeedChannel) > highestSpeedChannel)
        {
            cellLockDivider.GetComponent<RectTransform>().anchoredPosition = new Vector2(cellLockDivider.GetComponent<RectTransform>().anchoredPosition.x + 50, cellLockDivider.GetComponent<RectTransform>().anchoredPosition.y);
            highestSpeedChannel++;
        }
    }
}
