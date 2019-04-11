using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DisplayScore : MonoBehaviour
{
    [SerializeField] Text scoreText,timeText;
    [SerializeField] Color[] scoreColors;

    Animator winScreenAnim;
    int totalScore;

    void Awake()
    {
        winScreenAnim = gameObject.GetComponent<Animator>();
        SpeedChannel ch = ScoreBoi.instance.GetHighestSpeedChannel();
        scoreText.color = scoreColors[(int)ch - 1];
        timeText.text = ScoreBoi.instance.GetTime();
        totalScore = ScoreBoi.instance.CalculateScore();
    }

    //Called from animation trigger
    public void PlayerWin()
    {
        //Display score text in color of the coressponding channel
        StartCoroutine(CountScore(0.0001f));
    }

    IEnumerator CountScore(float timeBetweenText)
    {
        int counter = 0;
        for (int i = 0; i < totalScore; i++)
        {
            if (counter < totalScore)
            {
                counter += 21;
                scoreText.text = counter.ToString();
                yield return new WaitForSeconds(timeBetweenText);
            }
            else
            {
                counter = totalScore;
                scoreText.text = counter.ToString();
                break;
            }
        }

        yield return new WaitForSeconds(1.5f);
    }
}
