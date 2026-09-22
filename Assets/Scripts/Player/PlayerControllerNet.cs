using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerControllerNet : NetworkBehaviour
{
    [SerializeField] private PlayerData data;

    private CharacterController controller;
    private Animator animator;
    private Transform cameraTransform;

    private float verticalVelocity;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        // 只有自己的 Player 启用 CharacterController
        controller.enabled = IsOwner;

        // 只有自己的 Player 获取本地 Camera
        if (IsOwner)
        {
            Camera mainCamera = Camera.main;

            if (mainCamera != null)
            {
                cameraTransform = mainCamera.transform;
            }
            else
            {
                Debug.LogError("Main Camera not found!");
            }
        }
    }

    private void Update()
    {
        // 不是自己的 Player，不处理输入
        if (!IsOwner)
        {
            return;
        }

        // Camera 没找到，不移动
        if (cameraTransform == null)
        {
            return;
        }

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
