using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpeedChannel { QUICK, SPEEDY, FAST, BLUR, LIGHTNING, NUM_SPEEDS }

public enum ObstacleType
{
    UNDEFINED = -1, JUMP, GRIND, LOOP, BOOST, NUM_OBSTACLES
}

public class SpeedGate : MonoBehaviour
{
    PlaceObstacle placeObstacle;
    ObstacleType obstacleType;
    SpeedChannel speedChannel;

    private void Awake()
    {
        placeObstacle = gameObject.GetComponent<PlaceObstacle>();
        obstacleType = placeObstacle.obstacleType;
        speedChannel = placeObstacle.speedChannel;
    }

    void Update()
    {
        if(obstacleType == ObstacleType.JUMP)
        {

        }
        else if (obstacleType == ObstacleType.GRIND)
        {

        }
        else if (obstacleType == ObstacleType.LOOP)
        {

        }
        else if (obstacleType == ObstacleType.BOOST)
        {

        }
        else if (obstacleType == ObstacleType.UNDEFINED || obstacleType == ObstacleType.NUM_OBSTACLES)
        {

        }
    }
}
