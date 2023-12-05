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
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;

    private Vector3 velocity;
    bool isGrounded;

    private Rigidbody rb;

    public float speed;

    private bool blockMovement;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (!blockMovement) DoMovement();
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

        controller.Move(move * speed * Time.deltaTime);

        velocity.y -= gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
}