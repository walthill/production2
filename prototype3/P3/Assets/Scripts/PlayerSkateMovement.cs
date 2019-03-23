using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerSkateMovement : MonoBehaviour
{
    //some help w/ slopes https://www.reddit.com/r/Unity3D/comments/2b696a/rotate_player_to_angle_of_slope/

    public enum MoveType { SIM, ARCADE };
    public bool keyboardMovement;

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

    [SerializeField] float zAxisDeadzone = 0.25f; //Alter this value for more precise movement control w/ new controls
    public MoveType moveType;

    [SerializeField] ArcadeMoveData arcadeData = new ArcadeMoveData();
    [SerializeField] SimMoveData simData;

    [SerializeField] Transform respawn = null;

    [SerializeField] float liftCoeffiecient = 0;

    const float SLOPE_RAY_DIST = 1f;
    const float PLAYER_ALIGN_SPEED = 1;

    //Input vars
    float xMove;
    float rotationY, accelerationButton;
    bool accelButtonDown, isGrounded, applyDownforce;
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

        if (objTransform.localEulerAngles.z > 20 || objTransform.localEulerAngles.z < -20)
        {
            if (objTransform.position.y > -20)
            {
              //  Debug.Log("APPLYING DOWN FORCE");
               // applyDownforce = true;
            }
        }
        else
        {
            applyDownforce = false;
        }

        if (Input.GetKeyDown(KeyCode.R))
            ResetPlayer();
    }

    private void FixedUpdate()
    {
        if(applyDownforce)
        {
            float lift = liftCoeffiecient * rb.velocity.sqrMagnitude;
            rb.AddForceAtPosition(lift*transform.up, objTransform.position);
        }

        if (moveType == MoveType.ARCADE)
            rb.constraints = RigidbodyConstraints.FreezeRotation;

        RollerSkateMovement();
        AlignPlayerWithGround();
    }

    void ProcessInput()
    {
        xMove = Input.GetAxis("JoyHorizontal");
        accelerationButton = Input.GetAxis("JoyTurnRight");

    }

    private void RollerSkateMovement()
    {
        if (moveType == MoveType.SIM)
        {
           /* if (turnLeft < 0)

            {
                objTransform.eulerAngles = new Vector3(objTransform.eulerAngles.x, (objTransform.eulerAngles.y - (turnLeft * simData.rotationSpeed)), objTransform.eulerAngles.z);
            }

            if (turnRight > 0)
            {
                objTransform.eulerAngles = new Vector3(objTransform.eulerAngles.x, (objTransform.eulerAngles.y + (turnRight * simData.rotationSpeed)), objTransform.eulerAngles.z);
            }
            */
            float rotationY = objTransform.eulerAngles.y;// + MODEL_ROTATION_FACTOR;

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
                float moveFactor = accelerationButton * arcadeData.moveSpeed;

                Vector3 moveDir = objTransform.forward * moveFactor;                
                Vector3 vel = rb.velocity;

                if(vel.sqrMagnitude > arcadeData.maxVelocity*arcadeData.maxVelocity)
                {
                    rb.velocity = vel.normalized * arcadeData.maxVelocity;
                    Debug.Log("Help Me: " + wtf);
                }
                else
                {
                    rb.velocity += moveDir;
                    Debug.Log("Help Me: " + wtf);
                }
            }

            if (!applyDownforce)
            { 
                if (xMove < 0) //Bumper press for quick U-turn??
                {
                    rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
                    objTransform.localEulerAngles = new Vector3(objTransform.localEulerAngles.x, objTransform.localEulerAngles.y + (xMove * arcadeData.rotationSpeed), objTransform.localEulerAngles.z);
                }

                if (xMove > 0)
                {
                    rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
                    objTransform.localEulerAngles = new Vector3(objTransform.localEulerAngles.x, objTransform.localEulerAngles.y + (xMove * arcadeData.rotationSpeed), objTransform.localEulerAngles.z);
                }               
            }
            else
            {
                if (xMove < 0) //Bumper press for quick U-turn??
                {
                    rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
                    objTransform.localEulerAngles = new Vector3(objTransform.localEulerAngles.x, objTransform.localEulerAngles.y, objTransform.localEulerAngles.z + (xMove * arcadeData.rotationSpeed));
                }

                if (xMove > 0)
                {
                    rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
                    objTransform.localEulerAngles = new Vector3(objTransform.localEulerAngles.x, objTransform.localEulerAngles.y, objTransform.localEulerAngles.z + (xMove * arcadeData.rotationSpeed));

                }
            }
        }
    }

    private void Move()
    {
        if (moveType == MoveType.SIM)
        {

            /* expose input deadzone data
            if (zMove > zAxisDeadzone) //accelerate
            {
                if (!accelButtonDown && simData.baseMoveSpeed < 10)
                {
                    accelButtonDown = true;
                    simData.baseMoveSpeed++;
                }
            }
            else if (zMove < -zAxisDeadzone) //deccelerate, reverse
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
            }*/
        }
        else if (moveType == MoveType.ARCADE)
        {
            if (accelerationButton > 0)

            {
                accelButtonDown = true;
                gameObject.GetComponentInChildren<Animator>().SetBool("isSkating", true);
            }
            else
            {
                gameObject.GetComponentInChildren<Animator>().SetBool("isSkating", false);
            }
        }
    }

    void AlignPlayerWithGround()
    {
        //help @ https://bit.ly/2RMVeox

        //only align to gameobjects marked as ground layers
        // LayerMask layerMask = LayerMask.GetMask("Player");
        Ray ray = new Ray(objTransform.position, -objTransform.up);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, SLOPE_RAY_DIST, 1 << 9))
        {
            isGrounded = true;
          
            //Capture a rotation that makes player move in parallel with ground surface, lerp to that rotation
            Quaternion targetRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * PLAYER_ALIGN_SPEED);
        }
        else
        {
            isGrounded = false;
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
