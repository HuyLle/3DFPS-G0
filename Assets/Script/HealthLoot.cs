using UnityEngine;

public class HealthLoot : MonoBehaviour
{
    public float rotateSpeed = 90f; // Tốc độ xoay 360 độ
    public float floatAmplitude = 0.5f; // Chiều cao lên xuống
    public float floatSpeed = 1f; // Tốc độ lên xuống
    public float healAmount = 50f; // Số máu hồi

    private Vector3 startPosition;
    private float timeOffset;

    void Start()
    {
        startPosition = transform.position;
        timeOffset = Random.Range(0f, 2f * Mathf.PI); // Ngẫu nhiên thời gian bắt đầu
    }

    void Update()
    {
        // Xoay 360 độ quanh trục Y
        transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);

        // Lên xuống (hiệu ứng float)
        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed + timeOffset) * floatAmplitude;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Giả sử Player có tag "Player"
        {
            PlayerStats playerStats = other.GetComponent<PlayerStats>();
            if (playerStats != null && playerStats.currentHealth < playerStats.maxHealth)
            {
                //playerStats.TakeDamage(-healAmount); // Hồi máu (giá trị âm)
                playerStats.Heal(healAmount);
                Destroy(gameObject); // Hủy health_pickup khi nhặt
            }
        }
    }
}
