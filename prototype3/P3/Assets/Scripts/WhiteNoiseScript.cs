using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteNoiseScript : MonoBehaviour
{
    public int animCycles = 0;
    public int cyclesNeeded;

    private void Update()
    {
        if (animCycles >= cyclesNeeded)
        {
            //animCycles = 0;
            gameObject.transform.parent.parent.gameObject.SetActive(false);
        }
    }

    public void AddOne()
    {
        animCycles++;
    }
}
