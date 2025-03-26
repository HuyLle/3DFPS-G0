using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float detectionRange = 12f;
    public float attackRange = 7f;
    public float moveSpeed = 3f;
    public float attackRate = 1f;
    public int damage = 10;
    public Transform eyePoint;
    public GameObject flashEffectPrefab;

    private Animator animator;
    private float nextAttackTime = 0f;
    private bool isAlerted = false;
    private bool isAttacking = false;
    private bool isRunning = false;
    private bool canTransitionToRun = false;
    private float alertedDuration = 1f;

    // Patrol variables
    private Vector3 patrolStartPos;
    private Vector3 patrolTargetPos;
    private float patrolRange = 5f;
    private bool isPatrolling = false;

    public ParticleSystem vfx_alert;
    public GameObject vfx_muzzle;
    public Transform muzzlePoint;

    private NavMeshAgent agent;
    public float waitTime = 1.5f;
    private Coroutine patrolRoutine;

    private AudioSource audioSource;
    public AudioClip shootSound;
    public AudioClip alertSound;

    public Material eyeball;
    [ColorUsageAttribute(true, true)] public Color defaultEyeballColor;
    [ColorUsageAttribute(true, true)] public Color attackEyeballColor;


    void Start()
    {
        vfx_alert.Stop();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (eyePoint == null)
        {
            Debug.LogWarning("eyePoint not assigned! Using transform.position as default.");
            eyePoint = transform;
        }
        patrolStartPos = transform.position;

        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        agent.autoBraking = false;

        patrolRoutine = StartCoroutine(Patrol()); // Bắt đầu tuần tra ngay từ đầu
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            if (!isAlerted)
            {
                isAlerted = true;
                animator.SetBool("isAlerted", true);
                vfx_alert.Play();
                audioSource.PlayOneShot(alertSound);
                StartCoroutine(WaitForAlerted());
            }

            if (canTransitionToRun)
            {
                if (patrolRoutine != null)
                {
                    StopCoroutine(patrolRoutine); // Ngừng tuần tra ngay khi thấy Player
                    patrolRoutine = null;
                    isPatrolling = false;
                }

                if (distanceToPlayer > attackRange)
                {
                    ChasePlayer();
                }
                else if (distanceToPlayer < attackRange * 0.5f)
                {
                    RetreatFromPlayer();
                }
                else
                {
                    StopChase();
                    if (Time.time >= nextAttackTime)
                    {
                        Attack();
                        nextAttackTime = Time.time + 1f / attackRate;
                    }
                }
            }

            transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
        }
        else
        {
            isAlerted = false;
            isRunning = false;
            canTransitionToRun = false;
            isAttacking = false;
            animator.SetBool("isAlerted", false);
            animator.SetBool("isRunning", false);

            if (!isPatrolling && patrolRoutine == null)
            {
                patrolRoutine = StartCoroutine(Patrol());
            }
        }
    }

    IEnumerator WaitForAlerted()
    {
        yield return new WaitForSeconds(alertedDuration);
        canTransitionToRun = true;
    }

    void Attack()
    {
        ChangeEyeballColor(attackEyeballColor);
        animator.SetTrigger("Shoot");
        Instantiate(vfx_muzzle, muzzlePoint.position, muzzlePoint.rotation);
        audioSource.PlayOneShot(shootSound);
        RaycastHit hit;
        if (Physics.Raycast(eyePoint.position, eyePoint.forward, out hit, attackRange))
        {
            Instantiate(flashEffectPrefab, hit.point, Quaternion.LookRotation(hit.normal));
            PlayerStats playerStats = hit.transform.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(damage);
            }
        }
        // Đặt lại màu mắt sau 0.2 giây
        Invoke(nameof(ResetEyeballColor), 0.5f);
    }

    void ChangeEyeballColor(Color newColor)
    {
        if (eyeball != null)
        {
            Renderer renderer = GetComponent<Renderer>(); // Lấy Renderer của mắt
            if (renderer != null)
            {
                MaterialPropertyBlock mpb = new MaterialPropertyBlock();
                renderer.GetPropertyBlock(mpb);
                mpb.SetColor("_EmissionColor", newColor);
                renderer.SetPropertyBlock(mpb);
            }
        }
    }



    void ResetEyeballColor()
    {
        ChangeEyeballColor(defaultEyeballColor);
    }

    void ChasePlayer()
    {
        if (agent.isActiveAndEnabled)
        {
            agent.SetDestination(player.position);
            animator.SetBool("isRunning", true);
        }
    }

    void RetreatFromPlayer()
    {
        if (agent.isActiveAndEnabled)
        {
            Vector3 direction = (transform.position - player.position).normalized;
            Vector3 targetPosition = transform.position + direction * attackRange;
            agent.SetDestination(targetPosition);
            animator.SetBool("isRunning", true);
        }
    }

    void StopChase()
    {
        if (agent.isActiveAndEnabled)
        {
            agent.ResetPath();
        }
        isRunning = false;
        animator.SetBool("isRunning", false);
        isAttacking = true;
        vfx_alert.Stop();
    }

    private IEnumerator Patrol()
    {
        isPatrolling = true;

        while (!isAlerted)
        {
            patrolTargetPos = GetValidPatrolPoint();
            agent.SetDestination(patrolTargetPos);
            animator.SetBool("isRunning", true);

            while (Vector3.Distance(transform.position, patrolTargetPos) > 0.5f && !isAlerted)
            {
                yield return null;
            }

            animator.SetBool("isRunning", false);
            yield return new WaitForSeconds(waitTime);
        }

        isPatrolling = false;
    }

    private Vector3 GetValidPatrolPoint()
    {
        int attempts = 10;
        Vector3 randomPoint;
        do
        {
            float randomX = Random.Range(-patrolRange, patrolRange);
            float randomZ = Random.Range(-patrolRange, patrolRange);
            Vector3 rawTarget = patrolStartPos + new Vector3(randomX, 0, randomZ);

            NavMeshHit hit;
            if (NavMesh.SamplePosition(rawTarget, out hit, 2f, NavMesh.AllAreas))
            {
                return hit.position;
            }

            attempts--;

        } while (attempts > 0);

        return transform.position;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}

