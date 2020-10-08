using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    public float walkSpeed = 6f;
    public float runSpeed = 10f;
    public float gravity = -9.81f * 2;
    public float jumpHeight = 2f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private bool movementLocked;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0) {
            velocity.y = gravity / 3;
        }

        float x = 0;
        float z = 0;
        if (!movementLocked) {
            x = Input.GetAxis("Horizontal");
            z = Input.GetAxis("Vertical");
        }

        Vector3 move = transform.right * x + transform.forward * z;

        if (move.magnitude >= 1) {
            move.Normalize();
        }

        if (Input.GetKey(KeyCode.LeftShift)) {
            move *= runSpeed;
        } else {
            move *= walkSpeed;
        }

        if (!movementLocked && isGrounded && Input.GetButton("Jump")) {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move((move + velocity) * Time.deltaTime);
    }

    public void LockMovement() {
        this.movementLocked = true;
    }

    public void FreeMovement() {
        this.movementLocked = false;
    }
}
