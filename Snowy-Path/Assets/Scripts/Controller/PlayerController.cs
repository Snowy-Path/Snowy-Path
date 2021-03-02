using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour {

    [Header("Set up")]
    [SerializeField] Camera playerCamera;
    [SerializeField] Transform groundChecker;
    [SerializeField] float groundCheckRadius = 0.2f;
    [SerializeField] LayerMask groundLayer;             //Temp
    [SerializeField] float gravity = -9.81f;

    [Space]
    [Header("Movement")]
    [SerializeField] float walkingSpeed = 4f;
    [SerializeField] float backwardSpeed = 2f;
    [SerializeField] float runningSpeed = 8f;

    [Space]
    [Header("Sprint")]
    [SerializeField] float maxSprintDuration = 6f;
    [Tooltip("Recovery rate factor -> recovery = time * sprintRoveryRate")]
    [SerializeField] float sprintRecoveryRate = 0.5f;

    [Space]
    [Header("Jump")]
    [SerializeField] float jumpHeight = 1.5f;
    [SerializeField] float airSpeedFactor = 0.7f;
    [SerializeField] float airSpeedX = 3f;
    [SerializeField] float airSpeedZ = 1f;

    [Space]
    [Header("Slide")]
    [SerializeField] float slideSpeed = 20f;
    [SerializeField] float groundMaxDist = 0.5f;

    [Space]
    [Header("Camera")]
    [SerializeField] float lookSpeed = 1.0f;
    [Tooltip("Look limit angle up and down")]
    [SerializeField] float lookYLimit = 45.0f;

    //Status
    private CharacterController controller;
    private bool canMove = true;
    private bool isOnSlope = false;

    private bool isGrounded = true;
    public bool IsGrounded { get => isGrounded; }

    private bool isRunning = false;
    public bool IsRunning { get => isRunning; }

    //Sprint
    private float sprintTimer = 0f;
    public float SprintTimer { get => sprintTimer; }

    private float sprintRecoveryTimer = 0f;
    public float SprintRecoveryTimer { get => sprintRecoveryTimer; }

    private const float sprintTimeToRegen = 0f;

    //Velocity
    private float currentSpeed = 0f;
    public float CurrentSpeed { get => currentSpeed; }

    private Vector3 inputs = Vector3.zero;

    private Vector3 xzVelocity = Vector3.zero;
    public Vector3 XZVelocity { get => xzVelocity; }

    private Vector3 yVelocity = Vector3.zero;
    public Vector3 YVelocity { get => yVelocity; }

    public Vector3 ActualVelocity { get => controller.velocity; }

    //TODO : Replace by Stat ?
    public float SpeedFactor { get; set; }

    //Look
    private Vector2 lookPos = Vector2.zero;
    private float yRotation = 0f;

    //Parameters
    private float startStepOffset;
    private const float inputThreshold = 0.2f;
    private const float slopeSensorDist = 0.1f;

    #region MONOBEHAVIOUR METHODS

    void Start() {
        controller = GetComponent<CharacterController>();

        //Lock and hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        startStepOffset = controller.stepOffset;
        SpeedFactor = 1f;
    }

    void Update() {

        //Update ground status
        UpdateGroundSlopeStatus();
        if (isGrounded) {
            controller.stepOffset = startStepOffset;
        }
        else {
            controller.stepOffset = 0;
        }

        //Process movement
        ApplyGravity();
        UpdateVelocity();
        Look();

        //Update stamina
        if (isRunning) {
            sprintTimer += Time.deltaTime;
            sprintRecoveryTimer = 0.0f;

            //Stop sprint if reached max sprint duration
            if (sprintTimer >= maxSprintDuration)
                isRunning = false;
        }
        else if (sprintTimer > 0) {     //If not running, start recovery
            isRunning = false;

            if (sprintRecoveryTimer >= sprintTimeToRegen) {
                sprintTimer = Mathf.Clamp(sprintTimer - (sprintRecoveryRate * Time.deltaTime), 0.0f, maxSprintDuration);
            }
            else
                sprintRecoveryTimer += Time.deltaTime;
        }


        #region DEBUG
        Keyboard keyboard = Keyboard.current;
        if (keyboard.kKey.wasPressedThisFrame) {
            if (SpeedFactor != 1f)
                SpeedFactor = 1f;
            else
                SpeedFactor = 0.5f;
        }
        //if (keyboard.wKey.wasPressedThisFrame) {
        //    isMoving = true;
        //    starPos = transform.position;
        //}
        //else if(keyboard.wKey.wasReleasedThisFrame) {
        //    float dist = (transform.position - starPos).magnitude;
        //    float speed = dist / moveTestTimer;
        //    float factor = speed / runningSpeed;
        //    Debug.Log($"Dist={dist}  Time={moveTestTimer}  Speed={speed}  CurrentSpeed={currentSpeed}  Factor={factor}");

        //    isMoving = false;
        //    moveTestTimer = 0;
        //}

        //if(isMoving) {
        //    moveTestTimer += Time.deltaTime;
        //}
        #endregion

        //Move
        if (canMove) {
            controller.Move(xzVelocity * SpeedFactor * Time.deltaTime);
        }
        controller.Move(yVelocity * Time.deltaTime);
    }
    #endregion

    #region DEBUG
    //Vector3 starPos;
    //float moveTestTimer = 0f;
    //bool isMoving = false;
    #endregion

    #region INPUTS SYSTEM EVENTS
    public void OnMove(InputAction.CallbackContext context) {
        UpdateInputs(context.ReadValue<Vector2>());
    }

    public void OnLook(InputAction.CallbackContext context) {
        lookPos = context.ReadValue<Vector2>();
    }

    public void OnHoldSprint(InputAction.CallbackContext context) {
        switch (context.phase) {
            case InputActionPhase.Started:
                ToggleSprint(true);
                break;
            case InputActionPhase.Canceled:
                ToggleSprint(false);
                break;
        }
    }

    public void OnToggleSprint(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed) {
            ToggleSprint(!isRunning);
        }
    }

    public void OnJump(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed) {
            Jump();
        }
    }
    #endregion

    #region PRIVATE METHODS
    private void UpdateVelocity() {

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        //Check for speed change if player is on ground
        if (isGrounded) {
            if (isRunning) {
                currentSpeed = runningSpeed;
            }
            else if (inputs.z <= -inputThreshold) {
                currentSpeed = backwardSpeed;
            }
            else {
                currentSpeed = walkingSpeed;
            }
        }


        if (isGrounded) {
            //Compute x and z speed
            float zSpeed = inputs.z * currentSpeed;
            float xSpeed = inputs.x * currentSpeed;
            xzVelocity = Vector3.ClampMagnitude((forward * zSpeed) + (right * xSpeed), currentSpeed);
        }
        else if (isOnSlope) {
            float zSpeed = inputs.z * currentSpeed;
            float xSpeed = inputs.x * currentSpeed;
            xzVelocity = Vector3.ClampMagnitude((forward * zSpeed) + (right * xSpeed), currentSpeed) * 0.5f;
            xzVelocity = ComputeSlopeVelocity();
            Debug.Log("on slope");
        }
        else {
            float xSpeed = inputs.x * airSpeedX;
            float zSpeed = inputs.z * airSpeedZ;
            Vector3 airVelocity = ((forward * zSpeed) + (right * xSpeed));
            xzVelocity = Vector3.ClampMagnitude(Vector3.Lerp(xzVelocity, xzVelocity + airVelocity, 0.1f), currentSpeed * airSpeedFactor);
        }
    }

    private void UpdateInputs(Vector2 contextInputs) {
        //Update Axis
        inputs.x = contextInputs.x;
        inputs.z = contextInputs.y;
    }

    private void ApplyGravity() {
        //Reduce gravity if grounded
        if ((isGrounded && yVelocity.y < 0) || isOnSlope)
            yVelocity.y = -2f;
        else
            //Apply gravity
            yVelocity.y += gravity * Time.deltaTime;
    }

    private void Jump() {
        if (isGrounded && !isOnSlope) {
            yVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    private void ToggleSprint(bool run) {
        if (isGrounded && sprintTimer <= 0 && inputs.z >= inputThreshold) {
            isRunning = run;
        }
        else
            isRunning = false;
    }

    private void Look() {
        if (canMove) {

            yRotation += -lookPos.y * lookSpeed;
            yRotation = Mathf.Clamp(yRotation, -lookYLimit, lookYLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(yRotation, 0, 0);

            transform.rotation *= Quaternion.Euler(0, lookPos.x * lookSpeed, 0);
        }
    }

    private void UpdateGroundSlopeStatus() {

        bool sphereCheck = Physics.CheckSphere(groundChecker.position, groundCheckRadius, groundLayer, QueryTriggerInteraction.Ignore);
        RaycastHit hit;
        Ray ray = new Ray(transform.position + 0.5f * Vector3.up, Vector3.down);
        bool rayCheck = Physics.Raycast(ray, out hit, groundMaxDist, groundLayer);

        isGrounded = (sphereCheck && rayCheck);
        isOnSlope = (sphereCheck && !rayCheck && (colliderHit == null || colliderHit.normal.y <= 7));
        if (colliderHit != null)
            Debug.Log(colliderHit.normal);

        //Debug.Log($"Grounded : {isGrounded}  Slope : {isOnSlope}");
    }

    private void Sliding() {
        if (colliderHit == null)
            return Vector3.zero;

        Vector3 slopeVelocity = Vector3.zero;

        RaycastHit slopeHit;
        Ray raySlope = new Ray(colliderHit.point + colliderHit.normal * slopeSensorDist, Vector3.down);
        if (Physics.Raycast(raySlope, out slopeHit, 5f)) {

            Vector3 slopeDirection = (slopeHit.point - colliderHit.point).normalized;
            float slopeAngle = Vector3.Angle(new Vector3(slopeDirection.x, 0, slopeDirection.z), slopeDirection);

            if (slopeAngle > controller.slopeLimit) {
                slopeVelocity = slopeDirection * slideSpeed;
            }


            if (isGrounded <= groundMinDistance && GroundAngle() > slopeLimit) {
                if (_slidingEnterTime <= 0f || isSliding) {
                    var normal = groundHit.normal;
                    normal.y = 0f;
                    var dir = Vector3.ProjectOnPlane(normal.normalized, groundHit.normal).normalized;

                    if (Physics.Raycast(transform.position + Vector3.up * groundMinDistance, dir, groundMaxDistance, groundLayer)) {
                        isSliding = false;
                    }
                    else {
                        isSliding = true;
                        SlideMovementBehavior();
                    }
                }
                else {
                }
            }
            else {
                isSliding = false;
            }
        }
    }

    private Vector3 ComputeSlopeVelocity() {

        if (colliderHit == null)
            return Vector3.zero;

        Vector3 slopeVelocity = Vector3.zero;

        RaycastHit slopeHit;
        Ray raySlope = new Ray(colliderHit.point + colliderHit.normal * slopeSensorDist, Vector3.down);
        if (Physics.Raycast(raySlope, out slopeHit, 5f)) {

            Vector3 slopeDirection = (slopeHit.point - colliderHit.point).normalized;
            float slopeAngle = Vector3.Angle(new Vector3(slopeDirection.x, 0, slopeDirection.z), slopeDirection);

            if (slopeAngle > controller.slopeLimit) {
                slopeVelocity = slopeDirection * slideSpeed;
            }

            #region DEBUG
            //Debug.Log(slopeAngle);
            //Debug.DrawLine(colliderHit.point, raySlope.origin, Color.red); //from first hit to second ray origin
            //Debug.DrawLine(raySlope.origin, slopeHit.point, Color.blue); //from second ray origin to sencond hit
            //Debug.DrawLine(colliderHit.point, slopeHit.point, Color.green);  // from first hit to second hit
            #endregion
        }

        return slopeVelocity;
    }

    private ControllerColliderHit colliderHit;
    private void OnControllerColliderHit(ControllerColliderHit hit) {
        if (groundLayer.value == 1 << hit.gameObject.layer) {
            colliderHit = hit;
        }
    }
    #endregion

    #region DEBUG
    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(groundChecker.position, groundCheckRadius);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundMaxDist);
    }

    #endregion
}
