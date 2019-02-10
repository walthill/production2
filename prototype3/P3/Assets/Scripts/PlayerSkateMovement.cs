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
        public float moveSpeed, 
                     boostAcceleration, boostValue;

        [HeaderAttribute("RotationData")]
        public float medRotationSpeed;
        public float maxRotationSpeed, rotationSpeed;
    }

    public MoveType moveType;

    [SerializeField]
    ArcadeMoveData arcadeData = new ArcadeMoveData();
    [SerializeField]
    SimMoveData simData;

    [SerializeField]
    Transform respawn = null;

    const float MODEL_ROTATION_FACTOR = 180f;
    const float LEFT_STICK_DEADZONE = 0.35f;
    const float MODEL_ROTATION_MOVE_FACTOR = -1f;
    const float SLOPE_RAY_DIST = 10f;
    const float PLAYER_ALIGN_SPEED = 3;

    //Input vars
    float zMove;
    float turnLeft, turnRight, rotationY;
    bool accelButtonDown;
    Rigidbody rb;
    Transform objTransform;

    void Awake()
    {
        objTransform = gameObject.GetComponent<Transform>();
        rb = gameObject.GetComponent<Rigidbody>();

        respawn.position = objTransform.position;
        rb.drag = simData.turnDrag;

        if (moveType == MoveType.SIM)
            rb.useGravity = false;
    }

    void Update()
    {
        ProcessInput();
        Move();
        
        if (Input.GetKeyDown(KeyCode.R))
            ResetPlayer();
    }

    private void FixedUpdate()
    {
        if (moveType == MoveType.ARCADE)
            rb.constraints = RigidbodyConstraints.FreezeRotation;

        AlignPlayerWithGround();
        RollerSkateMovement();
    }


    void ProcessInput()
    {
        //NOTE: values between 0 and 1
        turnLeft = Input.GetAxis("JoyTurnLeft");
        turnRight = Input.GetAxis("JoyTurnRight");
        zMove = Input.GetAxis("JoyVertical");
    }

    private void RollerSkateMovement()
    {
        if (moveType == MoveType.SIM)
        {
            if (turnLeft > 0)
            {
                objTransform.eulerAngles = new Vector3(objTransform.eulerAngles.x, (objTransform.eulerAngles.y - (turnLeft * simData.rotationSpeed)), objTransform.eulerAngles.z);
            }

            if (turnRight > 0)
            {
                objTransform.eulerAngles = new Vector3(objTransform.eulerAngles.x, (objTransform.eulerAngles.y + (turnRight * simData.rotationSpeed)), objTransform.eulerAngles.z);
            }

            float rotationY = objTransform.eulerAngles.y + MODEL_ROTATION_FACTOR;

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

                //NOTE: issue with the model's transform forward. Using right instead
                Vector3 moveDir = objTransform.right*moveFactor*MODEL_ROTATION_MOVE_FACTOR;                
                Vector3 vel = rb.velocity;

                if(vel.sqrMagnitude> arcadeData.maxVelocity*arcadeData.maxVelocity)
                {
                    rb.velocity = vel.normalized * arcadeData.maxVelocity;
                }
                else
                {
                    rb.velocity += moveDir;
                }
            }

            if(turnLeft > 0) //Bumper press for quick U-turn??
            {
                rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
                objTransform.eulerAngles = new Vector3(objTransform.eulerAngles.x, objTransform.eulerAngles.y - (turnLeft*arcadeData.rotationSpeed), objTransform.eulerAngles.z);
            }

            if(turnRight > 0)
            {
                rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
                objTransform.eulerAngles = new Vector3(objTransform.eulerAngles.x, objTransform.eulerAngles.y + (turnRight* arcadeData.rotationSpeed), objTransform.eulerAngles.z);
            }
        }
    }

    private void Move()
    {
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

    void AlignPlayerWithGround()
    {
        //help @ https://bit.ly/2RMVeox

        RaycastHit hit;
        if (Physics.Raycast(objTransform.position, -objTransform.up, out hit, SLOPE_RAY_DIST))
        {
            //Debug.Log("hit the ground @ " + hit.normal);

            //Capture a rotation that makes player move in parallel with ground surface, lerp to that rotation
            Quaternion targetRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime* PLAYER_ALIGN_SPEED);
        }
    }


    #region Getters and Setters
    public void SetDrag()
    {
        //allow for designer to update rigidbody drag - SIM movement
        rb.drag = simData.turnDrag;
    }

    public void ResetPlayer()
    {
        objTransform.position = respawn.transform.position;
        objTransform.localRotation = Quaternion.identity;
        simData.baseMoveSpeed = 1;

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    public void IncreaseSpeed(float boostAcceleration)
    {
        arcadeData.moveSpeed += (Time.deltaTime * boostAcceleration);
    }

    public void Boost(float boostValue, float maxVelocityIncrease)
    {
        arcadeData.moveSpeed += (Time.deltaTime * boostValue);
        arcadeData.maxVelocity += maxVelocityIncrease;
        Camera.main.GetComponent<FollowCamera>().ToggleKnockback();
    }
    #endregion
}
