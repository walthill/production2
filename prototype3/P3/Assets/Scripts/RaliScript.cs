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
    [Range(0.0f, 1.0f)]
    public float nextPointThreshold = .1f;

    bool playerIsOnRail = false;
    bool rideForwards = true;
    GameObject player = null;
    Vector3 startPoint, nextPoint;

    float railDistance, startTime;
    float speed;
    int iterPoint = 0;

    // Update is called once per frame
    void Update()
    {
        if(playerIsOnRail && player != null)
        {
            rideRail();
        }
    }

    void initGrind()
    {
        startTime = Time.time;
        speed = player.GetComponent<Rigidbody>().velocity.magnitude;
        startPoint = railPoints[iterPoint].position;
        if (rideForwards)
            nextPoint = railPoints[iterPoint + 1].position;
        else
            nextPoint = railPoints[iterPoint - 1].position;
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

        if (nextPointThreshold >= fracJourney)
        {
            goToNextPoint();
        }
    }

    void calcDistance()
    {
        if (rideForwards)
            railDistance = Vector3.Distance(railPoints[iterPoint].position, railPoints[iterPoint + 1].position);
        else
            railDistance = Vector3.Distance(railPoints[iterPoint].position, railPoints[iterPoint - 1].position);
    }

    void goToNextPoint()
    {
        if (rideForwards)
        {
            iterPoint++;
            if (iterPoint < railPoints.Length)
            {
                startPoint = railPoints[iterPoint].position;
                nextPoint = railPoints[iterPoint + 1].position;
            }
            else
            {
                playerIsOnRail = false;
            }
        }
        else
        {
            iterPoint--;
            if (iterPoint <= 0)
            {
                startPoint = railPoints[iterPoint].position;
                nextPoint = railPoints[iterPoint - 1].position;
            }
            else
            {
                playerIsOnRail = false;
            }
        }
    }

    public void startRailForward(Collider col)
    {
        if (!playerIsOnRail)
        {
            rideForwards = true;
            iterPoint = 0;
            player = col.gameObject;
            initGrind();
        }
        else
        {
            player = null;
            playerIsOnRail = false;
        }
    }

    public void startRailBackward(Collider col)
    {
        if (!playerIsOnRail)
        {
            rideForwards = false;
            iterPoint = railPoints.Length;
            player = col.gameObject;
            initGrind();
        }
        else
        {
            player = null;
            playerIsOnRail = false;
        }
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
