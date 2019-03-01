using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    //Smooth camera vars
    [SerializeField] Transform target;
//    [SerializeField] Transform rotationTarget;

    [Header("Smooth Camera Values")]
    [SerializeField] float distance;
    [SerializeField] float height;
    [SerializeField] float damping;
    [SerializeField] float rotationDamping;
    [SerializeField] Vector3 lookAtOffset;
    [SerializeField] bool applyRotationDamp;

    //Camera collision vars
    [Header("Camera Collision Values")]
    [SerializeField] float raycastLength;
    [SerializeField] float collisionCameraHeight;
    [SerializeField] Vector3 collisionRaycastOffset;

    //Camera knockback vars
    [Header("Camera Knockback")]
    [SerializeField] float timer;
    [SerializeField] float knockbackTime;
    [SerializeField] float knockbackSpeed;
    [SerializeField] float returnSpeed;
    [SerializeField] float knockbackDistance;
    [SerializeField] float returnDistance;

    
    float distanceToReach;
    bool hasKnockback = false;
    [SerializeField] float rotationSpeed;
    [SerializeField] bool looking = false;

    float camYRotation;
    float camXRotation;

    Quaternion originalParentRotation;

    float deadzone = 0.3f;
    private void Awake()
    {
        originalParentRotation = transform.parent.rotation;
        distanceToReach = distance + knockbackDistance;
        returnDistance = returnDistance + distance;
    }

    private void Update()
    {
        camYRotation = Input.GetAxis("JoyHorizontalRS");
        camXRotation = Input.GetAxis("JoyVerticalRS");

        if (camYRotation != 0 || camXRotation != 0) looking = true;
        else looking = false;
       
    }

    private void FixedUpdate()
    {
        if (target && !looking)
        {
            transform.parent.rotation = originalParentRotation;

            Vector3 wantedPosition = target.transform.TransformPoint(new Vector3(0, height, -distance));
            Vector3 backDirection = target.transform.TransformDirection(-1 * Vector3.forward);

            Ray ray = new Ray(target.TransformPoint(collisionRaycastOffset), backDirection);
            RaycastHit hitInfo;

            //Camera collisions
            if (Physics.Raycast(ray, out hitInfo, raycastLength))
            {
                if (hitInfo.transform != target) //make sure collision isn't the player
                {
                    // Debug.Log("raycast hit" + hitInfo.collider.name);

                    wantedPosition = new Vector3(hitInfo.point.x,
                                                 Mathf.Lerp(hitInfo.point.y + collisionCameraHeight, wantedPosition.y, Time.deltaTime * damping),
                                                 hitInfo.point.z);
                }
            }

            transform.position = Vector3.Lerp(transform.position, wantedPosition, Time.deltaTime * damping);

            Vector3 lookAtPosition = target.TransformPoint(lookAtOffset);

            if (applyRotationDamp)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookAtPosition - transform.position, target.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationDamping);
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);//transform.eulerAngles.z = 0;
            }
            else
            {
                transform.rotation = Quaternion.LookRotation(lookAtPosition - transform.position, target.up);
            }

            CameraFallback();
        }
        else if (looking)
        {
            // Debug.Log("looking");
            //TODO: clamp rotation on the X and Y

            //TODO: set camera rig facing to be same as player

            if (camXRotation > deadzone || camXRotation < -deadzone)
            {
                transform.parent.Rotate(camXRotation * rotationSpeed, 0, 0.0f);
            }
            else if (camYRotation > deadzone || camYRotation < -deadzone)
            {
                transform.parent.Rotate(0, -camYRotation * rotationSpeed, 0.0f);
            }
            else
            {
                Debug.Log("RESET ROTATION");
            }
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
        else if (!hasKnockback && distance > returnDistance)
        {
            distance = Mathf.Lerp(distance, returnDistance, Time.deltaTime * returnSpeed);
            ToggleSpeedUI();
        }
    }

    public void ToggleKnockback()
    {
        hasKnockback = true;
    }

    public void ToggleSpeedUI()
    {
        //TEMP - need to separate UI code out into own class (see todo in SurfaceSpeedBoost)
        target.gameObject.GetComponent<SpeedSurfaceBoost>().speedIndicator.gameObject.SetActive(false);
        target.gameObject.GetComponent<SpeedSurfaceBoost>().speedText.gameObject.SetActive(false);
    }
}
