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
        public float maxVelocity, accelCap, localMaxVelocity, targetVelocity; 
        public float accelMultiplier, boostAcceleration, 
                     boostValue;

        public float rotationSpeed;
        public float jumpForce;
    }
    [SerializeField]
    float diffToSpeedLoss = 20f;

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
    SpeedThresholdBoi speedThresholdBoi;
    Vector3 driftVelocity;
    float driftSlowTimer; //when speed starts to decrease;
    [SerializeField]
    [Tooltip("How long before speed decreases")]
    float driftTime = 2f;
    [SerializeField]
    [Tooltip("How long it takes to stop after slowing down while drifting.")]
    float driftStopTime = 4f;
    [SerializeField]
    [Tooltip("How time is slowed down by during drifting at max speed")]
    float maxDriftTimeScale = 0.5f;
    [SerializeField]
    [Tooltip("should time slow less at lower speeds?")]
    bool changeTimeBySpeed;
    float normalTimeScale;
    FollowCamera playerCam;
    LayerMask layerToAlignWith;    //only align to gameobjects marked as ground layers

    void Awake()
    {
        objTransform = gameObject.GetComponent<Transform>();
        rb = gameObject.GetComponent<Rigidbody>();
        layerToAlignWith = LayerMask.GetMask("Ground");
        playerCam = GameObject.FindGameObjectWithTag("CameraRig").GetComponent<FollowCamera>();
        speedThresholdBoi = gameObject.GetComponent<SpeedThresholdBoi>();
        respawn.position = objTransform.position;
        arcadeData.localMaxVelocity = 25f; //min speed threshold
        normalTimeScale = Time.timeScale;
    }

    void Update()
    {
        ProcessInput();
        MoveAnimation();

        DriftCamRelease();

        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();


        if (Input.GetKeyDown(KeyCode.R))
            ResetPlayer(respawn);
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
        calcTargetVelocity(); //after debug movespeed to use most current move speed.
        sendSpeedToSoundBoi();
    }
    // this sends movespeed data to the sound boi optimize this if you want
    //called from update sorry :(


    void sendSpeedToSoundBoi()
    {
        if (isGrounded)
        {
            SoundBoi.instance.linkWheelSoundToSpeed(debugMoveSpeed);
        }
        
        if (!isGrounded)
        {
            SoundBoi.instance.linkWheelSoundToSpeed(0);
        }
        
        
    }

    #region Drifting
    private void startDrifting()
    {
        driftStartForward = objTransform.forward;
        
        isDrifting = true;
        driftVelocity = rb.velocity.normalized*debugMoveSpeed;
        driftSlowTimer = Time.time + driftTime;
        float modDriftScale = changeTimeBySpeed? 
            1.0f - maxDriftTimeScale * (float)speedThresholdBoi.getCurrentSpeedChannel() / ((float)SpeedChannel.NUM_SPEEDS - 1.0f) 
            : maxDriftTimeScale;
        Time.timeScale = Time.timeScale * modDriftScale;
    }
    private void stopDrifting()
    {
        endOfDrift = true;
        isDrifting = false;
        Time.timeScale = normalTimeScale;
        if (isGrounded)
        {
            //rb.velocity = transform.forward * rb.velocity.magnitude;
            setSpeed(driftVelocity.magnitude);

            rb.velocity = transform.forward * driftVelocity.magnitude;
            //TODO: clusterfuck
            debugMoveSpeed = rb.velocity.magnitude;
            arcadeData.targetVelocity = debugMoveSpeed;
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
            SoundBoi.instance.playJumpSound();
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
        transform.Rotate(new Vector3(0f, turnFactor, 0f));
    }

    private void calcTargetVelocity()
    {
        arcadeData.targetVelocity = rb.velocity.magnitude;
        if(debugMoveSpeed < arcadeData.localMaxVelocity - diffToSpeedLoss)
        { //decrease
            //arcadeData.localMaxVelocity-= .1f
            arcadeData.localMaxVelocity = Mathf.Lerp(arcadeData.localMaxVelocity, debugMoveSpeed, 0.1f);
            arcadeData.localMaxVelocity = Mathf.Max(arcadeData.localMaxVelocity, 25f);
        }
        else if (debugMoveSpeed > arcadeData.localMaxVelocity + diffToSpeedLoss)
        { //increase
            //arcadeData.localMaxVelocity += .1f
            arcadeData.localMaxVelocity = debugMoveSpeed;
        }
        if (accelButtonDown)
            arcadeData.targetVelocity = Mathf.Lerp(debugMoveSpeed, arcadeData.localMaxVelocity, Time.deltaTime);
        arcadeData.targetVelocity = Mathf.Min(arcadeData.targetVelocity, arcadeData.localMaxVelocity);
        float accel = accelerationButton * arcadeData.accelMultiplier;

    }
    private void MovePhysics()
    {
        //Forward movement
        if (accelButtonDown && isGrounded)
        {
            Vector3 vel;
            if (isDrifting)
            {
                // if hitting a wall then stop.
                if (rb.velocity.magnitude < 0.1)
                    driftVelocity = Vector3.zero;
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

                //vel = rb.velocity.normalized * arcadeData.targetVelocity;
                vel = transform.forward.normalized * arcadeData.targetVelocity;
            }
            if (vel.sqrMagnitude > arcadeData.maxVelocity * arcadeData.maxVelocity)
                rb.velocity = vel.normalized * arcadeData.maxVelocity;
            else
                rb.velocity = vel;
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
                gameObject.GetComponentInChildren<Animator>().SetBool("isSkating", true);
                //rb.drag = 0f;
            }
            else
            {
                accelButtonDown = false;
                gameObject.GetComponentInChildren<Animator>().SetBool("isSkating", false);
                //rb.drag = 0.7f;
            }
        }
    }

    #region Raycasts
    void JumpLandRaycast()
    {
        if(isAirborne)
        {
            Ray ray = new Ray(objTransform.position, -Vector3.up);

            if (Physics.Raycast(ray, out RaycastHit hit, 0.05f, layerToAlignWith))
            {
                //Debug.Log("JUMP RAY HIT");
                isAirborne = false;
                rb.velocity = rb.velocity.normalized * oldVel;
                SoundBoi.instance.playLandSound();
            }
        }
    }

    //TODO: give player control over x rotation in midair? Simple anim tricks?
    void JumpAlignRaycast()
    {
        if (isAirborne)
        {
            Ray ray = new Ray(objTransform.position, -Vector3.up);

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

        Ray ray = new Ray(objTransform.position, -objTransform.up);

        if (Physics.Raycast(ray, out RaycastHit hit, SLOPE_RAY_DIST, layerToAlignWith))
        {
            if (!isAirborne)
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
    public void ResetPlayer(Transform t) //TODO: reset speed threshold
    {
        objTransform.position = t.position;
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
        rb.velocity = rb.velocity.normalized * (debugMoveSpeed + boostValue);
        arcadeData.targetVelocity = rb.velocity.magnitude;
        arcadeData.localMaxVelocity = rb.velocity.magnitude;
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
        //TODO: clusterfuck
        debugMoveSpeed = rb.velocity.magnitude;
        arcadeData.targetVelocity = debugMoveSpeed;
        arcadeData.localMaxVelocity = newSpeed;
    }
    #endregion
}
