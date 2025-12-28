using UnityEngine;

public class FpsCamera : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float sensitivity = 300f;
    [SerializeField] private float maxVerticalAngle = 80f;

    [Header("References")]
    [SerializeField] private Transform playerBody;
    [SerializeField] private PlayerMovement player;

    private float xRotation;

    private void Start()
    {
        LockCursor(true);
    }

    private void Update()
    {
        if (player != null && player.IsDead)
        {
            LockCursor(false);
            return;
        }

        RotateCamera();
    }

    private void RotateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -maxVerticalAngle, maxVerticalAngle);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    private void LockCursor(bool locked)
    {
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !locked;
    }
}

