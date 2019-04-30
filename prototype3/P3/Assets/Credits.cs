using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Credits : MonoBehaviour
{
    [SerializeField] Scrollbar horizontalScroll;
    [SerializeField] float scrollSpeed, waitToScrollTime = 2.5f;

    float scrollVal = 0;
    bool scrollLeft, canScroll;

    const float MAX_VALUE = 1.0f;
    const float MIN_VALUE = 0.25f;

    void Awake()
    {
        scrollVal = MIN_VALUE;
        horizontalScroll.value = scrollVal;
    }

    void Update()
    {
        TrackScrolling();
        DoScrolling();    
    }

    void TrackScrolling()
    {
        scrollVal = horizontalScroll.value;
        if (!scrollLeft && scrollVal <= MIN_VALUE)
        {
            scrollLeft = true;
            canScroll = false;
            StartCoroutine(WaitAndScroll(waitToScrollTime));
        }
        else if (scrollLeft && scrollVal >= MAX_VALUE)
        {
            canScroll = false;
            scrollLeft = false;
            StartCoroutine(WaitAndScroll(waitToScrollTime));
        }
    }

    void DoScrolling()
    {
        if (canScroll)
        {
            scrollVal = horizontalScroll.value;
            if (scrollLeft && scrollVal <= MAX_VALUE)
            {
                scrollVal += (Time.deltaTime * scrollSpeed);
            }
            else if (!scrollLeft && scrollVal > MIN_VALUE)
            {
                scrollVal -= (Time.deltaTime * scrollSpeed);
            }

            horizontalScroll.value = scrollVal;
        }
    }

    IEnumerator WaitAndScroll(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        canScroll = true;
    }
}
