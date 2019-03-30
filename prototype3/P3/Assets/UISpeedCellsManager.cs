using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISpeedCellsManager : MonoBehaviour
{
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

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.RightArrow) && (int)(currentSpeedChannel) < 6)
        //{
        //    currentSpeedChannel = (SpeedChannel)(int)(currentSpeedChannel + 1);
        //
        //    ChangeCellSprites();
        //    ChangeChannelText();
        //
        //    MoveCellLockDivider();
        //}
        //
        //else if (Input.GetKeyDown(KeyCode.LeftArrow) && (int)(currentSpeedChannel) > 0)
        //{
        //    currentSpeedChannel = (SpeedChannel)(int)(currentSpeedChannel - 1);
        //
        //    ChangeCellSprites();
        //    ChangeChannelText();
        //}

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    Debug.Log(PlayerSceneRelay.instance.getSpeedChannel());
        //}

        if (Input.GetKey(KeyCode.A))
        {
            speedBar.value -= .01f;

            if (speedBar.value == 0f && (int)(currentSpeedChannel) > 0)
            {
                speedBar.value = 1f;

                currentSpeedChannel = (SpeedChannel)(int)(currentSpeedChannel - 1);

                ChangeCellSprites();
                ChangeChannelText();
            }
        }
        else if (Input.GetKey(KeyCode.D))
        {
            speedBar.value += .01f;

            if (speedBar.value == 1f && (int)(currentSpeedChannel) < 6)
            {
                speedBar.value = 0f;

                currentSpeedChannel = (SpeedChannel)(int)(currentSpeedChannel + 1);

                ChangeCellSprites();
                ChangeChannelText();

                MoveCellLockDivider();
            }
        }
    }


    // SetCurrentChannel function //
    //      current speed channel would be sent in

    // SetCurrentSpeed function //
    //      number between 0-1 would be sent in (speed bar value); literally dont worry about it being called every frame

    void ChangeCellSprites()
    {
        for (int i = 0; i < uiCells.Length; i++)
        {
            if (i != (int)(currentSpeedChannel))
            {
                uiCells[i].GetComponent<UISpeedCellInfo>().SetAsInactiveCell(inactiveCellSprite);
            }
            else
            {
                uiCells[i].GetComponent<UISpeedCellInfo>().SetAsActiveCell(activeCellSprite);
                activeCellBorder.transform.position = uiCells[i].transform.position;

                // Changes the color of the text
                channelText.color = uiCells[i].GetComponent<UISpeedCellInfo>().unlockedColor;

                //Changes the color of the bar
                speedBar.fillRect.GetComponent<Image>().color = uiCells[i].GetComponent<UISpeedCellInfo>().unlockedColor;
            }
        }
    }

    void ChangeChannelText()
    {
        channelText.text = "0" + (int)(currentSpeedChannel + 1);
    }

    void MoveCellLockDivider()
    {
        if ((int)(currentSpeedChannel) > highestSpeedChannel)
        {
            cellLockDivider.GetComponent<RectTransform>().anchoredPosition = new Vector2(cellLockDivider.GetComponent<RectTransform>().anchoredPosition.x + 50, cellLockDivider.GetComponent<RectTransform>().anchoredPosition.y);
            highestSpeedChannel++;
        }
    }
}
