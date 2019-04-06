using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkateMovement : MonoBehaviour
{
    //some help w/ slopes https://www.reddit.com/r/Unity3D/comments/2b696a/rotate_player_to_angle_of_slope/

    public enum MoveType { SIM, ARCADE };

    public float debugMoveSpeed;

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
        public float maxVelocity, accelCap; 
        public float accelMultiplier, boostAcceleration, 
                     boostValue;

        public float rotationSpeed;
        public float jumpForce;
    }
    [SerializeField]
    [Tooltip("will control how fast the player slows down when not holding accelerate")]
    float playerDrag = 0.7f;

    [SerializeField] float leftStickXAxisDeadzone = 0.25f;
    
	public MoveType moveType;

    [SerializeField] ArcadeMoveData arcadeData = new ArcadeMoveData();
    [SerializeField] Transform respawn = null;
    [SerializeField] float liftCoeffiecient = 0;

    const float SLOPE_RAY_DIST = 1f;
    const float PLAYER_ALIGN_SPEED = 10f;
  
    //Input vars
    float xMove, accelerationButton;
    bool accelButtonDown, jump, isGrounded, applyDownforce, isAirborne;
    Rigidbody rb;
    float oldVel;
    Transform objTransform;

    //Drifting stuff
    bool isDrifting = false;
    bool endOfDrift; //Flags smooth camera damping on drift release
    Vector3 driftStartForward;
    const float DRIFT_CAM_RESET_WAIT = 0.35f;
    const float FACING_STRAIGHT_DIST = 0.1f;

    Vector3 driftVelocity;
    float driftSlowTimer; //when speed starts to decrease;
    [SerializeField]
    [Tooltip("How long before speed decreases")]
    float driftTime = 2f;
    [SerializeField]
    [Tooltip("How long it takes to stop after slowing down while drifting.")]
    float driftStopTime = 4f;

    FollowCamera playerCam;

    void Awake()
    {
        objTransform = gameObject.GetComponent<Transform>();
        rb = gameObject.GetComponent<Rigidbody>();
        playerCam = GameObject.FindGameObjectWithTag("CameraRig").GetComponent<FollowCamera>();

        respawn.position = objTransform.position;
    }

    void Update()
    {
        ProcessInput();
        MoveAnimation();

        DriftCamRelease();

        if (Input.GetKeyDown(KeyCode.R))
            ResetPlayer();
    }

    void ProcessInput()
    {
        xMove = Input.GetAxis("JoyHorizontal");
        accelerationButton = Input.GetAxis("JoyTurnRight"); //rt
        jump = Input.GetButtonDown("JoyJump");

        if (Input.GetButtonDown("JoyDrift")) startDrifting();
        if (Input.GetButtonUp("JoyDrift")) stopDrifting();
    }

    private void FixedUpdate()
    {   
        if (moveType == MoveType.ARCADE)
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        
        AlignPlayerWithGround();
        RollerSkateMovement();
        JumpAlignRaycast();
        JumpLandRaycast();
        Jump();

        if (accelButtonDown && isGrounded) //research: https://answers.unity.com/questions/1362513/custom-gravity-to-drive-car-on-walls.html
        {
            float lift = liftCoeffiecient * rb.velocity.sqrMagnitude;
            rb.AddForceAtPosition(lift * -objTransform.up, objTransform.position, ForceMode.Force);
        }
        debugMoveSpeed = rb.velocity.magnitude;
    }

    #region Drifting
    private void startDrifting()
    {
        driftStartForward = objTransform.forward;
        
        isDrifting = true;
        driftVelocity = rb.velocity;
        driftSlowTimer = Time.time + driftTime;
    }
    private void stopDrifting()
    {
        endOfDrift = true;
        isDrifting = false;

        if (isGrounded)
        {
            rb.velocity = transform.forward * rb.velocity.magnitude;
        }
    }

    void DriftCamRelease()
    {
        //Apply camera drift effect when not facing forward & drift button released
        if (accelButtonDown && endOfDrift)
        {
            if (Vector3.Distance(driftStartForward.normalized, objTransform.forward.normalized) > FACING_STRAIGHT_DIST)
            {
                playerCam.ApplyDriftDamping();
                StartCoroutine(WaitAndChangeDamping(DRIFT_CAM_RESET_WAIT));
            }

            endOfDrift = false;
        }
    }

    private IEnumerator WaitAndChangeDamping(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        playerCam.ReturnToDefaultDamping();
    }
    #endregion

    void Jump()
    {
        if (jump && isGrounded)
        {
            //jump applied to player local y
            Vector3 jumpVec = arcadeData.jumpForce * objTransform.up;
            rb.AddForceAtPosition(jumpVec, objTransform.position, ForceMode.Impulse);

            isGrounded = false;
            isAirborne = true;
        }
    }

    private void RollerSkateMovement()
    {
        if(moveType == MoveType.ARCADE)
        {        
            if (xMove < -leftStickXAxisDeadzone || xMove > leftStickXAxisDeadzone)
            {
                TurnPhysics();
            }              

            MovePhysics();
        }
    }

    private void TurnPhysics()
    {
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        float turnFactor = xMove * arcadeData.rotationSpeed;
        objTransform.localEulerAngles = new Vector3(objTransform.localEulerAngles.x, objTransform.localEulerAngles.y + turnFactor, objTransform.localEulerAngles.z);

        //Align velocity vector with changed transform forward
        if (isGrounded && !isDrifting)
        {
            Vector3 vel = rb.velocity; //store current speed
            rb.velocity = Vector3.zero;
            rb.velocity = objTransform.forward.normalized * vel.magnitude; //change its direction
        }
    }

    private void MovePhysics()
    {
        //Forward movement
        if (accelButtonDown && isGrounded)
        {
            Vector3 acceleration; 
            Vector3 vel;
            if (isDrifting)
            {
                // if hitting a wall then stop.
                if (rb.velocity.magnitude < 0.1)
                    driftVelocity = Vector3.zero;
                acceleration = Vector3.zero;
                vel = driftVelocity;
                float time = Time.time - driftSlowTimer;
                
                if(time > 0 )
                {
                    driftVelocity -= driftVelocity * (time / driftStopTime);
                }
            }
            else
            {
                //Save player forward unless drift button released
                if(!endOfDrift)
                    driftStartForward = objTransform.forward;

                float moveFactor = accelerationButton * arcadeData.accelMultiplier;
                acceleration = objTransform.forward * moveFactor;
                vel = rb.velocity;
            }
            if(vel.sqrMagnitude > arcadeData.accelCap * arcadeData.accelCap)
            {
                acceleration = Vector3.zero;
            }
            if (vel.sqrMagnitude > arcadeData.maxVelocity * arcadeData.maxVelocity)
                rb.velocity = vel.normalized * arcadeData.maxVelocity;
            else
                rb.velocity = vel + acceleration;
        }
    }

    //This also controls acceleration button
    private void MoveAnimation()
    {
        if (moveType == MoveType.ARCADE)
        {
            if (accelerationButton > 0)
            {
                accelButtonDown = true;
                rb.drag = 0f;
                gameObject.GetComponentInChildren<Animator>().SetBool("isSkating", true);
            }
            else
            {
                accelButtonDown = false;
                rb.drag = playerDrag;
                gameObject.GetComponentInChildren<Animator>().SetBool("isSkating", false);
            }
        }
    }

    #region Raycasts
    void JumpLandRaycast()
    {
        if(isAirborne)
        {
            //only align to gameobjects marked as ground layers
            LayerMask layerToAlignWith = LayerMask.GetMask("Ground");
            Ray ray = new Ray(objTransform.position, -objTransform.up);

            if (Physics.Raycast(ray, out RaycastHit hit, 0.05f, layerToAlignWith))
            {
                Debug.Log("JUMP RAY HIT");
                isAirborne = false;
                rb.velocity = rb.velocity.normalized * oldVel;
            }
        }
    }

    //TODO: give player control over x rotation in midair? Simple anim tricks?
    void JumpAlignRaycast()
    {
        if (isAirborne)
        {
            //only align to gameobjects marked as ground layers
            LayerMask layerToAlignWith = LayerMask.GetMask("Ground");
            Ray ray = new Ray(objTransform.position, -objTransform.up);

            if (Physics.Raycast(ray, out RaycastHit hit, 3.5f, layerToAlignWith))
            {
                //Capture a rotation that makes player move in parallel with ground surface, lerp to that rotation
                Quaternion targetRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;

                //Debug.Log(hit.normal);

                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 12.0f);
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
            if(!isAirborne)
            {
                //Alter rotation damping for smoother adjustment
                isGrounded = true;

                //Capture a rotation that makes player move in parallel with ground surface, lerp to that rotation
                Quaternion targetRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;

                //Debug.Log(hit.normal);

                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * PLAYER_ALIGN_SPEED);

                oldVel = rb.velocity.magnitude;
            }
        }
        else
        {
            isGrounded = false;
            isAirborne = true;
        }
    }
#endregion

    #region Getters and Setters
    public void ResetPlayer() //TODO: reset speed threshold
    {
        objTransform.position = respawn.transform.position;
        objTransform.localRotation = Quaternion.identity;
   
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    public ArcadeMoveData GetArcadeMoveData()
    {
        return arcadeData;
    }
    public void IncreaseSpeed(float boostAcceleration)
    {
        arcadeData.accelMultiplier += (Time.deltaTime * boostAcceleration);
    }

    public void Boost(float boostValue, float maxVelocityIncrease)
    {
        arcadeData.maxVelocity += maxVelocityIncrease;
        rb.velocity += rb.velocity.normalized * boostValue;
        //Camera.main.GetComponent<FollowCamera>().ToggleKnockback();
    }
    public void setMaxVelocity(float maxVelocity)
    {
        arcadeData.maxVelocity = maxVelocity;
    }
    public void setAccelCap(float newAccelCap)
    {
        arcadeData.accelCap = newAccelCap;
    }
    public void setSpeed(float newSpeed)
    {
        rb.velocity = rb.velocity.normalized * newSpeed;
    }
    #endregion
}
