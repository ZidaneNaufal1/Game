using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class SimpleThirdPersonController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 4f;
    public float runSpeed = 7f;
    public float gravity = -25f;
    public float groundStickForce = -5f;

    [Header("Camera")]
    public float mouseSensitivity = 2f;
    public Transform cameraPivot;
    public float minPitch = -10f;
    public float maxPitch = 45f;
    public float cameraReturnSpeed = 12f;

    [Header("Model Visual")]
    public Transform playerModel;
    public float modelYawOffset = 0f;
    public float strafeTurnAngle = 15f;
    public float visualTurnSpeed = 8f;

    [Header("Animation Optional")]
    public Animator animator;

    private CharacterController controller;
    private float verticalVelocity;
    private float cameraYaw;
    private float cameraPitch = 15f;

    private bool wasPressingForward;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (cameraPivot != null)
        {
            cameraPivot.localPosition = new Vector3(0f, 1.5f, 0f);
            cameraPivot.localRotation = Quaternion.Euler(cameraPitch, cameraYaw, 0f);
        }

        if (animator == null)
        {
            if (playerModel != null)
            {
                animator = playerModel.GetComponentInChildren<Animator>();
            }
            else
            {
                animator = GetComponentInChildren<Animator>();
            }
        }

        if (animator != null)
        {
            animator.applyRootMotion = false;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        RotateCamera();
        MovePlayer();
        HandleCursor();
    }

    void RotateCamera()
    {
        if (cameraPivot == null) return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        float vertical = Input.GetAxisRaw("Vertical");
        bool isPressingForward = vertical > 0.1f;

        if (isPressingForward)
        {
            transform.Rotate(Vector3.up * mouseX);
            cameraYaw = Mathf.Lerp(cameraYaw, 0f, cameraReturnSpeed * Time.deltaTime);
        }
        else
        {
            cameraYaw += mouseX;
        }

        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(cameraPitch, minPitch, maxPitch);

        cameraPivot.localRotation = Quaternion.Euler(cameraPitch, cameraYaw, 0f);
    }

    void MovePlayer()
    {
        if (cameraPivot == null) return;

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        bool isPressingForward = vertical > 0.1f;
        bool justPressedForward = isPressingForward && !wasPressingForward;

        bool isMoving = Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f;
        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        float currentSpeed = isRunning ? runSpeed : moveSpeed;

        if (justPressedForward)
        {
            SnapPlayerToCameraDirection();
        }

        Vector3 moveDirection = Vector3.zero;

        if (isMoving)
        {
            Vector3 forward = transform.forward;
            Vector3 right = transform.right;

            moveDirection = forward * vertical + right * horizontal;
            moveDirection.Normalize();

            RotateVisual(horizontal, vertical);
        }
        else
        {
            RotateVisual(0f, 0f);
        }

        ApplyGravity();

        Vector3 finalMove = moveDirection * currentSpeed;
        finalMove.y = verticalVelocity;

        controller.Move(finalMove * Time.deltaTime);

        UpdateAnimation(isMoving, isRunning);

        wasPressingForward = isPressingForward;
    }

    void SnapPlayerToCameraDirection()
    {
        Vector3 cameraForward = cameraPivot.forward;
        cameraForward.y = 0f;

        if (cameraForward.sqrMagnitude < 0.01f) return;

        cameraForward.Normalize();

        Quaternion targetRotation = Quaternion.LookRotation(cameraForward);

        transform.rotation = targetRotation;

        cameraYaw = 0f;
        cameraPivot.localRotation = Quaternion.Euler(cameraPitch, cameraYaw, 0f);
    }

    void RotateVisual(float horizontal, float vertical)
    {
        if (playerModel == null) return;

        float visualTurn = 0f;

        if (vertical >= 0f)
        {
            visualTurn = horizontal * strafeTurnAngle;
        }

        Quaternion targetLocalRotation = Quaternion.Euler(
            0f,
            modelYawOffset + visualTurn,
            0f
        );

        playerModel.localRotation = Quaternion.Slerp(
            playerModel.localRotation,
            targetLocalRotation,
            visualTurnSpeed * Time.deltaTime
        );
    }

    void ApplyGravity()
    {
        if (controller.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = groundStickForce;
        }

        verticalVelocity += gravity * Time.deltaTime;
    }

    void UpdateAnimation(bool isMoving, bool isRunning)
    {
        if (animator == null) return;

        float targetSpeed = 0f;

        if (isMoving)
        {
            targetSpeed = isRunning ? 1f : 0.5f;
        }

        animator.SetFloat("Speed", targetSpeed, 0.1f, Time.deltaTime);
    }

    void HandleCursor()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}