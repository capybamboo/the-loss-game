using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    [SerializeField] private MouseLook ml;

    [Space]
    [SerializeField] private float gravity = 9.81f;

    [Space]
    [SerializeField] private float jumpHeight = 3f;
    [SerializeField] private float currentJumpHeight;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;

    private Vector3 velocity;
    [SerializeField] private bool isGrounded;

    private Rigidbody rb;

    public float speed;

    [SerializeField] private bool blockMovement;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        controller = GetComponent<CharacterController>();

        ResetJumpHeight();
    }

    void Update()
    {
        if (!blockMovement) DoMovement();

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded) velocity.y = Mathf.Sqrt(currentJumpHeight * 2f * gravity);
    }

    public void ChangeJumpHeight(float val)
    {
        currentJumpHeight = val;
    }

    public void ResetJumpHeight()
    {
        currentJumpHeight = jumpHeight;
    }

    public void LockMovement()
    {
        blockMovement = true;
        ml.blockCam = true;
    }

    public void UnlockMovement()
    {
        blockMovement = false;
        ml.blockCam = false;
    }
    public bool GetMovementStatus()
    {
	return blockMovement;
    }
    private void DoMovement()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0) velocity.y = -2f;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        move = move.magnitude > 1 ? move.normalized : move;

        controller.Move(move * speed * Time.deltaTime);

        velocity.y -= gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
}