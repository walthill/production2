using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DisplayScore : MonoBehaviour
{
    [SerializeField] Text scoreText,timeText;

    Animator winScreenAnim;
    int totalScore;

    void Awake()
    {
        winScreenAnim = gameObject.GetComponent<Animator>();
    }

    //Called from animation trigger
    public void PlayerWin()
    {
        totalScore = ScoreBoi.instance.CalculateScore();
        Debug.Log(totalScore);

        timeText.text = ScoreBoi.instance.GetTime();

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
