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
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float gravity = -9.81f;
    [SerializeField] Animator handsAnimator;

    [Space]
    [Header("Movement")]
    [SerializeField] float walkingSpeed = 4f;
    [SerializeField] float backwardSpeed = 2f;
    [SerializeField] float runningSpeed = 8f;

    [Space]
    [Header("Sprint")]
    [Tooltip("Max sprint duration")]
    [SerializeField] float maxSprintDuration = 6f;
    [Tooltip("Recovery rate factor -> recovery = time * sprintRoveryRate")]
    [SerializeField] float sprintRecoveryRate = 0.5f;
    [Tooltip("Mulitplier for lateral speed when sprinting")]
    [SerializeField] private float sprintLateralFactor = 0.5f;


    [Space]
    [Header("Jump")]
    [SerializeField] float jumpHeight = 1.5f;
    [SerializeField] float airSpeedFactor = 0.7f;
    [SerializeField] float airSpeedX = 3f;
    [SerializeField] float airSpeedZ = 1f;

    [Space]
    [Header("Slide")]
    [SerializeField] float slideSpeed = 5f;
    [SerializeField] float slideDetectorRadius = 0.3f;

    [Space]
    [Header("Camera")]
    [Tooltip("Look sensitivity")]
    [SerializeField] float lookSpeed = 1.0f;
    [Tooltip("Look limit angle up and down")]
    [SerializeField] float lookYLimit = 45.0f;

    //Status
    private CharacterController controller;
    private bool canMove = true;
    private bool isSliding = false;
    private ControllerColliderHit colliderHit;

    private bool isGrounded = true;
    public bool IsGrounded { get => isGrounded; }

    private bool sprintCmd = false;
    private bool isRunning = false;
    public bool IsRunning { get => isRunning; }

    //Sprint
    private float sprintTimer = 0f;
    public float SprintTimer { get => sprintTimer; }

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

    private HUD playerHud;

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
                if (sprintTimer <= 0)
                    sprintCmd = true;
                break;
            case InputActionPhase.Canceled:
                sprintCmd = false;
                break;
        }
    }

    public void OnToggleSprint(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed) {
            if (sprintTimer <= 0)
                sprintCmd = true;
        }
    }

    public void OnJump(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed) {
            Jump();
        }
    }
    #endregion


    #region MONOBEHAVIOUR METHODS

    void Start() {
        controller = GetComponent<CharacterController>();
        playerHud = GetComponent<HUD>();

        //Lock and hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        startStepOffset = controller.stepOffset;
        SpeedFactor = 1f;
    }

    private float speed = 0;
    void Update() {

        //Update ground status
        isGrounded = Physics.CheckSphere(groundChecker.position, groundCheckRadius, groundLayer, QueryTriggerInteraction.Ignore);
        if (isGrounded) {
            controller.stepOffset = startStepOffset;
        }
        else {
            controller.stepOffset = 0;
        }

        //Process movement
        ApplyGravity();
        UpdateVelocity();
        Sliding();
        Look();
        Sprint();
        handsAnimator.SetBool("Grounded", isGrounded);

        //Move
        if (canMove) {
            controller.Move(xzVelocity * SpeedFactor * Time.deltaTime);
            if (!isSliding && IsGrounded)
                speed = Mathf.Lerp(speed, (xzVelocity.magnitude * SpeedFactor) / currentSpeed, 0.1f);
            handsAnimator.SetFloat("Speed", speed);
        }
        controller.Move(yVelocity * Time.deltaTime);
    }
    #endregion

    #region DEBUG
    //Vector3 starPos;
    //float moveTestTimer = 0f;
    //bool isMoving = false;
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

        if (isGrounded/* && !isSliding*/) { //Testing before remove

            //Compute x and z speed
            Vector3 sprintInputs = inputs;
            if (IsRunning) {
                sprintInputs.x *= sprintLateralFactor;
                sprintInputs.Normalize();
            }
            float zSpeed = sprintInputs.z * currentSpeed;
            float xSpeed = sprintInputs.x * currentSpeed;
            xzVelocity = Vector3.ClampMagnitude((forward * zSpeed) + (right * xSpeed), currentSpeed);
        }
        else if (isSliding) {       //If is not grounded and is on slope
            float zSpeed = inputs.z * currentSpeed;
            float xSpeed = inputs.x * currentSpeed;
            xzVelocity = Vector3.ClampMagnitude((forward * zSpeed) + (right * xSpeed), currentSpeed * 0.5f);
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
        if ((isGrounded && yVelocity.y < 0) || isSliding)
            yVelocity.y = -2f;
        else
            //Apply gravity
            yVelocity.y += gravity * Time.deltaTime;
    }

    private void Jump() {
        if (isGrounded && !isSliding) {
            yVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    private void Sprint() {

        //canStartSprint = isGrounded && sprintTimer <= 0;

        isRunning = sprintCmd && sprintTimer <= maxSprintDuration && inputs.z >= inputThreshold;

        //Stop sprint if timer reached max sprint duration
        if (sprintTimer >= maxSprintDuration)
            sprintCmd = false;

        //Update stamina
        if (isRunning && inputs.z >= inputThreshold) {
            sprintTimer += Time.deltaTime;
        }
        else if (inputs.z < inputThreshold) { //if is not moving forward (and not running)
            sprintTimer = Mathf.Clamp(sprintTimer - (sprintRecoveryRate * Time.deltaTime), 0.0f, maxSprintDuration);
        }
        else {
            sprintCmd = false;
            sprintTimer = Mathf.Clamp(sprintTimer - (sprintRecoveryRate * Time.deltaTime), 0.0f, maxSprintDuration);
        }

        //Update animator
        handsAnimator.SetBool("Run", isRunning);
        playerHud.SetStamina(Mathf.Clamp(sprintTimer / maxSprintDuration, 0, 1));
    }

    private void Look() {
        if (canMove) {
            //Orient camera thanks to mouse position
            yRotation += -lookPos.y * lookSpeed;
            yRotation = Mathf.Clamp(yRotation, -lookYLimit, lookYLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(yRotation, 0, 0);

            //Rotate player 
            transform.rotation *= Quaternion.Euler(0, lookPos.x * lookSpeed, 0);
        }
    }

    private void Sliding() {
        if (colliderHit == null)
            return;

        //Get angle of slope
        float slopeAngle = Vector3.Angle(colliderHit.normal, Vector3.up);
        bool slideAngle = controller.slopeLimit < slopeAngle && slopeAngle <= 90;
        //Detect if player feet touch ground
        bool sphereCheck = Physics.CheckSphere(transform.position, slideDetectorRadius, groundLayer);

        //If on ground and slope + if the Y velocity is not positive
        if (sphereCheck && slideAngle && yVelocity.y <= 0) {

            //Detect slope direction
            var normal = colliderHit.normal;
            normal.y = 0f;
            var dir = Vector3.ProjectOnPlane(normal.normalized, colliderHit.normal).normalized;

            //Make player slide
            isSliding = true;
            controller.Move(dir * slopeAngle / 90f * slideSpeed * Time.deltaTime);
        }
        else
            isSliding = false;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit) {
        if (groundLayer == (groundLayer | (1 << hit.gameObject.layer))) {
            colliderHit = hit;
        }
    }
    #endregion

    #region DEBUG
    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(groundChecker.position, groundCheckRadius);
    }


    #endregion
}
