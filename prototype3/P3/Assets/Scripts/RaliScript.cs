using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaliScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform[] railPoints;
    public float timeOnRail;

    bool playerIsOnRail = false;
    GameObject player = null;
    Vector3 startPoint, endPoint;

    float railDistance, startTime;
    float speed;

    void Start()
    {
        railDistance = Vector3.Distance(railPoints[0].position, railPoints[1].position);
    }

    // Update is called once per frame
    void Update()
    {
        if(playerIsOnRail && player != null)
        {
            rideRail();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            player = other.gameObject;
            playerIsOnRail = true;
            startTime = Time.time;
            speed = player.GetComponent<Rigidbody>().velocity.z;
        }
    }

    void rideRail()
    {
        float distCovered = (Time.time - startTime) * speed;
        float fracJourney = distCovered / railDistance;
        player.GetComponent<Transform>().position = Vector3.Lerp(startPoint, endPoint, fracJourney);
    }

//GET AND SET

    public void setOnRail(bool status)
    {
        playerIsOnRail = status;
    }

    public void toggleOnRail()
    {
        playerIsOnRail = !playerIsOnRail;
    }

}
