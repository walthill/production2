using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISpeedCellInfo : MonoBehaviour
{
    public Color lockedColor;
    public Color unlockedColor;
    public bool isLocked;

    public void SetAsActiveCell(Sprite activeSprite)
    {
        if (isLocked)
        {
            gameObject.GetComponent<Image>().color = unlockedColor;
        }
        gameObject.GetComponent<Image>().sprite = activeSprite;
    }
    public void SetAsInactiveCell(Sprite inactiveSprite)
    {
        gameObject.GetComponent<Image>().sprite = inactiveSprite;

    }
}
