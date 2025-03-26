using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody; // Tham chiếu đến Player
    private float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Khóa chuột
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Xoay camera lên xuống
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Giới hạn góc nhìn
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Xoay toàn bộ nhân vật trái/phải
        playerBody.Rotate(Vector3.up * mouseX);
    }
}