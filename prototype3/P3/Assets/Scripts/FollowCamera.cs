using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    /*
     * Camera help
     * https://gamedev.stackexchange.com/questions/114742/how-can-i-make-camera-to-follow-smoothly
     * https://answers.unity.com/questions/600577/camera-rotation-around-player-while-following.html?page=1&pageSize=5&sort=votes
    */

    //TODO: pull back camera on speed increase
    //TODO: more control over camera offset

    /*
     *  https://github.com/DieselPrius/Unity-Third-Person-Camera-Script/blob/master/ThirdPersonCamera.cs
     *  
     *  //if a collider is blocking the line of sight of the camera snap the camera in front of the collider ...
        if (Physics.Raycast(parentTransform.position, playerToCamera, out hitInfo, playerToCamera.magnitude)) 
        {
            gameObject.transform.position = hitInfo.point;
        }
    */

    const float MODEL_ROTATION_FACTOR = 75f;

    [SerializeField]
    GameObject target;
    [SerializeField]
    float dampingValue;
    [SerializeField]
    float xOffsetValue, yOffsetValue, zOffsetValue;

    Vector3 offset;
    PlayerSkateMovement playerMove;

    void Awake()
    {
        offset = target.transform.position - gameObject.transform.position;

        playerMove = target.GetComponent<PlayerSkateMovement>();
    }

    void FixedUpdate()
    {
        float targetAngle, currentAngle, angle;

        currentAngle = gameObject.transform.eulerAngles.y;
        targetAngle = target.transform.eulerAngles.y - MODEL_ROTATION_FACTOR; 

        angle = Mathf.LerpAngle(currentAngle, targetAngle, Time.deltaTime * dampingValue);

        Quaternion rotation = Quaternion.Euler(0, angle, 0);
        gameObject.transform.position = target.transform.position - (rotation * offset);
  
        //create new transform
        Vector3 t = new Vector3(target.transform.position.x + xOffsetValue, target.transform.position.y + yOffsetValue, target.transform.position.z + zOffsetValue);
        transform.LookAt(t);
    }
}
