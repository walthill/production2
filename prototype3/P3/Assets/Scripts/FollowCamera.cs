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
    [SerializeField]
    bool shouldRotate = true;

    // The target we are following
    public Transform target;
    // The distance in the x-z plane to the target
    public float distance = 10.0f;
    // the height we want the camera to be above the target
    public float height = 5.0f;
    // How much we
    public float heightDamping = 2.0f;
    public float rotationDamping = 3.0f;

    [SerializeField]
    float yOffset =0;

    float wantedRotationAngle;
    float wantedHeight;
    float currentRotationAngle;
    float currentHeight;
    Quaternion currentRotation;

    [SerializeField]
    float timer = 0, knockbackTime =0,

    knockbackDistance = 0,     //value added to distance and lerped to communicate speed

    knockbackSpeed = 0,     //Speed of lerp on speed increase

    returnSpeed = 0;     //speed of lerp on speed decrease

    Camera cam;
    bool hasKnockback;
    float originalDistance, distanceToReach;
    Vector3 centerCamPos;
    RaycastHit hitInfo;

    private void Awake()
    {
        cam = gameObject.GetComponent<Camera>();

        float centerY = cam.scaledPixelHeight / 2;
        float centerX = cam.scaledPixelWidth / 2;
        centerCamPos = cam.ScreenToWorldPoint(new Vector3(centerX, centerY, 0));

        originalDistance = distance;
        distanceToReach = distance + knockbackDistance;
    }

    private void Update()
    {
        CameraFallback();
    }

    private void FixedUpdate()
    {
        /*Vector3 dir = gameObject.transform.position - target.position ;

        if (Physics.Raycast(target.position, dir, out hitInfo, dir.magnitude))
        {
            Debug.Log("ray hit. Need to move camera forward");
            //gameObject.transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y, hitInfo.point.z+10);
        }*/
    }

    void LateUpdate()
    {
        if (target)
        {
            // Calculate the current rotation angles
            wantedRotationAngle = target.eulerAngles.y-90;
            wantedHeight = target.position.y + height;
            currentRotationAngle = transform.eulerAngles.y;
            currentHeight = transform.position.y;

            // Damp the rotation around the y-axis
            currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);
            // Damp the height
            currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);
            // Convert the angle into a rotation
            currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);
            
            // Set the position of the camera on the x-z plane to:
            // distance meters behind the target
            transform.position = target.position;
            transform.position -= currentRotation * Vector3.forward * distance;
            
            // Set the height of the camera
            transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);
            
            // Always look at the target
            if (shouldRotate)
                transform.LookAt(new Vector3(target.position.x, target.position.y + yOffset, target.position.z));
        }

    }

    void CameraFallback()
    {
        if (hasKnockback)
        {
            timer += Time.deltaTime;
            distance = Mathf.Lerp(distance, distanceToReach, Time.deltaTime * knockbackSpeed);

            if (timer >= knockbackTime)
            {
                hasKnockback = false;
                timer = 0;
            }
        }
        else if (!hasKnockback && distance > originalDistance)
        {
            distance = Mathf.Lerp(distance, originalDistance, Time.deltaTime * knockbackSpeed);
        }
    }

    public void ToggleKnockback()
    {
        hasKnockback = true;
    }
}
