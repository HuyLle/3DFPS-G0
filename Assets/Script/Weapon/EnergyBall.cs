using UnityEngine;

public class EnergyBall : MonoBehaviour
{
    public float speed = 10f;
    public float explosionRadius = 3f;
    public int damage = 20;
    public float lifetime = 3f;
    public GameObject explosionEffect; 
    public AudioClip explosionSound;

    void Start()
    {
        // Thêm lực để đạn bay
        GetComponent<Rigidbody>().linearVelocity = transform.forward * speed;
        Destroy(gameObject, lifetime);
    }
    void OnCollisionEnter(Collision collision)
    {
        Explode();
    }
    void Explode()
    {
        
        Instantiate(explosionEffect, transform.position, Quaternion.identity);

        // Phát âm thanh nổ
        if (explosionSound != null)
        {
            AudioSource.PlayClipAtPoint(explosionSound, transform.position);
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider nearbyObject in colliders)
        {
            if (nearbyObject.CompareTag("Player"))
            {
                PlayerStats player = nearbyObject.GetComponent<PlayerStats>();
                if (player != null)
                {
                    player.TakeDamage(damage);
                }
            }
        }
        Destroy(gameObject);
    }
}

