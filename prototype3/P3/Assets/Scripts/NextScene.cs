using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    public string sceneName;
    public bool willQuitTheGame = false;

    int leosSpecialInt = 0;

    public GameObject tvDude;
    public GameObject lines;
    public GameObject title;

	public void Awake()
	{
		Cursor.visible = false;
	}
	
    public void GoToNextScene()
    {
        if (willQuitTheGame)
        {
            Application.Quit();
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    public void MoveEverything()
    {
        //if ((gameObject.name == "PlayButton" && leosSpecialInt == 2) || (gameObject.name == "ControlsButton" && leosSpecialInt == 1) || (gameObject.name == "QuitButton" && leosSpecialInt == 0))
        //{
            tvDude.transform.parent = this.transform;
            tvDude.transform.SetSiblingIndex(0);

            lines.transform.parent = this.transform;
            lines.transform.SetSiblingIndex(0);

            title.transform.parent = this.transform;
            title.transform.SetSiblingIndex(0);

            transform.SetSiblingIndex(6);
        //}
        //else
        //{
        //    leosSpecialInt++;
        //}
    }
}
