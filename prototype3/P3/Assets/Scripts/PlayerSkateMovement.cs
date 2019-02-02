using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpeedChannel { QUICK, SPEEDY, FAST, LIGHTNING, BLUR }

public class PlayerSkateMovement : MonoBehaviour
{
    const float MODEL_ROTATION_FACTOR = 180f;
    /*
     * CONTROLS
     * Up on stick to accel, down to deccel
     * A to crouch
     */

    [SerializeField]
    float baseTurnSpeed, turnDrag;

    [SerializeField]
    [Range(1, 10)]
    float baseMoveSpeed;

    [SerializeField]
    [Range(0, 1)]
    float rotationSpeed;

    //Input vars
    float zMove;
    float turnLeft, turnRight, rotationY;
    bool jump, jumpReleased;

    Rigidbody rb;

    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        rb.drag = turnDrag;
    }

    void Update()
    {
        //NOTE: values between 0 and 1
        turnLeft = Input.GetAxis("JoyTurnLeft");
        turnRight = Input.GetAxis("JoyTurnRight"); 

        zMove = Input.GetAxis("JoyVertical");

        // TODO: deadzone 
        if (zMove > 0.35) //accelerate
        {
            baseMoveSpeed = 5f;
        }
        else if (zMove < -0.35) //deccelerate
        {
            if(baseMoveSpeed == 5f)
                baseMoveSpeed = 3f;
        }

        jump = Input.GetButtonDown("JoyJump");
        jumpReleased = Input.GetButtonUp("JoyJump");
    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3();
        movement.z = zMove * baseMoveSpeed;

        if (turnLeft > 0)
        {
            gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, (gameObject.transform.eulerAngles.y - (turnLeft* rotationSpeed)), gameObject.transform.eulerAngles.z);
        }

        if (turnRight > 0)
        {
            gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, (gameObject.transform.eulerAngles.y + (turnRight * rotationSpeed)), gameObject.transform.eulerAngles.z);
        }

        float rotationY = gameObject.transform.eulerAngles.y + MODEL_ROTATION_FACTOR;

        Vector3 updatedDirection = new Vector3(Mathf.Cos(rotationY * Mathf.Deg2Rad), 0, -Mathf.Sin(rotationY * Mathf.Deg2Rad));

        //The add force based on the players current rotation and direction is constantly applied, we just change the movespeed & turnspeed
        rb.AddForce(updatedDirection * baseTurnSpeed * baseMoveSpeed, ForceMode.Acceleration);
    }
}
