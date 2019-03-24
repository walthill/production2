using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    //Tut @ https://www.windykeep.com/2018/02/15/make-loading-screen-unity/#siesta

    bool isLoading;
    AsyncOperation currentLoadingOperation;

    [SerializeField] RectTransform barFillRectTransform;
    [SerializeField] Text percentLoadedText;

    Vector3 barFillLocalScale;

    void Awake()
    {
        barFillLocalScale = barFillRectTransform.localScale;
        Hide();
    }

    void Update()
    {
        if(isLoading)
        {
            SetProgress(currentLoadingOperation.progress);

            if(currentLoadingOperation.isDone)
            {
                //Hide();
            }
        }
    }

    void SetProgress(float progress)
    {
        barFillLocalScale.x = progress;
        barFillRectTransform.localScale = barFillLocalScale;

        percentLoadedText.text = Mathf.CeilToInt(progress * 100).ToString() + "%";
    }

    public void Show(AsyncOperation op)
    {
        gameObject.SetActive(true);
        currentLoadingOperation = op;

        SetProgress(0);
        isLoading = true;
    }

    public void Hide()
    {
        gameObject.SetActive(false);

        currentLoadingOperation = null;
        isLoading = false;
    }
}
