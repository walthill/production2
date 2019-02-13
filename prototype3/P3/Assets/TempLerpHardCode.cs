using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempLerpHardCode : MonoBehaviour
{
    bool keepMoving = false;
    public Transform target;
    public Transform BoomBox;
    public float LerpSpeed = .5f;
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
        if (keepMoving)
        {
            // Distance moved = time * speed.
            float distCovered = (Time.time - startTime) * LerpSpeed;

            // Fraction of journey completed = current distance divided by total distance.
            float fracJourney = distCovered / journeyLength;

            // Set our position as a fraction of the distance between the markers.
            transform.position = Vector3.Lerp(BoomBox.position, target.position, fracJourney);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "boomBox"){
            keepMoving = true;
        }
        if (other.tag == "end")
        {
            keepMoving = false;
        }
    }
}
