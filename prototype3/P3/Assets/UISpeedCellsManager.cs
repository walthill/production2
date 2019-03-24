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

    [Header("Other Moving Parts")]
    public GameObject activeCellBorder;
    public GameObject cellLockDivider;
    private int highestSpeedChannel;
    public Text channelText;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) && (int)(currentSpeedChannel) < 6)
        {
            currentSpeedChannel = (SpeedChannel)(int)(currentSpeedChannel + 1);

            ChangeCellSprites();
            ChangeChannelText();

            MoveCellLockDivider();
        }

        else if (Input.GetKeyDown(KeyCode.LeftArrow) && (int)(currentSpeedChannel) > 0)
        {
            currentSpeedChannel = (SpeedChannel)(int)(currentSpeedChannel - 1);

            ChangeCellSprites();
            ChangeChannelText();
        }
    }



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
