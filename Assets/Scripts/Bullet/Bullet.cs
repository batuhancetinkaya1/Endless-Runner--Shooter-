using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float baseSpeed = 10f;
    [SerializeField] private float lifetime = 3f;
    private float speed;

    private void Start()
    {
        BulletManager.RegisterBullet(gameObject);
        Destroy(gameObject, lifetime);
        speed = baseSpeed;
    }

    private void OnDestroy()
    {
        BulletManager.UnregisterBullet(gameObject);
    }

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    public void SetSpeedBoost(float speedBoost)
    {
        speed = baseSpeed + speedBoost;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}