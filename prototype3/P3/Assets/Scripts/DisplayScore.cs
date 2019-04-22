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
	bool skipCounter = false, canSkip = true;
	
    void Awake()
    {
        winScreenAnim = gameObject.GetComponent<Animator>();
        SpeedChannel ch = ScoreBoi.instance.GetHighestSpeedChannel();
        scoreText.color = scoreColors[(int)ch - 1];
        timeText.text = ScoreBoi.instance.GetTime();
        totalScore = ScoreBoi.instance.CalculateScore(); 
    }

	void Update()
	{
		if(canSkip && Input.GetButtonDown("JoyJump"))
		{
			canSkip = false;
			//Debug.Log("skip score counter");
			skipCounter = true;
		}
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
			if(!skipCounter)
			{
				if (counter < totalScore)
				{
					counter += 76;
					scoreText.text = counter.ToString();
					yield return new WaitForSeconds(timeBetweenText);
				}
				else
				{
					counter = totalScore;
					scoreText.text = counter.ToString();
					canSkip = false;
					break;
				}
			}
			else
			{
				//Debug.Log("SKIP");
				scoreText.text = totalScore.ToString();
				
				skipCounter = false;
				break;
			}
		}
    }
	

	public void ReturnToMain()
	{
		if(!skipCounter && !canSkip)
			SceneManager.LoadScene("Title");
	}
}
