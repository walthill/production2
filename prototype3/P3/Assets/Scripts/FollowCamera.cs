using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{

    /**
     *  What type of camera do we want? 
     * 
     * 
     * 
     * */



    [SerializeField]
    GameObject target;
    [SerializeField]
    float dampingValue;

    Vector3 offset;

    void Awake()
    {
        offset = target.transform.position - gameObject.transform.position; //TODO: camera behind
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float targetAngle, currentAngle, angle;

        currentAngle = gameObject.transform.eulerAngles.y;
        targetAngle = target.transform.eulerAngles.y;
        angle = Mathf.LerpAngle(currentAngle, targetAngle, Time.deltaTime * dampingValue);

        Quaternion rotation = Quaternion.Euler(0, targetAngle, 0);
        gameObject.transform.position = target.transform.position - (rotation * offset);

        transform.LookAt(target.transform);
    }
}
