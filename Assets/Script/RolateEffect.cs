using UnityEngine;

public class RotateEffect : MonoBehaviour
{
    public float rotationSpeed = 50f; // Tốc độ xoay
    public float pulseSpeed = 2f; // Tốc độ thu vào và nở ra
    public float pulseAmount = 0.1f; // Biên độ nở ra

    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale; // Lưu kích thước gốc
    }

    void Update()
    {
        // Xoay vòng quanh trục Y
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);

        // Hiệu ứng thu vào và nở ra
        float scaleModifier = 1 + Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;
        transform.localScale = originalScale * scaleModifier;
    }
}
