using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerSkateMovement : MonoBehaviour
{
    [SerializeField]
    GameObject respawn;

    const float MODEL_ROTATION_FACTOR = 180f;

    //temptemptemp
    const float LEFT_STICK_DEADZONE = 0.35f;

    /*
     * CONTROLS
     * Up on stick to accel, down to deccel
     * A to crouch
     */

    [SerializeField]
    float baseTurnSpeed, turnDrag;

    [SerializeField]
    [Range(0, 10)]
    float baseMoveSpeed;

    [SerializeField]
    [Range(0, 1)]
    float rotationSpeed;

    //Input vars
    float zMove;
    float turnLeft, turnRight, rotationY;
    bool jump, jumpReleased;


    bool accelButtonDown;

    Rigidbody rb;

    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        rb.drag = turnDrag;
    }

    void Update()
    {
        //change local drag variable. this function takes care of the rest
        SetDrag();

        //NOTE: values between 0 and 1
        turnLeft = Input.GetAxis("JoyTurnLeft");
        turnRight = Input.GetAxis("JoyTurnRight");

        jump = Input.GetButtonDown("JoyJump");
        jumpReleased = Input.GetButtonUp("JoyJump"); //TODO(low): do jumping

        Move();

        if(Input.GetKeyDown(KeyCode.R))
        {
            ResetPlayer();
        }
    }

    private void FixedUpdate()
    {
        RollerSkateMovement();
    }

    private void RollerSkateMovement()
    {
        if (turnLeft > 0) //TODO: turn input value should affect base turn speed
        {
            gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, (gameObject.transform.eulerAngles.y - (turnLeft * rotationSpeed)), gameObject.transform.eulerAngles.z);
        }

        if (turnRight > 0)
        {
            gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, (gameObject.transform.eulerAngles.y + (turnRight * rotationSpeed)), gameObject.transform.eulerAngles.z);
        }

        float rotationY = gameObject.transform.eulerAngles.y + MODEL_ROTATION_FACTOR;

        Vector3 updatedDirection = new Vector3(Mathf.Cos(rotationY * Mathf.Deg2Rad), 0, -Mathf.Sin(rotationY * Mathf.Deg2Rad));

        //Add force based on player's current rotation and direction
        //constantly applied, we just change the movespeed & turnspeed

        rb.AddForce(updatedDirection * baseTurnSpeed * baseMoveSpeed, ForceMode.Acceleration);
    }

    private void Move()
    {
        zMove = Input.GetAxis("JoyVertical");

        // TODO(low): deadzone 
        if (zMove > LEFT_STICK_DEADZONE) //accelerate
        {
            if (!accelButtonDown && baseMoveSpeed < 10)
            {
                accelButtonDown = true;
                baseMoveSpeed++;
            }
        }
        else if (zMove < -LEFT_STICK_DEADZONE) //deccelerate
        {
            if (!accelButtonDown && baseMoveSpeed > 0)
            {
                accelButtonDown = true;
                baseMoveSpeed--;
            }
        }
        else
        {
            accelButtonDown = false;
        }
    }

    public void SetDrag()
    {
        //allow for designer to update rigidbody drag
        rb.drag = turnDrag;
    }

    public void ResetPlayer()
    {
        gameObject.transform.position = respawn.transform.position;
        gameObject.transform.localRotation = Quaternion.identity;
        baseMoveSpeed = 1;

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}
