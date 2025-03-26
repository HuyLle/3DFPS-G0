using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    public Transform firePoint; // Vị trí bắn (họng súng)
    public GameObject bulletPrefab; // Prefab của đạn
    public float fireRate = 0.5f; // Tốc độ bắn (giây/lần bắn)
    public float range = 100f; // Tầm bắn
    private float nextFireTime = 0f;

    public int maxAmmo = 30; // Số đạn tối đa trong băng
    public int currentAmmo;  // Số đạn hiện tại
    public float reloadTime; // Thời gian nạp đạn
    private bool isReloading = false;

    public GameObject impactEffect;
    //public ParticleSystem muzzleEffect;
    public GameObject muzzleParticle;
    public Transform headPoint;
    private Animator anim;

    public Transform cameraTransform; // Gắn Main Camera vào đây
    public float recoilAmount = 1f;

    private AudioSource audioSource;
    public AudioClip shootSound;
    public AudioClip emptyBulletSound;

    //public Text ammoDisplay;
    public float damage = 10f;

    //public Vector3 aimPosition; // Vị trí súng khi nhắm (giữa màn hình)
    //public Vector3 hipPosition; // Vị trí súng khi không nhắm (mặc định)
    public float aimFOV = 40f;  // FOV khi nhắm (zoom nhẹ)
    private float defaultFOV;   // FOV mặc định
    public float aimSpeed = 1f; // Tốc độ chuyển đổi khi nhắm
    private bool isAiming = false;
    private Camera mainCamera;

    public bool isSniper = false; // Đánh dấu đây là Sniper
    public GameObject scopeOverlay; // Gắn ScopeOverlay vào đây
    public GameObject crosshair;    // Gắn Crosshair vào đây

    private AmmoUI ammoUI; // Tham chiếu đến AmmoUI

    private void Start()
    {
        //muzzleEffect.Stop();
        currentAmmo = maxAmmo; // Khởi tạo băng đạn đầy
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        //hipPosition = transform.localPosition; // Lưu vị trí ban đầu
        mainCamera = Camera.main;
        defaultFOV = mainCamera.fieldOfView;

        //ammoDisplay.text = $"{currentAmmo}/{maxAmmo}";
        ammoUI = FindObjectOfType<AmmoUI>(); // Tìm AmmoUI trong Scene
        ammoUI.UpdateAmmoUI(currentAmmo, maxAmmo); // Cập nhật UI ban đầu
    }
    void Update()
    {
        // Không bắn khi đang nạp đạn
        if (isReloading) return;

        // Nhấn chuột phải để nhắm
        isAiming = Input.GetMouseButton(1);
        if (isAiming) anim.SetBool("Scope", true);
        else anim.SetBool("Scope", false);

        // Chuyển đổi vị trí súng
        //Vector3 targetPosition = isAiming ? aimPosition : hipPosition;
        //transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * aimSpeed);

        // Zoom camera
        float targetFOV = isAiming ? aimFOV : defaultFOV;
        mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, targetFOV, Time.deltaTime * aimSpeed);

        // Xử lý zoom và scope cho Sniper
        if (isSniper)
        {
            targetFOV = isAiming ? aimFOV : defaultFOV;
            mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, targetFOV, Time.deltaTime * aimSpeed);
            scopeOverlay.SetActive(isAiming);
            crosshair.SetActive(!isAiming);
        }
        else
        {
            targetFOV = isAiming ? aimFOV : defaultFOV;
            mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, targetFOV, Time.deltaTime * aimSpeed);
            scopeOverlay.SetActive(false);
            crosshair.SetActive(true);
        }

        // Nạp đạn khi nhấn phím R
        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo)
        {
            //StartCoroutine(Reload());
            Reload();

            //ammoDisplay.text = $"{currentAmmo}/{maxAmmo}";
            ammoUI.UpdateAmmoUI(currentAmmo, maxAmmo);
            return;
        }
        // Bắn khi nhấn chuột trái
        if (Input.GetMouseButtonDown(0) && Time.time >= nextFireTime)
        {
            if(currentAmmo > 0)
            {
                Shoot();
                currentAmmo--; // Giảm số đạn
                
                ammoUI.UpdateAmmoUI(currentAmmo, maxAmmo);
                nextFireTime = Time.time + fireRate;
            }
            else
            {
                audioSource.PlayOneShot(emptyBulletSound);
                ammoUI.StartFlash(currentAmmo);
            }
        }
    }

    void Shoot()
    {   
        Instantiate(muzzleParticle, headPoint.position, headPoint.rotation);
        if(isAiming) anim.SetTrigger("SpShoot");
        else anim.SetTrigger("Shoot");
        //muzzleEffect.Play();
        audioSource.PlayOneShot(shootSound);

        RaycastHit hit;

        var hitPosition = Camera.main.transform.position + Camera.main.transform.forward * range;
        var direction = hitPosition - firePoint.position;
        // Tạo đạn tại vị trí họng súng
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        if (Physics.Raycast(firePoint.position, /*firePoint.forward*/ direction, out hit, range))
        {
            Debug.Log("Hit: " + hit.transform.name);
            // Tạo hiệu ứng va chạm tại điểm trúng
            Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            //EnemyHealth enemy = hit.transform.GetComponent<EnemyHealth>();
            EnemyHealth enemy = hit.transform.GetComponentInParent<EnemyHealth>();
            if (enemy != null)
            {
                if(enemy.currentHealth > 0)
                {
                    enemy.TakeDamage(damage);
                }    
            }
        }

        // Thêm recoil
        cameraTransform.localRotation *= Quaternion.Euler(-recoilAmount, 0, 0);
    }

    void Reload()
    {
        isReloading = true;
        anim.SetTrigger("Reload");
        currentAmmo = maxAmmo;
        isReloading = false;
       
    }
    //System.Collections.IEnumerator Reload()
    //{
    //    isReloading = true;
    //    anim.SetTrigger("Reload"); // Trigger animation nạp đạn (sẽ thêm sau)
    //    yield return new WaitForSeconds(reloadTime);
    //    currentAmmo = maxAmmo;
    //    isReloading = false;
    //}
}

