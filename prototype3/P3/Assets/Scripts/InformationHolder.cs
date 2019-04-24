using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InformationHolder : MonoBehaviour
{
    public GameObject relatedInfo;
    public GameObject whiteNoise;
    bool canChange = false;

    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject != this.gameObject)
        {
            relatedInfo.SetActive(false);
            canChange = true;
        }
        else
        {
            if (canChange && gameObject.name != "0. BACK TO MENU")
            {
                whiteNoise.gameObject.transform.Find("RawImage/WhiteNoise").GetComponent<WhiteNoiseScript>().animCycles = 0;
                whiteNoise.SetActive(true);
                relatedInfo.SetActive(true);
                canChange = false;
            }
        }
    }
}
