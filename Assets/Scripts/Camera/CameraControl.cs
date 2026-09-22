using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] private Transform target;

    [Header("Follow")]
    [SerializeField] private Vector3 offset = new Vector3(0f, 2.5f, -4f);
    [SerializeField] private float followSpeed = 10f;

    [Header("Mouse")]
    [SerializeField] private float mouseSensitivity = 3f;
    [SerializeField] private float minPitch = -30f;
    [SerializeField] private float maxPitch = 60f;

    [SerializeField] private UIManager uiManager;
    private float yaw;
    private float pitch;

    private void Start()
    {
        yaw = 0f;
        pitch = 0f;

        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }

    private void LateUpdate()
    {
        if (target == null)
            return;

        RotateCamera();
        FollowTarget();
    }

    private void RotateCamera()
    {
        if (uiManager.IsUIOpen)
            return;

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        yaw += mouseX * mouseSensitivity;
        pitch -= mouseY * mouseSensitivity;

        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
    }

    private void FollowTarget()
    {
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);

        Vector3 targetPosition = target.position + rotation * offset;

        transform.position = Vector3.Lerp(transform.position,targetPosition,followSpeed * Time.deltaTime);

        transform.rotation = rotation;
    }
}
