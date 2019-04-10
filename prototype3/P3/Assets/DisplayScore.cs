using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DisplayScore : MonoBehaviour
{
    [SerializeField] Text scoreText;

    Animator winScreenAnim;
    int totalScore;

    // Start is called before the first frame update
    void Awake()
    {
        winScreenAnim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Called from animation trigger
    public void PlayerWin()
    {
        winScreenAnim.SetBool("win", true);
        totalScore = ScoreBoi.instance.CalculateScore();
        Debug.Log(totalScore);
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
                break;
            }
        }

        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("WinScreen", LoadSceneMode.Single);
    }
}
