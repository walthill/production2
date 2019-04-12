using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControllerWrapper : MonoBehaviour
{
    public KeyCode[] accessableButtons;
    Button button;
    public bool isSelected;

    void Start()
    {
        button = this.gameObject.GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        if (checkforButtons() && isSelected)
        {
            button.onClick.Invoke();
        }
    }

    bool checkforButtons()
    {
        for(int i = 0; i < accessableButtons.Length; i++)
        {
            if (Input.GetKeyDown(accessableButtons[i]))
            {
                Debug.Log(accessableButtons[i].ToString());
                return true;
            }
        }
        return false;
    }

    public void setSelected(bool selected)
    {
        if (selected == true)
        {
            gameObject.transform.Find("Arrow").gameObject.SetActive(true);
            gameObject.transform.Find("Text_Left").GetComponent<Text>().color = new Color (89 / 255, 255 / 255, 69 / 255);
            gameObject.transform.Find("Text_Right").GetComponent<Text>().color = new Color(89 / 255, 255 / 255, 69 / 255);
        }
        else
        {
            gameObject.transform.Find("Arrow").gameObject.SetActive(false);
            gameObject.transform.Find("Text_Left").GetComponent<Text>().color = Color.white;
            gameObject.transform.Find("Text_Right").GetComponent<Text>().color = Color.white;

        }
        Debug.Log(gameObject.name + " is " + selected.ToString());
        isSelected = selected;
    }
}
