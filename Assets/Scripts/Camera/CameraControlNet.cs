using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CameraControlNet : MonoBehaviour
{
    private Transform target;

    [Header("Follow")]
    [SerializeField] private Vector3 offset = new Vector3(0f, 2.5f, -4f);
    [SerializeField] private float followSpeed = 10f;

    [Header("Mouse")]
    [SerializeField] private float mouseSensitivity = 3f;
    [SerializeField] private float minPitch = -30f;
    [SerializeField] private float maxPitch = 60f;

    private float yaw;
    private float pitch;

    private void Start()
    {
        yaw = 0;
        pitch = 0;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void LateUpdate()
    {
        // 还没有找到自己的 Player
        if (target == null)
        {
            FindLocalPlayer();
            return;
        }

        RotateCamera();
        FollowTarget();
    }

    private void FindLocalPlayer()
    {
        if (NetworkManager.Singleton == null)
            return;

        if (!NetworkManager.Singleton.IsClient)
            return;

        NetworkObject playerObject =
            NetworkManager.Singleton.LocalClient?.PlayerObject;

        if (playerObject != null)
        {
            target = playerObject.transform;
        }
    }

    private void RotateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        yaw += mouseX * mouseSensitivity;
        pitch -= mouseY * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
    }

    private void FollowTarget()
    {
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);

        Vector3 targetPosition =
            target.position + rotation * offset;

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            followSpeed * Time.deltaTime
        );

        transform.rotation = rotation;
    }
}
