using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats instance;
    public float maxHealth = 100f;
    public float currentHealth;
    public float maxStamina = 50f;
    public float currentStamina;
    public float staminaRegenRate = 5f; // Tốc độ hồi Stamina
    private bool isSprinting = false;

    public Slider healthSlider;
    public Slider staminaSlider;
    private BloodOverlayUI bloodOverlayUI;

    private AudioSource audioSource;
    public AudioClip healthSound;

    //public string sceneName = "";
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        currentHealth = maxHealth;
        currentStamina = maxStamina;

        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
        staminaSlider.maxValue = maxStamina;
        staminaSlider.value = currentStamina;

        bloodOverlayUI = FindObjectOfType<BloodOverlayUI>(); // Tìm BloodOverlayUI trong Scene
    }

    void Update()
    {
        // Giảm Stamina khi sprint
        isSprinting = Input.GetKey(KeyCode.LeftShift);/* && getcomponent<playermovement>().controller.velocity.magnitude > 0.1f;*/
        if (isSprinting && currentStamina > 0)
        {
            currentStamina -= Time.deltaTime * 30f; // Giảm Stamina khi chạy
        }
        else if (!isSprinting && currentStamina < maxStamina)
        {
            currentStamina += Time.deltaTime * staminaRegenRate; // Hồi Stamina
            currentStamina = Mathf.Min(currentStamina, maxStamina); // Giới hạn tối đa
        }

        // Kiểm tra chết (tạm thời)
        if (currentHealth <= 0)
        {
            //SceneManager.LoadScene(sceneName);
            Debug.Log("Game Over!");
            if (SceneLoader.Instance != null)
            {
                SceneLoader.Instance.LoadSceneByIndex(2);
            }
        }

        healthSlider.value = currentHealth;
        staminaSlider.value = currentStamina;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0); // Giới hạn tối thiểu
        bloodOverlayUI.Flash();
        if (currentHealth < 0) 
        {
            Debug.Log("PlayerDeaded");
        }
    }
    public void Heal (float hp)
    {
        currentHealth += hp;
        currentHealth = Mathf.Max(currentHealth, 0);
        audioSource.PlayOneShot(healthSound);
    }
}
