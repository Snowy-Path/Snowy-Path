using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PlayerController2 : MonoBehaviour      //PROTOTYPE VERSION (BASIC)
{
    [Header("Scene set up")]
    [SerializeField] Camera playerCamera;
    [SerializeField] Transform groundChecker;
    [SerializeField] float groundCheckRadius = 0.2f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float gravity = -9.81f;

    [Header("Stats")]
    [SerializeField] float walkingSpeed = 7.5f;
    [SerializeField] float runningSpeed = 10f;
    [SerializeField] float jumpForce = 10f;

    [Header("Camera")]
    [SerializeField] float lookSpeed = 2.0f;
    [SerializeField] float lookXLimit = 45.0f;

    [SerializeField] bool canMove = true;

    //private
    private CharacterController controller;
    private Vector3 inputs = Vector3.zero;
    private bool isGrounded = true;

    private Vector3 xyMove = Vector3.zero;
    private float rotationX = 0;
    private Vector3 velocity = Vector3.zero;

    float curve;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        //Update status
        isGrounded = Physics.CheckSphere(groundChecker.position, groundCheckRadius, groundLayer, QueryTriggerInteraction.Ignore);

        //Update Axis
        inputs.x = Input.GetAxis("Horizontal");
        inputs.z = Input.GetAxis("Vertical");
        inputs.Normalize();

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        float xSpeed = 0f;
        float zSpeed = 0f;

        if (Input.GetKey(KeyCode.LeftShift)) {
            zSpeed = runningSpeed;
        }
        else {
            zSpeed = inputs.z * walkingSpeed;
            xSpeed = inputs.x;
        }

        xyMove = (forward * zSpeed) + (right * xSpeed);

        //Reduce gravity if grounded
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        Jump();

        //Apply gravity
        velocity.y += gravity * Time.deltaTime;

        //Camera 
        Rotate();
    }

    void FixedUpdate()
    {
        if (canMove)
            controller.Move(xyMove * Time.fixedDeltaTime);
        controller.Move(velocity * Time.fixedDeltaTime);
    }

    private void Jump() {
        if (Input.GetButtonDown("Jump") && isGrounded) {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }
    }

    private void Rotate()
    {
        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(groundChecker.position, groundCheckRadius);
    }

}
