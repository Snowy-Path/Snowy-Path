﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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
    [SerializeField] Animator leftHandAnimator;
    [SerializeField] Animator rightHandAnimator;

    [Space]
    [Header("Movement")]
    [SerializeField] float walkingSpeed = 4f;
    [SerializeField] float backwardSpeed = 2f;
    [SerializeField] float runningSpeed = 8f;

    [Space]
    [Header("Sprint")]
    [Tooltip("Max sprint duration")]
    public float maxSprintDuration = 6f;
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
    [SerializeField] UnityEvent onLand;
    [SerializeField] UnityEvent onJump;

    [Space]
    [Header("Slide")]
    [SerializeField] float slideFactor = 5f;
    [SerializeField] float slideDetectorRadius = 0.3f;

    [Space]
    [Header("Turn back")]
    [SerializeField] float turnTime = 0.5f;
    [SerializeField] bool allowMovementInTurn = false;

    [Space]
    [Header("Camera")]
    [Tooltip("Look limit angle up and down")]
    [SerializeField] float lookYLimit = 45.0f;

    //Status
    private CharacterController controller;
    private bool canMove = true;
    private bool canRotate = true;
    private bool isSliding = false;
    private float slideSpeed = 0;
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
    private bool isTurningBack = false;

    //Parameters
    private float startStepOffset;
    private const float inputThreshold = 0.2f;

    private HandController handController;


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
        //if (context.phase == InputActionPhase.Performed) {
        //    if (sprintTimer <= 0)
        //        sprintCmd = true;
        //}
    }

    public void OnJump(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed) {
            Jump();
        }
    }

    public void OnTurnBack(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed) {
            StartCoroutine(TurnBack());
        }
    }
    #endregion


    #region MONOBEHAVIOUR METHODS

    void Start() {
        controller = GetComponent<CharacterController>();
        handController = GetComponentInChildren<HandController>();

        //Lock and hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        startStepOffset = controller.stepOffset;
        SpeedFactor = 1f;
    }

    private float speed = 0;
    void Update() {

        UpdateSensitivity();

        bool wasGrounded = isGrounded;
        //Update ground status
        isGrounded = Physics.CheckSphere(groundChecker.position, groundCheckRadius, groundLayer, QueryTriggerInteraction.Ignore);
        if (isGrounded) {
            controller.stepOffset = startStepOffset;
            if (!wasGrounded)
                onLand.Invoke();
        }
        else {
            controller.stepOffset = 0;
            if (wasGrounded)
                onLand.Invoke();
        }

        //Process movement
        ApplyGravity();
        UpdateVelocity();
        Sliding();
        Look();
        Sprint();
        leftHandAnimator.SetBool("Grounded", isGrounded);
        rightHandAnimator.SetBool("Grounded", isGrounded);

        //Move
        if (canMove) {
            if (!isTurningBack || isTurningBack == allowMovementInTurn)
                controller.Move(xzVelocity * SpeedFactor * Time.deltaTime);

            if (!isSliding && IsGrounded)
                speed = Mathf.Lerp(speed, (xzVelocity.magnitude * SpeedFactor) / currentSpeed, 0.1f);

            leftHandAnimator.SetFloat("Speed", speed);
            rightHandAnimator.SetFloat("Speed", speed);
        }
        else {
            leftHandAnimator.SetFloat("Speed", 0);
            rightHandAnimator.SetFloat("Speed", 0);
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

        if (isGrounded && !isSliding) {

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

    bool groundedLastFrame = false;
    private void ApplyGravity() {
        if (isGrounded && yVelocity.y <= 0) {
            yVelocity.y = gravity;
        }
        else {
            if (groundedLastFrame && yVelocity.y <= 0)
                yVelocity.y = 0;

            yVelocity.y += gravity * Time.deltaTime;
        }
        groundedLastFrame = isGrounded;
    }

    private void Jump() {
        if (isGrounded && !isSliding) {
            yVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    private void Sprint() {

        //canStartSprint = isGrounded && sprintTimer <= 0;

        isRunning = sprintCmd && sprintTimer <= maxSprintDuration && inputs.z >= inputThreshold &&
           (handController.CurrentTool.IsBusy == false || handController.CurrentTool.GetType() == typeof(Gun));

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
        leftHandAnimator.SetBool("Run", isRunning);
        rightHandAnimator.SetBool("Run", isRunning);
    }

    private const float factorLook = 0.01f;
    private float sensitivity = 1f;
    private void Look() {
        if (canMove && canRotate) {
            //Orient camera thanks to mouse position
            yRotation += -lookPos.y * sensitivity;
            yRotation = Mathf.Clamp(yRotation, -lookYLimit, lookYLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(yRotation, 0, 0);

            //Rotate player 
            transform.rotation *= Quaternion.Euler(0, lookPos.x * sensitivity, 0);
        }
    }

    private void UpdateSensitivity() {
        if (OptionHandler.Instance) {
            sensitivity = factorLook * OptionHandler.Instance.Sensitivity;
        }
        else {
            sensitivity = 10;
        }
    }

    private IEnumerator TurnBack() {
        if (isTurningBack) {
            yield break;
        }

        canRotate = false;
        isTurningBack = true;

        float turnSpeed = 180 / turnTime;
        float rot = 0;
        while (rot < 180) {
            transform.Rotate(Vector3.up, turnSpeed * Time.deltaTime);
            rot += turnSpeed * Time.deltaTime;
            yield return null;
        }
        canRotate = true;
        isTurningBack = false;
    }

    private void Sliding() {
        if (colliderHit == null)
            return;

        //Reset slideSpeed
        if (!isSliding)
            slideSpeed = 0;

        //Get angle of slope
        float slopeAngle = Vector3.Angle(colliderHit.normal, Vector3.up);
        bool slideAngle = controller.slopeLimit < slopeAngle && slopeAngle <= 90;
        //Detect if player feet touch ground
        bool sphereCheck = Physics.CheckSphere(transform.position, slideDetectorRadius, groundLayer);

        //If on ground and slope + if the Y velocity is not positive
        if (sphereCheck && slideAngle && yVelocity.y <= 0) {
            isSliding = true;

            //Detect slope direction
            var normal = colliderHit.normal;
            normal.y = 0f;
            var dir = Vector3.ProjectOnPlane(normal.normalized, colliderHit.normal).normalized;

            //Make player slide
            slideSpeed += slopeAngle / 90f * slideFactor;
            controller.Move(dir * slideSpeed * Time.deltaTime);
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
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, slideDetectorRadius);
    }


    #endregion
}
