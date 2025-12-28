
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class PlayerMovement : MonoBehaviour
{
     public float speed = 6f;
    public float jumpHeight = 1.2f;
    public float gravity = -9.81f;
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;

    private CharacterController controller;
    private Vector3 velocity;  
    private bool isGrounded;
    private bool isDashing;
    private float dashTime;
    private Vector3 dashDirection;

    public float dashCooldown = 1f;

    private float dashCooldownTimer;
    private bool canDash = true;
    public bool isDead;

    public TextMeshProUGUI gameOverText;
    public Button restartButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        restartButton.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(false);
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
            return;
        if (isDashing)
        {
            controller.Move(dashDirection * dashSpeed * Time.deltaTime);
            dashTime -= Time.deltaTime;

            if (dashTime <= 0f)
            
                isDashing = false;

                return; // impede movimento normal durante o dash
            
            
        }

       isGrounded = controller.isGrounded;

       if(isGrounded && velocity.y < 0)
       
       velocity.y = -2f;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

         velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        Vector3 pos = transform.position;

    
        pos.x = Mathf.Clamp(pos.x, -24f, 24f);
        pos.z = Mathf.Clamp(pos.z, -24f, 24f);

        transform.position = pos;

        if (!canDash)
        {
            dashCooldownTimer -= Time.deltaTime;
        
            if (dashCooldownTimer <= 0f)
            {
                canDash = true;
            }
        }


        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing && canDash)
        {
            Dash(move);
        }
    }

    void Dash(Vector3 moveDirection)
    {
        if (moveDirection.magnitude <= 0.1f) return;
        {
            isDashing = true;
            dashTime = dashDuration;
            dashDirection = moveDirection.normalized;
        }

        canDash = false;
        dashCooldownTimer = dashCooldown;
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Enemy"))
        {
            Die();
        }
    }

    public void Die()
    {
        isDead = true;
        restartButton.gameObject.SetActive(true);
        gameOverText.gameObject.SetActive(true);
        isDashing = false;
        velocity = Vector3.zero;
    }
    
}
