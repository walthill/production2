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
        unPause();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton7))
        {
            Debug.Log("Button Pressed");
            doPause();
            StartCoroutine(startPause());
        }
    }

    IEnumerator startPause()
    {
        Debug.Log("Started Pause");
        Time.timeScale = 0.0f;
        yield return new WaitForEndOfFrame(); //So it does not immediately unpause
        while (isPaused)
        {
            yield return waitForKeyPress();
        }
        Debug.Log("Ended Pause");
        Time.timeScale = 1.0f;
        unPause();
    }

    private IEnumerator waitForKeyPress()
    {
        bool done = false;
        while (!done) // essentially a "while true", but with a bool to break out naturally
        {
            //UnPause
            if (Input.GetKeyDown(KeyCode.JoystickButton7))
            {
                done = true; // breaks the loop
                isPaused = false;
            }
            if (Input.GetAxis("JoyVertical") > 0.5f)
            {
                Debug.Log("Move Up");
                //MOVE CURSOR UPWARD
                done = true;
            }
            if (Input.GetAxis("JoyVertical") < -0.5f)
            {
                //MOVE CURSOR DOWNWARD HERE
                done = true;
            }
            if (Input.GetKeyDown(KeyCode.JoystickButton0))
            {
                //CLICK SELECTED BUTTON HERE
                done = true;
            }

            yield return null; // wait until next frame, then continue execution from here (loop continues)
        }
    }

    public void unPause()
    {
        isPaused = false;
        for(int i = 0; i < UIElements.Length; i++)
        {
            UIElements[i].SetActive(false);
        }
    }

    public void doPause()
    {
        isPaused = true;
        for (int i = 0; i < UIElements.Length; i++)
        {
            UIElements[i].SetActive(true);
        }
    }

    public void invertCamera()
    {
        GameObject.Find("CameraRig").GetComponent<FollowCamera>().toggleInvert();
    }

}
