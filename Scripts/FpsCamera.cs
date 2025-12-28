using UnityEngine;

public class FpsCamera : MonoBehaviour
{
    public float sensitivity = 300f;
    public Transform playerBody;

    public GameObject cameraHolder;
    public PlayerMovement player;

    float xRotation = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {

        if (player.isDead)
        {   
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            return;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        playerBody.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
    
}
