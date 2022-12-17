using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MovementScript : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 15;
    private float horizontal, vertical;
    public Transform cam;
    private Vector3 direction;
    public float turnSmoothTime = 0.1f;
    public float turnSmoothVelocity;
    public CharacterController controller;
    public float gravity;
    public float maxGravity;
    public float constantGravity;
    public float jumpForce;
    private bool groundedPlayer;
    private Vector3 playerVelocity;
    public Transform groundCheck;
    public LayerMask Ground;
    private float turnSmoothness;
    private float turnSmoothing;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void FixedUpdate()
    {
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
        groundedPlayer = Physics.CheckSphere(groundCheck.position, 0.05f, Ground);

        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        var moveDirection = new Vector3(horizontal, 0, vertical);

        if (moveDirection.magnitude > 0.1f)
        {
            controller.Move(camRotate(direction).normalized * moveSpeed * Time.deltaTime);
        }

        if (Input.GetButtonDown("Jump") && groundedPlayer == true)
        {
            playerVelocity.y = Mathf.Sqrt(jumpForce * -3.0f * gravity);
        }
        playerVelocity.y += gravity * Time.deltaTime;

        controller.Move(playerVelocity * Time.deltaTime);
    }

    public Vector3 camRotate(Vector3 direction)
    {
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);

        Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        return moveDir;
    }
}