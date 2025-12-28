using UnityEngine;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 6f;
    [SerializeField] private float jumpHeight = 1.2f;
    [SerializeField] private float gravity = -9.81f;

    [Header("Dash")]
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;

    [Header("Bounds")]
    [SerializeField] private Vector2 movementLimits = new Vector2(24f, 24f);

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private Button restartButton;

    public bool IsDead { get; private set; }

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private bool isDashing;
    private float dashTimer;
    private float dashCooldownTimer;
    private Vector3 dashDirection;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Start()
    {
        gameOverText.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (IsDead) return;

        HandleGroundCheck();
        HandleDash();
        HandleMovement();
        ApplyGravity();
        ClampPosition();
    }

    private void HandleMovement()
    {
        if (isDashing) return;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && dashCooldownTimer <= 0f && move.magnitude > 0.1f)
        {
            StartDash(move);
        }
    }

    private void HandleDash()
    {
        if (!isDashing)
        {
            dashCooldownTimer -= Time.deltaTime;
            return;
        }

        controller.Move(dashDirection * dashSpeed * Time.deltaTime);
        dashTimer -= Time.deltaTime;

        if (dashTimer <= 0f)
        {
            isDashing = false;
        }
    }

    private void StartDash(Vector3 direction)
    {
        isDashing = true;
        dashTimer = dashDuration;
        dashCooldownTimer = dashCooldown;
        dashDirection = direction.normalized;
    }

    private void HandleGroundCheck()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0f)
        {
            velocity.y = -2f;
        }
    }

    private void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void ClampPosition()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -movementLimits.x, movementLimits.x);
        pos.z = Mathf.Clamp(pos.z, -movementLimits.y, movementLimits.y);
        transform.position = pos;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Enemy"))
        {
            Die();
        }
    }

    private void Die()
    {
        IsDead = true;
        velocity = Vector3.zero;
        isDashing = false;

        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
    }
}
