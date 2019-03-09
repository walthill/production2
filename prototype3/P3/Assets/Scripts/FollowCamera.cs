using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] Transform target;
 
    //Smooth camera vars
    [Header("Smooth Camera Values")]
    [SerializeField] float distance = 0;
    [SerializeField] float height = 0;
    [SerializeField] float damping = 0;
    [SerializeField] float rotationDamping = 0;
    [SerializeField] Vector3 lookAtOffset;
    [SerializeField] bool applyRotationDamp=true;

    //Camera collision vars
    [Header("Camera Collision Values")]
    [SerializeField] float raycastLength = 0;
    [SerializeField] float collisionCameraHeight = 0;
    [SerializeField] Vector3 collisionRaycastOffset;

    //Camera knockback vars
    [Header("Camera Knockback")]
    [SerializeField] float timer = 0; //tracks time spent in knockback
    [SerializeField] float knockbackTime = 0;
    [SerializeField] float knockbackSpeed = 0;
    [SerializeField] float returnSpeed = 0;
    [SerializeField] float knockbackDistance = 0;
    [SerializeField] float returnDistance = 0;

    //Free camera vars
    [Header("Camera Rotation")]
    [SerializeField] float inputDeadZone = 0;
    [SerializeField] float resetRotationSpeed=0; //Make this a large value
    [SerializeField] float camRotationSpeed = 0;
    [SerializeField] float camMinXAngle = 0, camMaxXAngle = 0;
    [SerializeField] float camMinYAngle = 0, camMaxYAngle = 0; //Clamp values for cam rotation

    float distanceToReach;
    bool hasKnockback = false;

    float camYRotation,  camXRotation; //camera rotate input
    bool lookBehindDown, lookBehindUp; //lookback input
    bool lookBehindToggle, canLookBack = true;

    Quaternion originalTargetRotation;

    float xRot, yRot;
    
    private void Awake()
    {
        originalTargetRotation = target.localRotation; //keep track of Quaternion at object rotation's 0,0,0
        
        //init knockback
        distanceToReach = distance + knockbackDistance;
        returnDistance = returnDistance + distance;
    }

    private void Update()
    {
        lookBehindDown = Input.GetButton("JoyRSDown");
        lookBehindUp = Input.GetButtonUp("JoyRSDown");

        camYRotation = Input.GetAxis("JoyHorizontalRS");
        camXRotation = Input.GetAxis("JoyVerticalRS");
    }

    private void FixedUpdate()
    {
        if(lookBehindDown)
            lookBehindToggle = true;
        if(lookBehindUp)
            lookBehindToggle = false;

        CameraMove();
    }

    void CameraMove()
    {
        if (target)
        {

            Vector3 wantedPosition = target.transform.TransformPoint(new Vector3(0, height, -distance));
            Vector3 backDirection = target.transform.TransformDirection(-1 * Vector3.forward);

            Ray ray = new Ray(target.TransformPoint(collisionRaycastOffset), backDirection);
            RaycastHit hitInfo;

            //Camera collisions - TODO(low): revisit this collision code 
            if (Physics.Raycast(ray, out hitInfo, raycastLength)) //TODO: shoot ray toward camera instead of straight back 
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

            FreeCameraMovement();
            CameraFallback();
        }
    }

    void FreeCameraMovement()
    {
        //Pan camera over/under player
        if (camXRotation > inputDeadZone || camXRotation < -inputDeadZone) 
        {
            xRot += camXRotation * camRotationSpeed;            
            xRot = Mathf.Clamp(xRot, camMinXAngle, camMaxXAngle);

            target.localEulerAngles = new Vector3(xRot, target.localEulerAngles.y, target.localEulerAngles.z);
        }
        //Move camera left/right around player
        else if (camYRotation > inputDeadZone || camYRotation < -inputDeadZone)
        {            
            yRot += camYRotation * camRotationSpeed;
            yRot = Mathf.Clamp(yRot, camMinYAngle, camMaxYAngle);

            target.localEulerAngles = new Vector3(target.localEulerAngles.x, yRot, target.localEulerAngles.z);
        }
        else if(lookBehindToggle) //Quick look back
        {
            if(canLookBack)
            {
                FlipAlignment(false);
            }
        }        
        else if(!canLookBack)
        {
            float zPosOffset = 2;
            FlipAlignment(true, zPosOffset);
        }
        else
        {
           ResetAlignment();
        }

    }

    void FlipAlignment(bool ableToLookBack, float lookBackZOffset=0)
    {
        float behind = target.localEulerAngles.y + 180;
        target.localEulerAngles = new Vector3(target.localEulerAngles.x, behind, target.localEulerAngles.z);
        target.localPosition = new Vector3(target.localPosition.x, target.localPosition.y, target.localPosition.z + lookBackZOffset);
        canLookBack = ableToLookBack;
    }

    void ResetAlignment()
    {
        xRot = 0;
        yRot = 0;
        target.localRotation = Quaternion.RotateTowards(target.localRotation,
                                                        originalTargetRotation, Time.deltaTime * resetRotationSpeed);
        target.localPosition = new Vector3(0, 0, 0);
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
