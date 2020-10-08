using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLookController : MonoBehaviour
{
    public float mouseSensitivity = 500f;

    private Transform playerBody;
    private float xRotation = 0f;
    private bool lockCameraMovement;

    // Start is called before the first frame update
    void Start()
    {
        playerBody = transform.parent;
        LockCursorFreeCamera();
    }

    // Update is called once per frame
    void Update()
    {
        if (lockCameraMovement) {
            return;
        }

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    public void FreeCursorLockCamera() {
        lockCameraMovement = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void LockCursorFreeCamera() {
        lockCameraMovement = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
