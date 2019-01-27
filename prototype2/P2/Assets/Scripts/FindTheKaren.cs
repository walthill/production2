using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindTheKaren : MonoBehaviour
{
    [SerializeField]
    float karenTimer, karenTimeBonus, karenTimeLoss;
    [SerializeField]
    int karenMatchesToWin;

    [SerializeField]
    GameObject[] miniGameObjs;
    [SerializeField]
    Material phoneRingingMaterial, correctMaterial;

    Material originalMaterial;

    [SerializeField]
    float phoneRingingDelay;

    FindTheImage normalMinigame;

    bool started = false, timeReset = false, wrong = false, right = false;
    float timer, timeLeft = 3;

    int phoneIndex = -1, oldPhoneIndex = -2;

    void Awake()
    {
        originalMaterial = miniGameObjs[0].GetComponent<Renderer>().material;

        foreach (GameObject obj in miniGameObjs)
            obj.SetActive(false);

        normalMinigame = gameObject.GetComponent<FindTheImage>();
        timer = timeLeft;
    }

    void Update()
    {
        //begin minigame
        if(normalMinigame.won)
        {
            Camera.main.backgroundColor = Color.red;
            normalMinigame.winAndLoseText.text = "KAREN";
            if (timer < 0)
            {
                foreach (GameObject obj in miniGameObjs)
                    obj.SetActive(true);

                normalMinigame.won = false;
                normalMinigame.enabled = true;
                normalMinigame.ResetMatches();
                normalMinigame.SetMatchesToWin(karenMatchesToWin);
                normalMinigame.ResetTimer(karenTimer);
                normalMinigame.InitImages();

                started = true;
            }
            else
            {
                timer -= Time.deltaTime;
            }

            /**/
        }

        //logic
        if(started && normalMinigame.CheckWins() != 2)
        {
            if(!timeReset)
            {
                timer = phoneRingingDelay;
                timeReset = true;

                while (oldPhoneIndex == phoneIndex)
                    phoneIndex = Random.Range(0, miniGameObjs.Length);

                oldPhoneIndex = phoneIndex;

                if (phoneIndex == 0)
                {
                    miniGameObjs[0].GetComponent<Renderer>().material = phoneRingingMaterial;
                    miniGameObjs[1].GetComponent<Renderer>().material = originalMaterial;
                    miniGameObjs[2].GetComponent<Renderer>().material = originalMaterial;

                }
                else if (phoneIndex == 1)
                {
                    miniGameObjs[0].GetComponent<Renderer>().material = originalMaterial;
                    miniGameObjs[1].GetComponent<Renderer>().material = phoneRingingMaterial;
                    miniGameObjs[2].GetComponent<Renderer>().material = originalMaterial;

                }
                else if (phoneIndex == 2)
                {
                    miniGameObjs[0].GetComponent<Renderer>().material = originalMaterial;
                    miniGameObjs[1].GetComponent<Renderer>().material = originalMaterial;
                    miniGameObjs[2].GetComponent<Renderer>().material = phoneRingingMaterial;

                }
            }

            if (timer < 0)
            {
                if(!wrong && !right) //Decrease time
                    normalMinigame.DecreaseTime(karenTimeLoss);

                timeReset = false;
            }
            else
            {
                timer -= Time.deltaTime;
                CheckInput();
            }
        }
    }

    void CheckInput()
    {

        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            if (phoneIndex == 0)
            {
                normalMinigame.MadeAMatch();
                miniGameObjs[phoneIndex].GetComponent<Renderer>().material = correctMaterial;
                right = true;
            }
            else
            {
                miniGameObjs[0].GetComponent<Renderer>().material.color = Color.red;
                miniGameObjs[1].GetComponent<Renderer>().material = originalMaterial;
                miniGameObjs[2].GetComponent<Renderer>().material = originalMaterial;
                wrong = true;
                right = false;
                normalMinigame.DecreaseTime(karenTimeLoss);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            if (phoneIndex == 1)
            {
                normalMinigame.MadeAMatch();
                miniGameObjs[phoneIndex].GetComponent<Renderer>().material = correctMaterial;
                right = true;
            }
            else
            {
                miniGameObjs[1].GetComponent<Renderer>().material.color = Color.red;
                miniGameObjs[0].GetComponent<Renderer>().material = originalMaterial;
                miniGameObjs[2].GetComponent<Renderer>().material = originalMaterial;
                wrong = true;
                right = false;
                normalMinigame.DecreaseTime(karenTimeLoss);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            if (phoneIndex == 2)
            {
                normalMinigame.MadeAMatch();
                miniGameObjs[phoneIndex].GetComponent<Renderer>().material = correctMaterial;
                right = true;
            }
            else
            {
                miniGameObjs[2].GetComponent<Renderer>().material.color = Color.red;
                miniGameObjs[1].GetComponent<Renderer>().material = originalMaterial;
                miniGameObjs[0].GetComponent<Renderer>().material = originalMaterial;
                right = false;
                wrong = true;
                normalMinigame.DecreaseTime(karenTimeLoss);
            }
        }

    }
}
