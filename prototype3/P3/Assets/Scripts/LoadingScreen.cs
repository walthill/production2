using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    //Tut @ https://www.windykeep.com/2018/02/15/make-loading-screen-unity/#siesta
    bool isLoading;
    AsyncOperation currentLoadingOperation;

    [SerializeField] RectTransform barFillRectTransform = null;
    [SerializeField] Text percentLoadedText = null;
    [SerializeField] Text startButton = null;
    [SerializeField] Vector3 textRGB = new Vector3(); //No alpha value as it fades in and out programatically
    [SerializeField] GameObject loadingBar;
    [SerializeField] GameObject pressAToContinue;

    Vector3 barFillLocalScale;
    float alphaTime;
    bool waitForButton, canPauseGame = false, readyToPlay = false, shouldHideLoadingBar = true;
	int numScenesToLoad, scenesLoaded = 0;

    void Awake()
    {
        alphaTime = 0;
        waitForButton = true;
        startButton.color = Vector4.zero;
        barFillLocalScale = barFillRectTransform.localScale;
        //Hide(); not sure why this is here, but don't uncomment it. When this runs, the player can pause the game in the loading screen
    }

    void Update()
    {
        if(isLoading)
        {
            SetProgress(currentLoadingOperation.progress);

            if(currentLoadingOperation.isDone)
            {
                //Hide();
                waitForButton = true;
            }
        }

        if (waitForButton)
        {
            //Flash Text
            alphaTime += Time.deltaTime;
            float textAlpha = Mathf.Abs(Mathf.Sin(alphaTime / 20.0f)) * 255.0f;
            startButton.color = new Vector4(textRGB.x, textRGB.y, textRGB.z, textAlpha);
            
        }
    }

    void SetProgress(float progress)
    {
		if(barFillLocalScale.x < 1)
		{
			float randomFillScale = Random.Range(15, 127);
			barFillLocalScale.x += progress/randomFillScale;
		}
		else
			barFillLocalScale.x = 1;
		
        barFillRectTransform.localScale = barFillLocalScale;

        percentLoadedText.text = Mathf.CeilToInt(progress * 100).ToString() + "%";
		
        if (Mathf.CeilToInt(progress * 100) >= 99 && barFillLocalScale.x == 1)
        {
			
			if(scenesLoaded == numScenesToLoad && shouldHideLoadingBar)
			{
				shouldHideLoadingBar = false;
				StartCoroutine(WaitAndHideLoadBar(Random.Range(0.5f, 1f)));
			}
		}
    }
	
	IEnumerator WaitAndHideLoadBar(float waitTime)
	{
			yield return new WaitForSecondsRealtime(waitTime);
			loadingBar.SetActive(false);
			pressAToContinue.SetActive(true);
			readyToPlay = true;
	}

    public void Show(AsyncOperation op)
    {
		scenesLoaded++;
        gameObject.SetActive(true);
        currentLoadingOperation = op;

        SetProgress(0);
        isLoading = true;
    }

    public void Hide()
    {
        SoundBoi.instance.driftReleaseSource.Play();
        gameObject.SetActive(false);
        isLoading = false;
        currentLoadingOperation = null;
		canPauseGame = true;
    }
	
	public void SetNumScenesToLoad(int amount)
	{
		numScenesToLoad = amount;
	}
	
	public bool ReadyToPlay()
	{
		return readyToPlay;
	}
	
	public bool AbleToPause()
	{
		return canPauseGame;
	}
}
