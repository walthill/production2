using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] Transform target = null;
 
    //Smooth camera vars
    [Header("Smooth Camera Values")]
    [SerializeField] float distance = 0;
    [SerializeField] float height = 0;
    [SerializeField] float damping = 100;
    [SerializeField] float driftDamping = 10;
    [SerializeField] float rotationDamping = 0;
    [SerializeField] Vector3 lookAtOffset = new Vector3();
    [SerializeField] bool applyRotationDamp=true;

    //Camera collision vars
    [Header("Camera Collision Values")]
    [SerializeField] float raycastLength = 0;
    [SerializeField] float collisionCameraHeight = 0;
    [SerializeField] Vector3 collisionRaycastOffset = new Vector3();

    //Free camera vars
    [Header("Camera Rotation")]
    [SerializeField] float inputDeadZone = 0;
    [SerializeField] float resetRotationSpeed=0; //Make this a large value
    [SerializeField] float camRotationSpeed = 0;
    [SerializeField] float camMinXAngle = 0, camMaxXAngle = 0;
    [SerializeField] float camMinYAngle = 0, camMaxYAngle = 0; //Clamp values for cam rotation

    float distanceToReach, originalDamping;
    bool hasKnockback = false;

    float camYRotation,  camXRotation; //camera rotate input
    bool resetDamping, applyDamping;

    Quaternion originalTargetRotation;

    float xRot, yRot;
    
    private void Awake()
    {
        originalTargetRotation = target.localRotation; //keep track of Quaternion at object rotation's 0,0,0
        originalDamping = damping;
    }

    private void Update()
    {
        camYRotation = Input.GetAxis("JoyHorizontalRS");
        camXRotation = Input.GetAxis("JoyVerticalRS");       
    }

    private void FixedUpdate()
    {
        //Lerp to default cam position
        if (damping < originalDamping - 1 && resetDamping)
        {
            damping = Mathf.Lerp(damping, originalDamping, Time.deltaTime);
        }
        else
            resetDamping = false;

        //Lerp to drifting cam position by altering drift damp
        if (damping > driftDamping + 1 && applyDamping)
        {
            damping = Mathf.Lerp(damping, driftDamping, Time.deltaTime * 2.5f);
        }
        else
            applyDamping = false;

        CameraMove();
    }

    void CameraMove()
    {
        if (target)
        {

            Vector3 wantedPosition = target.transform.TransformPoint(new Vector3(0, height, -distance));
            Vector3 backDirection = target.transform.TransformDirection(-1 * Vector3.forward);

            Ray ray = new Ray(target.TransformPoint(collisionRaycastOffset), backDirection);
            Debug.DrawRay(ray.origin, ray.direction, Color.green);

            //Camera collisions - TODO revisit this collision code 
            if (Physics.Raycast(ray, out RaycastHit hitInfo, raycastLength))
            {
                if (hitInfo.transform != target.parent.transform) //make sure collision isn't the player
                {
                    Debug.Log("raycast hit" + hitInfo.collider.name);

                    wantedPosition = new Vector3(hitInfo.point.x,
                                                 Mathf.Lerp(hitInfo.point.y + collisionCameraHeight, wantedPosition.y, Time.deltaTime * damping),
                                                 hitInfo.point.z);
                }
            }

            transform.position = Vector3.Slerp(transform.position, wantedPosition, Time.deltaTime * damping);

            Vector3 lookAtPosition = target.TransformPoint(lookAtOffset);

            if (applyRotationDamp)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookAtPosition - transform.position, target.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationDamping);
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
            }
            else
            {
                transform.rotation = Quaternion.LookRotation(lookAtPosition - transform.position, target.up);
            }

            FreeCameraMovement();
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
    }

    void ResetAlignment()
    {
        xRot = 0;
        yRot = 0;
        target.localRotation = Quaternion.RotateTowards(target.localRotation,
                                                        originalTargetRotation, Time.deltaTime * resetRotationSpeed);
        target.localPosition = new Vector3(0, 0, 0);
    }

    public void ToggleKnockback()
    {
        hasKnockback = true;
    }

    public void ToggleSpeedUI()
    {
        //TEMP - need to separate UI code out into own class (see todo in SurfaceSpeedBoost)
        //target.gameObject.GetComponent<SpeedSurfaceBoost>().speedIndicator.gameObject.SetActive(false);
        //target.gameObject.GetComponent<SpeedSurfaceBoost>().speedText.gameObject.SetActive(false);
    }

    public void SetRotationDamping(float val) 
    {
        rotationDamping = val;
    }

    public void SetDriftDamping(float val)
    {
        driftDamping = val;
    }
    public void ApplyDriftDamping()
    {
       resetDamping = false;
       applyDamping = true;
    }

    public void ReturnToDefaultDamping() 
    {
        resetDamping = true;
        applyDamping = false;
    }
}
