using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    //Smooth camera vars
    [SerializeField] Transform target;
//    [SerializeField] Transform rotationTarget;

    [Header("Smooth Camera Values")]
    [SerializeField] float distance = 0;
    [SerializeField] float height = 0;
    [SerializeField] float damping = 0;
    [SerializeField] float rotationDamping = 0;
    [SerializeField] Vector3 lookAtOffset;
    [SerializeField] bool applyRotationDamp;

    //Camera collision vars
    [Header("Camera Collision Values")]
    [SerializeField] float raycastLength = 0;
    [SerializeField] float collisionCameraHeight = 0;
    [SerializeField] Vector3 collisionRaycastOffset;

    //Camera knockback vars
    [Header("Camera Knockback")]
    [SerializeField] float timer = 0;
    [SerializeField] float knockbackTime = 0;
    [SerializeField] float knockbackSpeed = 0;
    [SerializeField] float returnSpeed = 0;
    [SerializeField] float knockbackDistance = 0;
    [SerializeField] float returnDistance = 0;

    
    float distanceToReach;
    bool hasKnockback = false;
    [SerializeField] float camRotationSpeed=0;
    [SerializeField] bool looking = false;

    [SerializeField] float /* camTurnLeftBound, camTurnRightBound,*/ camLookDownBound=0;

    float camYRotation;
    float camXRotation;
    bool ableToLookBehind;

    Quaternion originalParentRotation;

    float deadzone = 0.3f;
    float baseYRotation;

    private void Awake()
    {
       // originalParentRotation = transform.parent.rotation;
        distanceToReach = distance + knockbackDistance;
        returnDistance = returnDistance + distance;
    }

    private void Update()
    {
        camYRotation = Input.GetAxis("JoyHorizontalRS");
        camXRotation = Input.GetAxis("JoyVerticalRS");

        if (camYRotation != 0 || camXRotation != 0) 
            looking = true;
        else 
            looking = false;
    }

    private void FixedUpdate()
    {
        if (target && !looking)
        {
            ableToLookBehind = true;

           // transform.parent.gameObject.GetComponent<CameraRigFollow>().AlignRotation();

            Vector3 wantedPosition = target.transform.TransformPoint(new Vector3(0, height, -distance));
            Vector3 backDirection = target.transform.TransformDirection(-1 * Vector3.forward);

            Ray ray = new Ray(target.TransformPoint(collisionRaycastOffset), backDirection);
            RaycastHit hitInfo;

            //Camera collisions
           /* if (Physics.Raycast(ray, out hitInfo, raycastLength)) //TODO: shoot ray toward camera instead of straight back 
            {
                if (hitInfo.transform != target) //make sure collision isn't the player
                {
                    // Debug.Log("raycast hit" + hitInfo.collider.name);

                    wantedPosition = new Vector3(hitInfo.point.x,
                                                 Mathf.Lerp(hitInfo.point.y + collisionCameraHeight, wantedPosition.y, Time.deltaTime * damping),
                                                 hitInfo.point.z);
                }
            }*/

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

           // baseYRotation = transform.parent.localEulerAngles.y;
           // Debug.Log("ROT START: " + baseYRotation);
            
            CameraFallback();

        }
        else if (looking) //TODO: rotate target gameobject
        {
            if (camXRotation > deadzone) //Pan camera over top of player
            {
                transform.Rotate(camXRotation * camRotationSpeed, 0, 0.0f);

                if (transform.parent.localEulerAngles.x > camLookDownBound)
                {
                    transform.parent.localEulerAngles = new Vector3(camLookDownBound, transform.parent.localEulerAngles.y, transform.parent.localEulerAngles.z);
                }
            }
            else if(camXRotation < -deadzone) //Look beind
            {
                if(ableToLookBehind)
                {
                    float behind = transform.localEulerAngles.y + 180;
                    transform.localEulerAngles = new Vector3(0, behind, 0);
                    ableToLookBehind = false;
                }
            }
            else if (camYRotation > deadzone || camYRotation < -deadzone) //TODO: clamp cam rotation?
            {
                transform.Rotate(0, camYRotation * camRotationSpeed, 0.0f);               
            }
            else
            {
                //Debug.Log("RESET ROTATION");
            }

            Vector3 wantedPosition = target.transform.TransformPoint(new Vector3(0, height, -distance));
            Vector3 backDirection = target.transform.TransformDirection(-1 * Vector3.forward);

            Ray ray = new Ray(target.TransformPoint(collisionRaycastOffset), backDirection);
            RaycastHit hitInfo;

            Debug.DrawRay(ray.origin,ray.direction, Color.green, 2, false);
            
            //Camera collisions
           /* if (Physics.Raycast(ray, out hitInfo, raycastLength))
            {
                if (hitInfo.transform != target) //make sure collision isn't the player
                {
                    Debug.Log("raycast hit" + hitInfo.collider.name);

                    wantedPosition = new Vector3(hitInfo.point.x,
                                                 Mathf.Lerp(hitInfo.point.y + collisionCameraHeight, wantedPosition.y, Time.deltaTime * damping),
                                                 hitInfo.point.z);

                    transform.position = Vector3.Lerp(transform.position, wantedPosition, Time.deltaTime * damping);
                    Vector3 lookAtPosition = target.TransformPoint(lookAtOffset);
                }  
            }*/
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
