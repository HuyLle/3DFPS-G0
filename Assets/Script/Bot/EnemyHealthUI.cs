using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthUI : MonoBehaviour
{
    public Image imageBackground; // Hình nền thanh máu
    public Image imageFill;       // Thanh máu điền
    public EnemyHealth enemyHealth;
    private Canvas canvas;
    private bool isVisible = false;
    private float hideDelay = 2f; // Thời gian ẩn UI sau khi hit
    private float hideTimer = 0f;
    public float heightRate = 2f;
    void Start()
    {
        canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main; // Đảm bảo camera đúng
        canvas.enabled = false; // Ẩn ban đầu
        imageFill.fillAmount = 1f; // Đặt đầy máu ban đầu
    }

    void Update()
    {
        if (enemyHealth.IsHit())
        {
            isVisible = true;
            canvas.enabled = true;
            UpdateHealthBar();
            transform.LookAt(Camera.main.transform.position); // Hướng về camera
            transform.Rotate(0, 180, 0); // Quay ngược lại để đúng hướng
        }

        if (isVisible)
        {
            hideTimer += Time.deltaTime;
            if (hideTimer >= hideDelay)
            {
                canvas.enabled = false;
                isVisible = false;
                hideTimer = 0f;
                enemyHealth.ResetHit(); // Đặt lại trạng thái hit
            }
        }

        // Đặt vị trí UI trên đầu Enemy
        transform.position = enemyHealth.transform.position + Vector3.up * heightRate; // Cao hơn đầu
    }

    void UpdateHealthBar()
    {
        float healthPercent = enemyHealth.GetCurrentHealth() / enemyHealth.GetMaxHealth();
        imageFill.fillAmount = Mathf.Clamp01(healthPercent); // Cập nhật thanh máu
    }
}