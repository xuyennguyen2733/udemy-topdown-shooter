using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    private Player player;

    private PlayerControls controls;

    private CharacterController characterController;

    private Animator animator;

    [Header("Movement Info")]
    private float speed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float turnSpeed;
    public Vector3 movementDirection;
    private float verticalVelocity;
    private bool isRunning;
    private Vector2 moveInput;

    private void Start()
    {
        player = GetComponent<Player>();
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        speed = walkSpeed;

        AssignInputEvents();
    }

    private void Update()
    {
        ApplyRotation();
        ApplyMovement();
        AnimatorControllers();
    }

    private void AnimatorControllers()
    {
        float xVelocity = Vector3.Dot(movementDirection.normalized, transform.right);
        float zVelocity = Vector3.Dot(movementDirection.normalized, transform.forward);

        animator.SetFloat("xVelocity", xVelocity, .1f, Time.deltaTime);
        animator.SetFloat("zVelocity", zVelocity, .1f, Time.deltaTime);

        bool playRunAnimation = isRunning && movementDirection.magnitude > 0;
        animator.SetBool("isRunning", playRunAnimation);
    }

    private void ApplyRotation()
    {
        Vector3 lookingDirection = player.aimController.GetMouseHitInfo().point - transform.position;
        lookingDirection.y = 0f;
        lookingDirection.Normalize();

        Quaternion desiredRotation = Quaternion.LookRotation(lookingDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, turnSpeed * Time.deltaTime);
    }

    private void ApplyMovement()
    {

        // Transform movement from world space to local space:
        // For reference purposes only. This type of movement isn't
        // intuitive.
        
            //Vector3 forward = transform.forward;
            //Vector3 right = transform.right;
            //forward.y = 0f;
            //right.y = 0f;
            //forward.Normalize();   
            //right.Normalize();
            //movementDirection = moveInput.y * forward + moveInput.x * right;

        movementDirection = new Vector3(moveInput.x, 0, moveInput.y);


        ApplyGravity();

        if (movementDirection.magnitude > 0)
        {
            characterController.Move(movementDirection * Time.deltaTime * speed);
        }
    }

    private void ApplyGravity()
    {
        if (characterController.isGrounded == false)
        {
            verticalVelocity = verticalVelocity - 9.81f * Time.deltaTime;
            movementDirection.y = verticalVelocity;
        }
        else
        {
            verticalVelocity = -.5f;
        }
    }

    private void AssignInputEvents()
    {
        controls = player.controls;

        controls.Character.Movement.performed += context => moveInput = context.ReadValue<Vector2>();
        controls.Character.Movement.canceled += context => moveInput = Vector2.zero;

        

        controls.Character.Run.performed += context =>
        {
            isRunning = true;
            speed = runSpeed;
        };
        controls.Character.Run.canceled += context =>
        {
            isRunning = false;
            speed = walkSpeed;
        };
    }

}