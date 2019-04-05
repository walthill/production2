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

    Vector3 barFillLocalScale;
    float alphaTime;
    bool waitForButton;

    void Awake()
    {
        alphaTime = 0;
        waitForButton = false;
        startButton.color = Vector4.zero;
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

    public bool Hide()
    {
        //Wait for input
        isLoading = false;
        if (Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            gameObject.SetActive(false);

            currentLoadingOperation = null;
            return true;
        }

        return false;
    }
}
