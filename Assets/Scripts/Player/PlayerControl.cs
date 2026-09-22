using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerData data;
    [SerializeField] private Transform cameraTransform;

    private CharacterController controller;
    private Animator animator;

    private float verticalVelocity;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 input = new Vector3(horizontal, 0f, vertical);

        if (input.magnitude > 1f)
        {
            input.Normalize();
        }

        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 moveDirection =
            cameraForward * input.z +
            cameraRight * input.x;

        if (moveDirection.magnitude > 0.1f)
        {
            transform.forward = moveDirection;
        }

        Vector3 movement = moveDirection * data.moveSpeed;

        if (controller.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }

        verticalVelocity += Physics.gravity.y * Time.deltaTime;

        movement.y = verticalVelocity;

        controller.Move(movement * Time.deltaTime);

        animator.SetFloat("speed", input.magnitude);
    }
}
