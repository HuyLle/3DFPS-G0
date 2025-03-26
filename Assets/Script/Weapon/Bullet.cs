using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public float lifetime = 2f; // Đạn tự hủy sau 2 giây
    //public GameObject impactEffect;
    void Start()
    {
        // Thêm lực để đạn bay
        GetComponent<Rigidbody>().linearVelocity = transform.forward * speed;
        Destroy(gameObject, lifetime);
    }
    void OnCollisionEnter(Collision collision)
    {
        //Instantiate(impactEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}