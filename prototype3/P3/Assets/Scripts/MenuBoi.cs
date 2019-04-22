using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuBoi : MonoBehaviour
{
    public bool isPaused = false;
    public GameObject[] UIElements;

    public Button[] buttonsToPress;
    public Button currentButton;
    public int buttonInt;

    public bool camInverted = false;

    bool isHeld;

    public bool isInControlsMenu = false;

    float preTimeScale = 1.0f;

    void Start()
    {
        unPause();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton7))
        {
            //Debug.Log("Button Pressed");
            doPause();
            StartCoroutine(startPause());
        }
    }

    IEnumerator startPause()
    {
        //Debug.Log("Started Pause");
        preTimeScale = Time.timeScale;
        Time.timeScale = 0.0f;
        yield return new WaitForEndOfFrame(); //So it does not immediately unpause
        while (isPaused)
        {
            yield return waitForKeyPress();
        }
        //Debug.Log("Ended Pause");
        Time.timeScale = preTimeScale;
        unPause();
    }

    private IEnumerator waitForKeyPress()
    {
        bool done = false;
        while (!done) // essentially a "while true", but with a bool to break out naturally
        {
            //UnPause
            /*if (Input.GetKeyDown(KeyCode.JoystickButton7) && !isInControlsMenu)
            {
                done = true; // breaks the loop
                isPaused = false;
            }*/
            if (Input.GetAxis("JoyVertical") > 0.5f && !isHeld && !isInControlsMenu)
            {
                if (buttonInt > 0)
                {
                    currentButton.GetComponent<UIControllerWrapper>().setSelected(false);
                    buttonInt--;
                    currentButton = buttonsToPress[buttonInt];
                    currentButton.GetComponent<UIControllerWrapper>().setSelected(true);
                    done = true;
                    isHeld = true;
                }
                else
                {
                    //Debug.Log("Where the FUCK are you going up");
                }
            }
            if (Input.GetAxis("JoyVertical") < -0.5f && !isHeld && !isInControlsMenu)
            {
                if (buttonInt+1 < buttonsToPress.Length)
                {
                    currentButton.GetComponent<UIControllerWrapper>().setSelected(false);
                    buttonInt++;
                    currentButton = buttonsToPress[buttonInt];
                    currentButton.GetComponent<UIControllerWrapper>().setSelected(true);
                    done = true;
                    isHeld = true;
                }
                else
                {
                    //Debug.Log("Where the FUCK are you going down");
                }
            }

            if (Input.GetAxis("JoyVertical") == 0 && isHeld && !isInControlsMenu)
            {
                isHeld = false;
            }
            
            if (Input.GetKeyDown(KeyCode.JoystickButton0) && !isInControlsMenu)
            {
                //CLICK SELECTED BUTTON HERE
                done = true;
            }
            
            //if (Input.GetKeyDown(KeyCode.JoystickButton0) && isInControlsMenu)
            //{
            //    gameObject.transform.Find("DefaultMenu").gameObject.SetActive(true);
            //    gameObject.transform.Find("ControlsMenu/Controls").gameObject.SetActive(false);
            //    isInControlsMenu = false;
            //}

            yield return null; // wait until next frame, then continue execution from here (loop continues)
        }
    }

    public void GoToControls()
    {
        //Debug.Log("whhhh");
        gameObject.transform.Find("DefaultMenu").gameObject.SetActive(false);
        gameObject.transform.Find("ControlsMenu/Controls").gameObject.SetActive(true);
        gameObject.transform.Find("ControlsMenu/Controls/GoBack").GetComponent<UIControllerWrapper>().setSelected(true);
        isInControlsMenu = true;
    }

    public void GoBackToPauseMenu()
    {
        gameObject.transform.Find("DefaultMenu").gameObject.SetActive(true);
        gameObject.transform.Find("ControlsMenu/Controls").gameObject.SetActive(false);
        gameObject.transform.Find("ControlsMenu/Controls/GoBack").GetComponent<UIControllerWrapper>().setSelected(false);
        isInControlsMenu = false;
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
        currentButton = buttonsToPress[0];
        buttonInt = 0;
        currentButton.GetComponent<UIControllerWrapper>().setSelected(true);

        for (int i = 1; i < buttonsToPress.Length; i++)
        {
            buttonsToPress[i].GetComponent<UIControllerWrapper>().setSelected(false);
        }

        isPaused = true;
        for (int i = 0; i < UIElements.Length; i++)
        {
            UIElements[i].SetActive(true);
        }
    }

    public void invertCamera(GameObject invertButton)
    {
        if (!camInverted)
        {
            invertButton.transform.Find("Text_Right").GetComponent<Text>().text = "YES";
            camInverted = true;
        }
        else
        {
            invertButton.transform.Find("Text_Right").GetComponent<Text>().text = "NO";
            camInverted = false;
        }


        GameObject.Find("CameraRig").GetComponent<FollowCamera>().toggleInvert();
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Title");
    }

}
