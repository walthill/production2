using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuBoi : MonoBehaviour
{
    public bool isPaused = false;
    public GameObject[] UIElements;

    float preTimeScale = 1.0f;

    void Start()
    {
        if (isPaused)
        {
            startPause();
        }
        else
        {
            unPause();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton7))
        {
            Debug.Log("Start Button Pressed");
            if (isPaused == false)
            {
                startPause();
                //preTimeScale = Time.timeScale;
                //Time.timeScale = 0.0f;
            }
            else
            {
                unPause();
                //Time.timeScale = preTimeScale;
            }
        }
    }

    public void unPause()
    {
        isPaused = false;
        //Time.timeScale = preTimeScale;
        for(int i = 0; i < UIElements.Length; i++)
        {
            UIElements[i].SetActive(false);
        }
    }

    void startPause()
    {
        isPaused = true;
        for (int i = 0; i < UIElements.Length; i++)
        {
            UIElements[i].SetActive(true);
        }
    }



}
