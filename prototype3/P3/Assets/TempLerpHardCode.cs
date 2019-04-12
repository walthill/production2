using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempLerpHardCode : MonoBehaviour
{
    Rigidbody rb;
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
        rb = GetComponent<Rigidbody>();
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
            /*
            // Distance moved = time * speed.
            float distCovered = (Time.time - startTime) * LerpSpeed;

            // Fraction of journey completed = current distance divided by total distance.
            float fracJourney = distCovered / journeyLength;

            // Set our position as a fraction of the distance between the markers.
            transform.position = Vector3.Lerp(BoomBox.position, target.position, fracJourney*Time.deltaTime);
            */
            
            // Distance moved = time * speed.
            float distCovered = (Time.time - startTime) * LerpSpeed;

            // Fraction of journey completed = current distance divided by total distance.
            float fracJourney = distCovered / journeyLength;


            float BoomX = transform.position.x;
            float BoomY = transform.position.y;
            float Boomz = transform.position.z;
            float TargetY = target.transform.position.y;
            float TargetX = target.transform.position.x;
            float Targetz = target.transform.position.z;

            // Set our position as a fraction of the distance between the markers.
            transform.position = new Vector3(Mathf.Lerp(BoomX, TargetX, fracJourney * Time.deltaTime), Mathf.Lerp(BoomY, TargetY, fracJourney * Time.deltaTime), Mathf.Lerp(Boomz, Targetz, fracJourney * Time.deltaTime));
            
        }
    }
    /*
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
    
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag== "boomBox")
        {
            Debug.Log("you hit the boom box");
            keepMoving = true;
        }
        if (collision.collider.tag == "end")
        {
            keepMoving = false;
        }
    }
    */
    public void keepMovingTru()
    {
        //GetComponent<PlayerSkateMovement>().enabled=false;
        rb.useGravity = false;
        keepMoving = true;

    }

    public void keepMovingFal()
    {
        //GetComponent<PlayerSkateMovement>().enabled = true;
        keepMoving = false;
        rb.useGravity = true;
    }
}
