﻿using System.Collections;
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
    [SerializeField] float slideSpeed = 20f;
    [SerializeField] float groundMaxDist = 0.5f;

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
    private float startGroundCheckRadius;
    private float startStepOffset;
    private const float inputThreshold = 0.2f;
    private const float slopeSensorDist = 0.1f;


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
            ToggleSprint(!IsRunning);
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

        //Lock and hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        startStepOffset = controller.stepOffset;
        startGroundCheckRadius = groundCheckRadius;
        SpeedFactor = 1f;
    }

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

        //Update stamina
        if (isRunning) {
            isRunning = isGrounded && sprintTimer <= maxSprintDuration && inputs.z >= inputThreshold;
            if (inputs.z >= inputThreshold) {
                sprintTimer += Time.deltaTime;
                sprintRecoveryTimer = 0.0f;
            }
        }
        else {
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
        else if (isSliding) {
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

    private void ToggleSprint(bool sprint) {
        if (isGrounded && sprintTimer <= 0) {
            isRunning = sprint;
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

    private void Sliding() {
        if (colliderHit == null)
            return;

        RaycastHit hit;
        bool rayCheck = Physics.Raycast(transform.position, Vector3.down, out hit, 5f, groundLayer);
        bool sphereCheck = Physics.CheckSphere(transform.position, 0.3f, groundLayer);

        if (rayCheck ) {
            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);

            if
        }
        else {

        }
        if (&& controller.slopeLimit < slopeAngle && slopeAngle < 80) {
            Debug.Log($"slope angle : {slopeAngle}");
            var normal = colliderHit.normal;
            normal.y = 0f;
            var dir = Vector3.ProjectOnPlane(normal.normalized, colliderHit.normal).normalized;

            if (Physics.Raycast(transform.position + Vector3.up * 0.1f, dir, groundMaxDist, groundLayer)) {
                isSliding = false;
            }
            else {
                isSliding = true;
                controller.Move(dir * slideSpeed * Time.deltaTime);
            }
        }
        else {
            isSliding = false;
        }
    }

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
