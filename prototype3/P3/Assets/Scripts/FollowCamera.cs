using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
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

    // Update is called once per frame
    void LateUpdate()
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
