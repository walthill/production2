using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoi : MonoBehaviour
{
    public static ScoreBoi instance;

    [SerializeField] bool isGameRunning;
    [SerializeField] Text timerText = null;
    [SerializeField] int itemsCollected = 0;

    [Header("Scoring Data")]
    [SerializeField] int[] bestTimesPerSpeed = null; //element 0 is Speed Channel 2

    [SerializeField] int baseScore = 5000;
    [SerializeField] int timePenalty = 66;
    [SerializeField] float scoreBonus = 1.25f;

    float timeElapsed;
    int seconds, minutes, totalSeconds;
  
    /*      SCORING
     *      
     *      Variables Needed:
     *          int :: "Collectables" found - can be music or just coin-like things?
     *          int :: seconds
     *          int :: max speed channel
     *      
     *      Player Highest Possible Base Score - 30,000
     *      
     *      Collectables Bonus: Score *= (1.25* num of collectable found)
     *      
     *      All scoring is currently calculated through number of seconds to finish, collectables found, 
     *      and the channel the player finished at.
     *      
     *      To convert seconds into score, start with score value at max. Decrement every (x) seconds  
     *      
     *      [Best times currently in editor]
     *      CH 2
     *          Multiplier: 1 * 5,000 (where 5000 is the best time)
     *          Best Time: under 30 seconds
     *      CH 3
     *          Multiplier: 2 * 5,000
     *          Best Time: under 90 seconds
     *      CH 4
     *          Multiplier: 3 * 5,000
     *          Best Time: under 240 seconds
     *      CH 5
     *          Multiplier: 4 * 5,000
     *          Best Time: under 300 seconds
     *      CH 6
     *          Multiplier: 5 * 5,000 
     *          Best Time: under 420 seconds
     *      CH 7
     *          Multiplier: 6 * 5,000 
     *          Best Time: under 480 seconds
     * 
     */

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        GameClock();
    }

    void GameClock()
    {
        if(isGameRunning)
        {
            timeElapsed += Time.deltaTime;

            seconds = Mathf.FloorToInt(timeElapsed);
            totalSeconds = seconds;

            if (seconds == 60)
            {
                minutes += 1;
                timeElapsed = 0;
                seconds = 0;
            }

            if (seconds < 10)
                timerText.text = minutes + ":0" + seconds;
            else
                timerText.text = minutes + ":" + seconds;
        }
    }

    public int CalculateScore()
    {
        SpeedChannel ch = PlayerSceneRelay.instance.getMaxSpeedChannel();
        float collectablesMultiplier = 1;
        int speedMultiplier = 1;
        int score = 0, totalScore = -1;

        //Speed Multiplier
        if (ch == SpeedChannel.SPEEDY)
            speedMultiplier = 1;
        else if (ch == SpeedChannel.FAST)
            speedMultiplier = 2;
        else if (ch == SpeedChannel.BLUR)
            speedMultiplier = 3;
        else if (ch == SpeedChannel.LIGHTNING)
            speedMultiplier = 4;
        else if (ch == SpeedChannel.WOW_SO_FAST)
            speedMultiplier = 5;
        else if (ch == SpeedChannel.LIVE_IN_DARKNESS)
            speedMultiplier = 6;

        //Time Multiplier
        if (speedMultiplier == 1)
        {
            score += CheckForBestTime(0); //Index of speed channel 2 minus 1

            if(score == 0)
            {
                score += CalculateTimePenalty();
            }
        }
        else if (speedMultiplier == 2)
        {
            score += CheckForBestTime(1);

            if (score == 0)
            {
                score += CalculateTimePenalty();
            }
        }
        else if (speedMultiplier == 3)
        {
            score += CheckForBestTime(2);

            if (score == 0)
            {
                score += CalculateTimePenalty();
            }
        }
        else if (speedMultiplier == 4)
        {
            score += CheckForBestTime(3);

            if (score == 0)
            {
                score += CalculateTimePenalty();
            }
        }

        else if (speedMultiplier == 5)
        {
            score += CheckForBestTime(4);
            if (score == 0)
            {
                score += CalculateTimePenalty();
            }
        }
        else if (speedMultiplier == 6)
        {
            score += CheckForBestTime(5);
            if (score == 0)
            {
                score += CalculateTimePenalty();
            }
        }

        //Collectables Multiplier
        if(itemsCollected > 0)
            collectablesMultiplier = scoreBonus * itemsCollected;

        totalScore = score;
        totalScore *= (int)collectablesMultiplier;
        totalScore *= speedMultiplier;
        return totalScore;
    }


    int CheckForBestTime(int speedChannelIndex)
    {
        int score = 0;

        if (totalSeconds < bestTimesPerSpeed[speedChannelIndex])
            score += 5000;

        return score;  
    }

    int CalculateTimePenalty()
    {
        //convert seconds over best time into score

        //For every 5 seconds over best time, decrement score by 66
        int fiveSecondIntervals = seconds % 5;
        int bestPossibleScore = baseScore;

        for (int i = 0; i < fiveSecondIntervals; i++)
            bestPossibleScore -= timePenalty;

        return bestPossibleScore;
    }

 
    public void HideClock()
    {
        timerText.gameObject.SetActive(false);
    }
    public void ShowClock()
    {
        timerText.gameObject.SetActive(true);
    }

    public void RunGameClock()
    {
        isGameRunning = true;
    }
    public void StopClock()
    {
        isGameRunning = false;
    }

    public string GetTime()
    {
        return timerText.text;
    }
}
