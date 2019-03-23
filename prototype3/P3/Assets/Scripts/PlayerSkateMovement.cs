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
        public float moveSpeed, boostAcceleration, 
                     boostValue;

        public float rotationSpeed;
    }

    [SerializeField] float leftStickXAxisDeadzone = 0.25f;
    
	public MoveType moveType;

    [SerializeField] ArcadeMoveData arcadeData = new ArcadeMoveData();
    [SerializeField] Transform respawn = null;
    [SerializeField] float liftCoeffiecient = 0;

    const float SLOPE_RAY_DIST = 1f;
    const float PLAYER_ALIGN_SPEED = 15f;

    //Input vars
    float xMove, accelerationButton;
    bool accelButtonDown, isGrounded, applyDownforce;
    Rigidbody rb;
    Transform objTransform;

    void Awake()
    {
        objTransform = gameObject.GetComponent<Transform>();
        rb = gameObject.GetComponent<Rigidbody>();

        respawn.position = objTransform.position;

        if (moveType == MoveType.SIM)
            rb.useGravity = false;
    }

    void Update()
    {
        ProcessInput();
        MoveAnimation();

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

		AlignPlayerWithGround();
        RollerSkateMovement();
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
        }
        else if(moveType == MoveType.ARCADE)
        {        
            if (!applyDownforce)
            { 
                if (xMove < -leftStickXAxisDeadzone || xMove > leftStickXAxisDeadzone)
                {
                    TurnPhysics();
                }               
            }
            else
            {
                if (xMove < 0) 
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

            MovePhysics();
        }
    }

    private void TurnPhysics()
    {
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        float turnFactor = xMove * arcadeData.rotationSpeed;
        objTransform.localEulerAngles = new Vector3(objTransform.localEulerAngles.x, objTransform.localEulerAngles.y + turnFactor, objTransform.localEulerAngles.z);

        Vector3 vel = rb.velocity; //store current speed
        rb.velocity = Vector3.zero;
        rb.velocity = objTransform.forward.normalized * vel.magnitude; //change its direction
    }

    private void MovePhysics()
    {
        //Forward movement
        if (accelButtonDown && isGrounded)
        {
            float moveFactor = accelerationButton * arcadeData.moveSpeed;

            Vector3 moveDir = objTransform.forward * moveFactor;
            Vector3 vel = rb.velocity;

            if (vel.sqrMagnitude > arcadeData.maxVelocity * arcadeData.maxVelocity)
                rb.velocity = vel.normalized * arcadeData.maxVelocity;
            else
                rb.velocity += moveDir;
        }
    }

    private void MoveAnimation()
    {
        if (moveType == MoveType.SIM)
        {
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
        LayerMask layerToAlignWith = LayerMask.GetMask("Ground");
        Ray ray = new Ray(objTransform.position, -objTransform.up);

        if (Physics.Raycast(ray, out RaycastHit hit, SLOPE_RAY_DIST, layerToAlignWith))
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
    public void ResetPlayer() //TODO: reset speed threshold
    {
        objTransform.position = respawn.transform.position;
        objTransform.localRotation = Quaternion.identity;
   
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
        //Camera.main.GetComponent<FollowCamera>().ToggleKnockback();
    }
    #endregion
}
