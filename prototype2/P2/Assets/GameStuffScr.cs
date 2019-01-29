using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStuffScr : MonoBehaviour
{
    public static GameStuffScr instance;
    public Text timTXT;
    public Text scoreTXT;
    public int Score = 0;
    public float Timer = 30;


    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
        
        scoreTXT.text = ("" + Score);
        if (Timer < 0)
        {
            timTXT.text = ("You lose, sorry my dude");
        }
        if (Score > 30)
        {
            timTXT.text = ("You won, nice.");

        }
        if (Score < 30&&Timer>0)
        {
            Timer -= Time.deltaTime;
            timTXT.text = ("" + Timer);
        }
    }
    

    public void addScore()
    {
        Score++;
    }
    public void subScore()
    {
        Score--;
    }


    public void addTime()
    {
        Timer=Timer+3;
    }

    public void subTimer()
    {
        Timer--;
    }
}
