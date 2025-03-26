using UnityEngine;
using UnityEngine.UI;

public class BloodOverlayUI : MonoBehaviour
{
    public Image bloodOverlay;
    public float flashDuration = 1f; // Thời gian nhấp nháy
    public float flashSpeed = 5f; // Tốc độ nhấp nháy
    private float flashTimer = 0f;
    private bool isFlashing = false;

    void Start()
    {
        bloodOverlay.enabled = false; // Ẩn ban đầu
    }

    public void Flash()
    {
        if (!isFlashing)
        {
            isFlashing = true;
            flashTimer = 0f;
            bloodOverlay.enabled = true;
        }
    }

    void Update()
    {
        if (isFlashing)
        {
            flashTimer += Time.deltaTime;
            if (flashTimer >= flashDuration)
            {
                isFlashing = false;
                bloodOverlay.enabled = false;
                return;
            }

            // Nhấp nháy bằng cách thay đổi alpha
            float alpha = Mathf.PingPong(flashTimer * flashSpeed, 1f);
            Color color = bloodOverlay.color;
            color.a = alpha;
            bloodOverlay.color = color;
        }
    }
}
