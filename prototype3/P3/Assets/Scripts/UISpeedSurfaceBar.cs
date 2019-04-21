using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISpeedSurfaceBar : MonoBehaviour
{
    float shortSide = 0.1f;
    float longside = 0.27f;

    public GameObject[] bars;

    public void SetCurrentBar(int activeBar)
    {
        if (activeBar < bars.Length)
        {
            bars[activeBar].GetComponent<RectTransform>().sizeDelta = new Vector2(shortSide, longside);
        }
    }

    public void ResetBars()
    {
        foreach (GameObject bar in bars)
        {
            bar.GetComponent<RectTransform>().sizeDelta = new Vector2(shortSide, shortSide);
        }
    }
}
