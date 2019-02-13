using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempLerpHardCode : MonoBehaviour
{
    bool keepMoving = false;
    public Transform target;
    public Transform BoomBox;
    public float LerpSpeed = 1;
    // Time when the movement started.
    private float startTime;
    // Total distance between the markers.
    private float journeyLength;

    // Start is called before the first frame update
    void Start()
    {
        // Keep a note of the time the movement started.
        startTime = Time.time;

        // Calculate the journey length.
        journeyLength = Vector3.Distance(target.position, BoomBox.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
