using UnityEngine;
using UnityEngine.UI;

public class AmmoUI : MonoBehaviour
{
    public Text ammoText; // Text hiển thị số đạn
    public Image ammoIcon; // Icon đạn
    public Color defaultColor = Color.white; // Màu mặc định
    public Color flashColor = Color.red;     // Màu khi nhấp nháy
    public float flashSpeed = 5f;           // Tốc độ nhấp nháy
    public float flashDuration = 1f;        // Thời gian nhấp nháy

    private float flashTimer = 0f;
    private bool isFlashing = false;

    void Start()
    {
        SetDefaultColor();
    }

    public void UpdateAmmoUI(int currentAmmo, int maxAmmo)
    {
        ammoText.text = $"{currentAmmo}/{maxAmmo}";
    }

    public void StartFlash(int currentAmmo)
    {
        if (!isFlashing && currentAmmo <=0)
        {
            isFlashing = true;
            flashTimer = 0f;
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
                SetDefaultColor();
                return;
            }

            // Nhấp nháy màu
            float t = Mathf.PingPong(flashTimer * flashSpeed, 1f);
            Color currentColor = Color.Lerp(defaultColor, flashColor, t);
            ammoText.color = currentColor;
            ammoIcon.color = currentColor;
        }
    }

    private void SetDefaultColor()
    {
        ammoText.color = defaultColor;
        ammoIcon.color = defaultColor;
    }
} 
