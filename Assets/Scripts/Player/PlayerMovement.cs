using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 5f;
    public float JumpHeight = 2f;
    //falling
    public float fallGravityMultiplier = 2f;
    public float mouseSensitivity = 2f;
    public float pitchRange = 60f;

    private float forwardInputValue;
    private float strafeInputValue;
    private bool jumpInput;

    private float terminalVelocity = 53f;
    private float verticalVelocity;

    private float rotateCameraPitch;

    private Camera firstPersonCam;
    private CharacterController characterController;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        firstPersonCam = GetComponentInChildren<Camera>();
    }

    void Update()
    {
        forwardInputValue = Input.GetAxisRaw("Vertical");
        strafeInputValue = Input.GetAxisRaw("Horizontal");
        jumpInput = Input.GetButtonDown("Jump");
        Movement();
        JumpAndGravity();
        CameraMovement();
    }

    void Movement()
    {
        Vector3 direction = (transform.forward * forwardInputValue
                            + transform.right * strafeInputValue).normalized
                            * movementSpeed * Time.deltaTime;
        direction += Vector3.up * verticalVelocity * Time.deltaTime;

        characterController.Move(direction);
    }

    void JumpAndGravity()
    {
        if (characterController.isGrounded)
        {
            if (verticalVelocity < 0.0f)
            {
                verticalVelocity = -2f;
            }

            if (jumpInput)
            {
                verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Physics.gravity.y);
            }
        }
        else
        {
            if(verticalVelocity < terminalVelocity)
            {
                float gravityMultiplier = 1;
                if(characterController.velocity.y < -1)
                {
                    gravityMultiplier = fallGravityMultiplier;
                }
                verticalVelocity += Physics.gravity.y * gravityMultiplier * Time.deltaTime;
            }
        }
    }

    void CameraMovement()
    {
        //rotate player around
        float rotateYaw = Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.Rotate(0, rotateYaw, 0);

        //rotate cam up and down
        rotateCameraPitch += -Input.GetAxis("Mouse Y") * mouseSensitivity;
        //lock rotation so no flip camera
        rotateCameraPitch = Mathf.Clamp(rotateCameraPitch, -pitchRange, pitchRange);
        firstPersonCam.transform.localRotation = Quaternion.Euler(rotateCameraPitch, 0, 0);

    }
}
