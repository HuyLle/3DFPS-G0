using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 5f;           // Tốc độ đi bộ
    public float sprintSpeed = 10f;     // Tốc độ chạy
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;
    private Vector3 velocity;

    private PlayerStats playerStats;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        bool isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Lấy input di chuyển
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 move = transform.right * moveX + transform.forward * moveZ;


        // Thêm sprint khi giữ phím Shift
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : speed;
        controller.Move(move * currentSpeed * Time.deltaTime);

        // Nhảy
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Áp dụng trọng lực
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
    
}