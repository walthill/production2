using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaliScript : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isCurved;
    public Transform[] railPoints;
    public float timeOnRail;
    public Collider railCollider;
    public float nextPointThreshold = .5f;

    bool playerIsOnRail = false;
    GameObject player = null;
    Vector3 startPoint, nextPoint;

    float railDistance, startTime;
    float speed;
    int iterPoint = 0;

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
            Debug.Log("Riding Rail");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            player = other.gameObject;
            playerIsOnRail = true;
            startTime = Time.time;
            speed = player.GetComponent<Rigidbody>().velocity.magnitude;


            startPoint = railPoints[0].position;
            nextPoint = railPoints[1].position;

            for(int i = 0; i< railPoints.Length; i++)
            {
                Debug.Log(railPoints[i].position.ToString());
            }
        }
       
    }

    void rideRail()
    {
        float distCovered = (Time.time - startTime) * speed;
        float fracJourney = distCovered / railDistance;
        if (!isCurved)
        {
            player.GetComponent<Transform>().position = Vector3.Lerp(startPoint, nextPoint, fracJourney);
        }
        else
        {
            player.GetComponent<Transform>().position = Vector3.Slerp(startPoint, nextPoint, fracJourney);
        }

        if (Vector3.Distance(player.GetComponent<Transform>().position, nextPoint) <= nextPointThreshold)
        {
            iterPoint++;
            if (iterPoint <= railPoints.Length)
            {
                goToNextPoint();
            }
            else
            {
                playerIsOnRail = false;
            }
        }
    }

    void goToNextPoint()
    {
        startPoint = railPoints[iterPoint].position;
        nextPoint = railPoints[iterPoint + 1].position;
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
