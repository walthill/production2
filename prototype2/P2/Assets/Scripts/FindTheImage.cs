using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FindTheImage : MonoBehaviour
{
    public bool won = false;
    int winCounter;

    [SerializeField]
    GameObject leftSelect, rightSelect;
    [SerializeField]
    Material selectionMaterial;

    Material originalSelectorMat;

    [SerializeField]
    GameObject imageToFind, currentImage;

    Material currentMat, matToFind;

  // [SerializeField]
     int currentMatIndex;

    [SerializeField]
    Material[] imageMats;

    public Text timeText, scoreText, winAndLoseText;

    float selectTimer = 0.2f, countdown = 10.0f;
    int lastMatIndex, numMatches = 0;

    [SerializeField]
    int matchesToWin;

    public float timeBonus, timeLoss;

    bool isClockRunning = true;

    public void InitImages()
    {
        //set up image to find and current image
        currentMatIndex = Random.Range(0, imageMats.Length);
        imageToFind.GetComponent<Renderer>().material = imageMats[currentMatIndex];

        currentMatIndex = Random.Range(0, imageMats.Length);
        currentImage.GetComponent<Renderer>().material = imageMats[currentMatIndex];


        while (currentImage.GetComponent<Renderer>().material.name == imageToFind.GetComponent<Renderer>().material.name)
        {
            currentMatIndex = Random.Range(0, imageMats.Length);
            currentImage.GetComponent<Renderer>().material = imageMats[currentMatIndex];
        }

    }

    void Awake()
    {
        InitImages();

        originalSelectorMat = leftSelect.GetComponent<Renderer>().material;

        currentMat = currentImage.GetComponent<Renderer>().material;

        for (int i = 0; i < imageMats.Length; i++)
        {
         
            if (currentMat.name.Contains(imageMats[i].name))
            {
                currentMatIndex = i;
            }
        }

        lastMatIndex = imageMats.Length-1;
    //    Debug.Log(lastMatIndex);
    }

    void Update()
    {
        if (selectTimer < 0)
        {
            leftSelect.GetComponent<Renderer>().material = originalSelectorMat;
            rightSelect.GetComponent<Renderer>().material = originalSelectorMat;
            winAndLoseText.text = "";
        }
        else
        {
            selectTimer -= Time.deltaTime;
        }

        if (countdown < 0)
        {
            selectTimer = 100000f;
            winAndLoseText.text = "U LOSE";
        }
        else if(isClockRunning)
        {
            countdown -= Time.deltaTime;
            string textStr = countdown.ToString("F2");
            timeText.text = "Time Left: " + textStr;
        }

        CheckInput();
       
    }


    void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.A)) //left
        {
            rightSelect.GetComponent<Renderer>().material = originalSelectorMat;
            leftSelect.GetComponent<Renderer>().material = selectionMaterial;

            for(int i = 0; i < imageMats.Length; i++)
            {
                if (currentMatIndex == i)
                {
                    if (currentMatIndex == 0)
                    {
                        currentMatIndex = lastMatIndex;
                        break;
                    }
                    else
                    {
                        currentMatIndex--;
                        break;
                    }
                }
            }

            currentMat = imageMats[currentMatIndex];
            currentImage.GetComponent<Renderer>().material = currentMat;

            selectTimer = 0.2f;
        }
        else if (Input.GetKeyDown(KeyCode.D)) //right
        {
            leftSelect.GetComponent<Renderer>().material = originalSelectorMat;
            rightSelect.GetComponent<Renderer>().material = selectionMaterial;

            for (int i = 0; i < imageMats.Length; i++)
            {
                if (currentMatIndex == i)
                {
                    if (currentMatIndex == lastMatIndex)
                    {
                        currentMatIndex = 0;
                        break;
                    }
                    else
                    {
                        currentMatIndex++;
                        break;
                    }
                }
            }

            currentMat = imageMats[currentMatIndex];
            currentImage.GetComponent<Renderer>().material = currentMat;

            selectTimer = 0.2f;
        }
        else if (Input.GetKeyDown(KeyCode.Space)) //select
        {
            //correct selection
            if (currentMat.name.Contains(imageMats[currentMatIndex].name) && imageToFind.GetComponent<Renderer>().material.name.Contains(imageMats[currentMatIndex].name))
            {
                winAndLoseText.text = "nice";
                selectTimer = 0.15f;

                if (numMatches < matchesToWin-1)
                {
                    countdown += timeBonus;
                    numMatches++;
                    scoreText.text = "Matches: " + numMatches;
                    InitImages();
                }
                else
                {
                    winCounter++;

                    isClockRunning = false;

                    numMatches++;
                    scoreText.text = "Matches: " + numMatches;

                    winAndLoseText.text = "YOU WIN";
                    selectTimer = 100000f;

                    if(winCounter < 2)
                        won = true;

                    this.enabled = false;

                    Camera.main.backgroundColor = Color.blue;
                }
            }
            else // incorrect selection
            {
                selectTimer = 0.15f;
                winAndLoseText.text = "nope";
                countdown -= timeLoss;
            }
        }
    }

    public void DecreaseTime(float timeLost)
    {
        countdown -= timeLost;
    }

    public void AddTime(float timeGained)
    {
        countdown += timeGained;
    }
    public void ResetTimer(float time)
    {
        countdown = time;
        isClockRunning = true;
    }

    public void ResetMatches()
    {
        numMatches = 0;
        scoreText.text = "Matches: " + numMatches;
    }

    public void SetMatchesToWin(int numMatchesToWin)
    {
        matchesToWin = numMatchesToWin;
    }

    public void MadeAMatch()
    {
        numMatches++;
        scoreText.text = "Matches: " + numMatches;
    }

    public int CheckWins()
    {
        return winCounter;
    }

}
