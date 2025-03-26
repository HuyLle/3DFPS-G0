using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100f; // Máu tối đa
    public float currentHealth;
    //private EnemyAI enemyAI;
    private Animator anim;
    private bool isHit = false; // Để kiểm soát UI hiển thị
    public ParticleSystem[] hitSparks; // Mảng các VFX_hitspark

    public GameObject explosionEffectPrefab;
    public Transform explosionPoit;
    public Transform dropPoit;
    public bool isTurretBot = false;
    //public GameObject objectivePoint;

    public ParticleSystem steamEffectI;
    public ParticleSystem steamEffectII;

    public GameObject healthLoot;
    public Transform[] summonPoints;
    public GameObject objectiveSummon;
    

    private MissionUI missionUI;

    private bool spawnedAtHalfHealth = false;
    private bool spawnedAtLowHealth = false;

    void Start()
    {
        currentHealth = maxHealth;
        //enemyAI = GetComponent<EnemyAI>();
        anim = GetComponent<Animator>();
      
        

        missionUI = FindObjectOfType<MissionUI>();       
    }
    private void Update()
    { 
        if (currentHealth < maxHealth / 1.5f && !spawnedAtHalfHealth)
        {
            spawnedAtHalfHealth = true; // Đánh dấu đã spawn ở 1.5x
            steamEffectI.Play();
            if (isTurretBot) 
            {
                SpawnObjectives();
            }
        }
        else if (currentHealth < maxHealth / 3f && !spawnedAtLowHealth)
        {
            spawnedAtLowHealth = true;
            steamEffectII.Play();
            if (isTurretBot)
            {
                SpawnObjectives();
            }
        }    
    }

    void SpawnObjectives()
    {
        foreach (Transform point in summonPoints)
        {
            if (objectiveSummon != null && point != null)
            {
                Instantiate(objectiveSummon, point.position, point.rotation);
            }
        }
    }
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        isHit = true; // Kích hoạt UI khi bị hit
        anim.SetTrigger("OnDamaged"); // Chạy animation Hurt
        PlayRandomHitSpark();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        //enemyAI.enabled = false; // Tắt AI khi chết
        anim.SetTrigger("OnDamaged"); // Có thể dùng animation Die nếu có
        // Hiệu ứng nổ 
        Instantiate(explosionEffectPrefab, explosionPoit.position, Quaternion.identity);
        DropHealthPickup();

        if (missionUI != null)
        {
            missionUI.EnemyDefeated(gameObject.tag);
        }
        if (isTurretBot)
        {
            ExplodeAllHoverBots();
        }
        Destroy(gameObject, 0.2f);
    }

    // Hàm rớt health_pickup (để gọi sau)
    void DropHealthPickup()
    {
        //if (!isTurretBot) Instantiate(healthLoot, dropPoit.position, Quaternion.identity);
        //else Instantiate(objectivePoint, dropPoit.position, Quaternion.identity);

        Instantiate(healthLoot, dropPoit.position, Quaternion.identity);
    }
    public bool IsHit()
    {
        return isHit;
    }

    public void ResetHit()
    {
        isHit = false; // Đặt lại sau khi UI ẩn
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    void PlayRandomHitSpark()
    {
        if (hitSparks.Length > 0)
        {
            int randomIndex = Random.Range(0, hitSparks.Length);
            ParticleSystem selectedSpark = hitSparks[randomIndex];
            //selectedSpark.transform.position = transform.position; // Đặt vị trí tại Enemy
            selectedSpark.Play(); // Chạy hiệu ứng
        }
    }

    void ExplodeAllHoverBots()
    {
        GameObject[] hoverBots = GameObject.FindGameObjectsWithTag("Enemy"); // Giả sử HoverBot có tag "Enemy"

        foreach (GameObject bot in hoverBots)
        {
            EnemyHealth enemy = bot.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.ForceDie(); // Gọi hàm để HoverBot chết ngay lập tức
            }
        }
    }

    public void ForceDie()
    {
        // Gọi hiệu ứng nổ
        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }

        // Hủy bot
        Destroy(gameObject);
    }
}