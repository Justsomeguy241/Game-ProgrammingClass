using UnityEngine;

public class EnemyScript : CharacterBase
{
    [SerializeField] protected int currentFloor = 0;
    [SerializeField] protected int targetBounces;
    [SerializeField] protected int bounceCount = 0;
    [SerializeField] protected float fireCooldown = 0f;

    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bounceSpeed = 2f;
    public float minFireInterval = 1f;
    public float maxFireInterval = 2f;

    protected virtual void Awake()
    {
        
    }

    protected override void Start()
    {
        base.Start();
    }

    protected virtual void Update()
    {
        HandleBounce();
        
        HandleShooting();
    }

    protected virtual void HandleBounce()
    {
        // Placeholder: move left/right and bounce off walls
        // After X bounces, go to next floor
        // You'd need collision detection or manual position tracking here
    }

    protected virtual void HandleShooting()
    {
        fireCooldown += Time.deltaTime;
        if (fireCooldown >= FireRate)
        {
            Shoot();
            fireCooldown = Random.Range(minFireInterval, maxFireInterval);
        }
    }

    protected virtual void Shoot()
    {
        if (bulletPrefab != null && firePoint != null)
        {
            Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        }
    }

    public void SetBounceRange(int min, int max)
    {
        targetBounces = Random.Range(min, max + 1);
    }
}
