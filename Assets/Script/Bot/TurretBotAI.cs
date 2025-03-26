using UnityEngine;
using UnityEngine.Rendering;

public class TurretBotAI : MonoBehaviour
{
    public Transform player;
    public float detectionRange = 12f;
    public float attackRange = 10f;
    public float rotationSpeed = 5f;

    public GameObject vfx_hitEffect;
    public ParticleSystem vfx_muzzle;
    public ParticleSystem vfx_alert;
    public Transform firePoint;

    public float attackRate = 1f;
    private float nextAttackTime = 0f;
    public float damage = 20f;

    private Animator animator;
    private bool isActive = false;

    public GameObject energyBallPrefab;
    public GameObject barrier;
    public EnemyHealth health;

    private AudioSource audioSource;
    public AudioClip attackSound;
    public AudioClip alertSound;
    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        audioSource = GetComponent<AudioSource>();

        vfx_alert.Stop();
        vfx_muzzle.Stop();
        barrier.SetActive(false);
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            if (!isActive)
            {
                isActive = true;
                animator.SetBool("IsActive", true);
                vfx_alert.Play();
                audioSource.PlayOneShot(alertSound);
            }

            RotateTowardsPlayer();

            if (distanceToPlayer <= attackRange && Time.time >= nextAttackTime)
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
        else if (isActive)
        {
            isActive = false;
            animator.SetBool("IsActive", false);
        }

        if (health != null) 
        {
            if(health.currentHealth < health.maxHealth / 2f)
            {
                barrier.SetActive(true);
            }
        }
    }

    void RotateTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;

        // Tạo quaternion để xoay theo hướng player
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

        // Bổ sung góc xoay nếu model không hướng trục Z+
        Quaternion adjustedRotation = lookRotation * Quaternion.Euler(0, 90, 0); // Thay 90 nếu model lệch góc khác

        transform.rotation = Quaternion.Slerp(transform.rotation, adjustedRotation, rotationSpeed * Time.deltaTime);
    }


    void Attack()
    {
        vfx_muzzle.Play();
        //Instantiate(vfx_muzzle, firePoint.position, firePoint.rotation);
        audioSource.PlayOneShot(attackSound);
        
        if (energyBallPrefab == null || firePoint == null)
        {
            Debug.LogError("FireEnergyBall() failed: energyBallPrefab or firePoint is null!");
            return;
        }


        // Xác định vị trí mục tiêu (hạ thấp một chút để dễ trúng Player)
        Vector3 targetPos = player.position + Vector3.up * 1f; // Điều chỉnh độ cao

        // Tính toán hướng bắn
        Vector3 direction = (targetPos - firePoint.position).normalized;

        // Lấy góc quay cần thiết để hướng về mục tiêu
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        // Chỉ cập nhật góc X, giữ nguyên Y và Z của firePoint
        firePoint.rotation = Quaternion.Euler(lookRotation.eulerAngles.x, firePoint.rotation.eulerAngles.y, firePoint.rotation.eulerAngles.z);


        GameObject energyBall = Instantiate(energyBallPrefab, firePoint.position, firePoint.rotation);
        
    }

    public void TakeDamage()
    {
        animator.SetTrigger("OnDamaged");
    }
}

