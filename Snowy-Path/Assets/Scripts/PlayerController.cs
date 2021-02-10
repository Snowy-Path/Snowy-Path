using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {
    [Header("Set up")]
    [SerializeField] Camera playerCamera;
    [SerializeField] Transform groundChecker;
    [SerializeField] float groundCheckRadius = 0.2f;
    [SerializeField] LayerMask groundLayer;             //Temp
    [SerializeField] float gravity = -9.81f;

    [Header("Movement")]
    [SerializeField] float walkingSpeed = 7.5f;
    [SerializeField] float backwardSpeed = 7.5f;
    [SerializeField] float runningSpeed = 10f;
    [SerializeField] float jumpForce = 10f;

    [Header("Camera")]
    [SerializeField] float lookSpeed = 2.0f;
    [SerializeField] float lookXLimit = 45.0f;

    private CharacterController controller;
    private bool canMove = true;
    private Vector3 inputs = Vector3.zero;
    private bool isGrounded = true;

    private Vector3 xyVelocity = Vector3.zero;
    private Vector3 yVelocity = Vector3.zero;
    private Vector2 lookPos = Vector2.zero;
    private float xRotation = 0;

    private bool isRunning;

    #region MONOBEHAVIOUR METHODS

    void Start() {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update() {

        //Update ground status
        isGrounded = Physics.CheckSphere(groundChecker.position, groundCheckRadius, groundLayer, QueryTriggerInteraction.Ignore);

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        float xSpeed = 0f;
        float zSpeed = 0f;

        #region DEBUG
        //UpdateInputs(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));

        //lookPos.x = Input.GetAxis("Mouse X");
        //lookPos.y = Input.GetAxis("Mouse Y");

        //if (Input.GetKey(KeyCode.LeftShift)) {
        //    ToggleRun(true);
        //}
        //if (Input.GetKeyUp(KeyCode.LeftShift)) {
        //    ToggleRun(false);
        //}
        //if (Input.GetButtonDown("Jump"))
        //    Jump();
        #endregion

        //Compute speed
        if (isRunning) {
            zSpeed = runningSpeed;
        }
        else {
            zSpeed = inputs.z * walkingSpeed;
            xSpeed = inputs.x * walkingSpeed;
        }
        xyVelocity = (forward * zSpeed) + (right * xSpeed);

        //Rotate camera 
        Rotate();

        //Reduce gravity if grounded
        if (isGrounded && yVelocity.y < 0)
            yVelocity.y = -2f;

        //Apply gravity
        yVelocity.y += gravity * Time.deltaTime;
    }

    void FixedUpdate() {
        if (canMove) {
            controller.Move(xyVelocity * Time.fixedDeltaTime);
        }

        controller.Move(yVelocity * Time.fixedDeltaTime);
    }
    #endregion

    #region INPUTS SYSTEM EVENTS
    public void OnMove(InputAction.CallbackContext context) {
        UpdateInputs(context.ReadValue<Vector2>());
        //Debug.Log($"Move : {context.ReadValue<Vector2>()}");
    }

    public void OnLook(InputAction.CallbackContext context) {
        lookPos = context.ReadValue<Vector2>();
        //Debug.Log($"Look : {context.ReadValue<Vector2>()}");
    }

    public void OnStartRun(InputAction.CallbackContext context) {
        ToggleRun(true);
        Debug.Log("Run");
    }

    public void OnStopRun(InputAction.CallbackContext context) {
        ToggleRun(false);
        Debug.Log("Stop Run");
    }

    public void OnJump(InputAction.CallbackContext context) {
        Jump();
    }
    #endregion

    #region PRIVATE METHODS
    private void ToggleRun(bool run) {
        isRunning = run;
    }

    private void UpdateInputs(Vector2 contextInputs) {
        //Update Axis
        inputs.x = contextInputs.x;
        inputs.z = contextInputs.y;
        //inputs.Normalize();
    }

    private void Jump() {
        if (isGrounded) {
            yVelocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }
    }

    private void Rotate() {
        if (canMove) {
            xRotation += -lookPos.y * lookSpeed;
            xRotation = Mathf.Clamp(xRotation, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
            transform.rotation *= Quaternion.Euler(0, lookPos.x * lookSpeed, 0);
        }
    }
    #endregion


    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(groundChecker.position, groundCheckRadius);
    }
}
