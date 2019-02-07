using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerSkateMovement : MonoBehaviour
{
    //some help w/ slopes https://www.reddit.com/r/Unity3D/comments/2b696a/rotate_player_to_angle_of_slope/

    public enum MoveType { SIM, ARCADE };

    [System.Serializable]
    public struct SimMoveData
    {
        public float baseTurnSpeed, turnDrag;

        [Range(0, 10)]
        public float baseMoveSpeed;

        [Range(0, 1)]
        public float rotationSpeed;
    }

    [System.Serializable]
    public struct ArcadeMoveData
    {
        //clamp value - increase this for speed channels?
        public float maxVelocity; 

        public float moveSpeed;

        [HeaderAttribute("RotationData")]
        public float medRotationSpeed;
        public float maxRotationSpeed, rotationSpeed;
    }

    public MoveType moveType;

    [SerializeField]
    ArcadeMoveData arcadeData;
    [SerializeField]
    SimMoveData simData;

    [SerializeField]
    GameObject respawn;

    const float MODEL_ROTATION_FACTOR = 180f;
    //temptemptemp
    const float LEFT_STICK_DEADZONE = 0.35f;
    const float MODEL_ROTATION_MOVE_FACTOR = -1f;

    /*
     * CONTROLS
     * Up on stick to accel, down to deccel
     * A to crouch
     */

    //Input vars
    float zMove;
    float turnLeft, turnRight, rotationY;
    bool jump, jumpReleased;

    bool accelButtonDown;

    Rigidbody rb;

    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        rb.drag = simData.turnDrag;

        if (moveType == MoveType.SIM)
            rb.useGravity = false;
    }

    void Update()
    {
        //change local drag variable. this function takes care of the rest
    //    SetDrag();

        //NOTE: values between 0 and 1
        turnLeft = Input.GetAxis("JoyTurnLeft");
        turnRight = Input.GetAxis("JoyTurnRight");

        jump = Input.GetButtonDown("JoyJump");
        jumpReleased = Input.GetButtonUp("JoyJump"); //TODO(low): do jumping

        Move();
        
        //TODO: speed channel interactions
        if (jump)
        {
        }

        if (Input.GetKeyDown(KeyCode.R))
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
        if (moveType == MoveType.SIM)
        {
            if (turnLeft > 0) //TODO: turn input value should affect base turn speed
            {
                gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, (gameObject.transform.eulerAngles.y - (turnLeft * simData.rotationSpeed)), gameObject.transform.eulerAngles.z);
            }

            if (turnRight > 0)
            {
                gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, (gameObject.transform.eulerAngles.y + (turnRight * simData.rotationSpeed)), gameObject.transform.eulerAngles.z);
            }

            float rotationY = gameObject.transform.eulerAngles.y + MODEL_ROTATION_FACTOR;

            Vector3 updatedDirection = new Vector3(Mathf.Cos(rotationY * Mathf.Deg2Rad), 0, -Mathf.Sin(rotationY * Mathf.Deg2Rad));

            //Add force based on player's current rotation and direction
            //constantly applied, we just change the movespeed & turnspeed

            rb.AddForce(updatedDirection * simData.baseTurnSpeed * simData.baseMoveSpeed, ForceMode.Acceleration);
        }
        else if(moveType == MoveType.ARCADE)
        {
            //Forward and back movement
            if (accelButtonDown)
            {
                float moveFactor = zMove * arcadeData.moveSpeed;

                // Debug.Log(Vector3.ClampMagnitude(movement, arcadeData.maxVelocity));

                //NOTE: issue with the model's transform forward. Using right instead
                Vector3 moveDir = transform.right*moveFactor*MODEL_ROTATION_MOVE_FACTOR; 

                 rb.velocity += Vector3.ClampMagnitude(moveDir, arcadeData.maxVelocity);
            }

            if(turnLeft > 0) //Bumper press for quick U-turn??
            {
               /* if (turnLeft == 1)
                {
                    arcadeData.rotationSpeed = arcadeData.maxRotationSpeed;
                }
                else
                {
                    arcadeData.rotationSpeed = arcadeData.medRotationSpeed;
                }*/

                gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y - (turnLeft*arcadeData.rotationSpeed), gameObject.transform.eulerAngles.z);
            }

            if(turnRight > 0)
            {
                /*if (turnRight == 1)
                {
                    arcadeData.rotationSpeed = arcadeData.maxRotationSpeed;
                }
                else
                {
                    arcadeData.rotationSpeed = arcadeData.medRotationSpeed;
                }*/

                gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y + (turnRight* arcadeData.rotationSpeed), gameObject.transform.eulerAngles.z);
            }
        }
    }

    private void Move()
    {
        zMove = Input.GetAxis("JoyVertical");

        if (moveType == MoveType.SIM)
        {
            // TODO(low): expose input deadzone data
            if (zMove > LEFT_STICK_DEADZONE) //accelerate
            {
                if (!accelButtonDown && simData.baseMoveSpeed < 10)
                {
                    accelButtonDown = true;
                    simData.baseMoveSpeed++;
                }
            }
            else if (zMove < -LEFT_STICK_DEADZONE) //deccelerate, reverse
            {
                if (!accelButtonDown && simData.baseMoveSpeed > 0)
                {
                    accelButtonDown = true;
                    simData.baseMoveSpeed--;
                }
            }
            else
            {
                accelButtonDown = false;
            }
        }
        else if (moveType == MoveType.ARCADE)
        {
         
            if(zMove > LEFT_STICK_DEADZONE)
            {
                accelButtonDown = true;
            }
            else if(zMove < -LEFT_STICK_DEADZONE)
            {
                accelButtonDown = true;
            }
            else
            {
                accelButtonDown = false;
            }
        }
    }

    public void SetDrag()
    {
        //allow for designer to update rigidbody drag - SIM movement
        rb.drag = simData.turnDrag;
    }

    public void ResetPlayer()
    {
        gameObject.transform.position = respawn.transform.position;
        gameObject.transform.localRotation = Quaternion.identity;
        simData.baseMoveSpeed = 1;

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}
