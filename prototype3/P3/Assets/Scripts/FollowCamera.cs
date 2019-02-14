using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    //Smooth camera vars
    [SerializeField] Transform target;

    [Header("Smooth Camera Values")]
    [SerializeField] float distance;
    [SerializeField] float height;
    [SerializeField] float damping;
    [SerializeField] float rotationDamping;
    [SerializeField] Vector3 lookAtOffset;
    [SerializeField] bool applyRotationDamping;

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
    
    private void Awake()
    {
        distanceToReach = distance + knockbackDistance;
        returnDistance = returnDistance + distance;
    }

    private void FixedUpdate()
    {
        if (target)
        {
            Vector3 wantedPosition = target.transform.TransformPoint(new Vector3(0, height, -distance));
            Vector3 backDirection = target.transform.TransformDirection(-1 * Vector3.forward);

            Ray ray = new Ray(target.TransformPoint(collisionRaycastOffset), backDirection);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo, raycastLength ))
            {
                if (hitInfo.transform != target) //make sure collision isn't the player
                {
                    Debug.Log("raycast hit" + hitInfo.collider.name);

                    wantedPosition = new Vector3(hitInfo.point.x,
                                                 Mathf.Lerp(hitInfo.point.y + collisionCameraHeight, wantedPosition.y, Time.deltaTime * damping),
                                                 hitInfo.point.z);
                }
            }

            transform.position = Vector3.Lerp(transform.position, wantedPosition, Time.deltaTime * damping);

            Vector3 lookAtPosition = target.TransformPoint(lookAtOffset);

            if (applyRotationDamping)
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
