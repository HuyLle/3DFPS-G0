using UnityEngine;
using UnityEngine.UI;

public class CrosshairUI : MonoBehaviour
{
    public Image crosshairImage;
    public Color defaultColor = Color.white; // Màu mặc định
    public Color lockOnColor = Color.red;   // Màu khi lock-on
    public Vector2 defaultSize = new Vector2(20f, 20f); // Kích thước mặc định
    public Vector2 lockOnSize = new Vector2(15f, 15f);  // Kích thước khi lock-on
    public float transitionSpeed = 5f; // Tốc độ chuyển đổi

    private Camera mainCamera;
    private RectTransform crosshairRect;
    private bool isLockedOn = false;

    void Start()
    {
        mainCamera = Camera.main;
        crosshairRect = crosshairImage.GetComponent<RectTransform>();
        crosshairImage.color = defaultColor;
        crosshairRect.sizeDelta = defaultSize;
    }

    void Update()
    {
        // Kiểm tra xem Crosshair có ngắm vào Enemy không
        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;
        isLockedOn = Physics.Raycast(ray, out hit, Mathf.Infinity) && hit.transform.CompareTag("Enemy");

        // Chuyển đổi màu và kích thước
        Color targetColor = isLockedOn ? lockOnColor : defaultColor;
        Vector2 targetSize = isLockedOn ? lockOnSize : defaultSize;

        crosshairImage.color = Color.Lerp(crosshairImage.color, targetColor, Time.deltaTime * transitionSpeed);
        crosshairRect.sizeDelta = Vector2.Lerp(crosshairRect.sizeDelta, targetSize, Time.deltaTime * transitionSpeed);
    }
}
