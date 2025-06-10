using System.Collections;
using UnityEngine;

public class EnemyScript : CharacterBase
{
    [Header("Movement and Bounce")]
    [SerializeField] protected int currentFloor = 0;
    [SerializeField] protected int targetBounces = 0;
    [SerializeField] protected int bounceCount = 0;
    [SerializeField] protected float fireCooldown = 0f;
    [SerializeField] protected float bounceSpeed = 2f;
    [SerializeField] protected float verticalStep = 1.5f;
    [SerializeField] protected int maxFloors = 5;
    [SerializeField] protected bool goingRight;
    [SerializeField] protected bool isTrackingPlayer = false;
    [SerializeField] protected int EnemyMinMoney = 0;
    [SerializeField] protected int EnemyMaxMoney = 0;

    [Header("Shooting")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float minFireInterval = 1f;
    public float maxFireInterval = 2f;

    [Header("Bounce")]
    [SerializeField] protected int MinBounce = 1;
    [SerializeField] protected int MaxBounce = 3;
    private bool readyToDescend = false;
    [SerializeField] private float[] floorYPositions = { 3f, 2f, 1f, 0f, -1f };
    private bool movingToNextFloor = false;
    private Vector2 nextFloorPosition;
    public float descendSpeed = 3f;
    private float bounceCooldown = 0f;
    private const float bounceCooldownDuration = 0.1f;
    private bool isKnockedBack = false;


    public System.Action OnDeath;
    protected Rigidbody2D rb;
    protected Transform player;

    [Header("Upgrade Drop")]
    public GameObject[] possibleUpgradePrefabs; // Assign different upgrade prefabs in Inspector
    [Range(0f, 1f)]
    public float upgradeDropChance = 1.0f; // 30% chance




    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        goingRight = Random.value > 0.5f;
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        SetBounceRange(MinBounce, MaxBounce);
    }

    protected override void Start()
    {
        base.Start();
    }

    protected virtual void Update()
    {
        if (isKnockedBack) return;
        if (bounceCooldown > 0)
            bounceCooldown -= Time.deltaTime;

        if (!isTrackingPlayer && !isKnockedBack)
            HandleBounce();
        else
            TrackPlayer();

        HandleShooting();

        if (movingToNextFloor)
            MoveToNextFloor();
        else
            CheckAndReturnToFloor();
    }



    protected virtual void HandleBounce()
    {
        Vector2 direction = goingRight ? Vector2.right : Vector2.left;
        rb.linearVelocity = new Vector2(direction.x * bounceSpeed, rb.linearVelocity.y);
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

    protected virtual void TrackPlayer()
    {
        if (player == null) return;

        Vector2 dir = (player.position - transform.position).normalized;
        rb.linearVelocity = new Vector2(dir.x * bounceSpeed, dir.y * bounceSpeed);
    }

    public void SetBounceRange(int min, int max)
    {
        targetBounces = Random.Range(min, max + 1);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isTrackingPlayer && other.CompareTag("Wall") && bounceCooldown <= 0f)
        {
            goingRight = !goingRight;
            bounceCount++;
            bounceCooldown = bounceCooldownDuration; // start cooldown

            if (bounceCount >= targetBounces)
            {
                bounceCount = 0;

                if (currentFloor + 1 >= floorYPositions.Length)
                {
                    isTrackingPlayer = true;
                }
                else
                {
                    currentFloor++;
                    nextFloorPosition = new Vector2(transform.position.x, floorYPositions[currentFloor]);
                    movingToNextFloor = true;
                    SetBounceRange(MinBounce, MaxBounce);
                }
            }
        }



        if (isTrackingPlayer && other.CompareTag("Player"))
        {
            PlayerScript playerScript = other.GetComponent<PlayerScript>();
            if (playerScript != null)
            {
                playerScript.TakeDamage(damage);
            }
            Die();
        }
    }

    private void MoveToNextFloor()
    {
        float newY = Mathf.MoveTowards(transform.position.y, nextFloorPosition.y, descendSpeed * Time.deltaTime);
        transform.position = new Vector2(transform.position.x, newY);


        if (Mathf.Abs(transform.position.y - nextFloorPosition.y) < 0.01f)
        {
            movingToNextFloor = false;

         

            // Reset bounce cooldown
            bounceCooldown = 0f;
        }
    }


    private void CheckAndReturnToFloor()
    {
        float desiredY = floorYPositions[Mathf.Clamp(currentFloor, 0, floorYPositions.Length - 1)];
        float currentY = transform.position.y;

        if (Mathf.Abs(currentY - desiredY) > 0.05f)
        {
            float newY = Mathf.MoveTowards(transform.position.y, desiredY, descendSpeed * Time.deltaTime);
            transform.position = new Vector2(transform.position.x, newY);

        }
    }


    protected virtual bool CanDescend()
    {
        return true; // Default enemies always descend
    }

    

    public void ApplyKnockback(Vector2 force, float duration = 0.7f)
    {
        StartCoroutine(KnockbackRoutine(force, duration));
    }

    private IEnumerator KnockbackRoutine(Vector2 force, float duration)
    {
        isKnockedBack = true;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        Collider2D col = GetComponent<Collider2D>();

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(force, ForceMode2D.Impulse);
        }

        if (col != null)
            col.isTrigger = false;

        // Wait a short time to simulate "push"
        yield return new WaitForSeconds(0.2f);  // Push duration (tweak this)

        // Stop movement
        if (rb != null)
            rb.linearVelocity = Vector2.zero;

        // Wait out rest of the stun
        yield return new WaitForSeconds(duration - 0.2f);

        // Re-enable trigger
        if (col != null)
            col.isTrigger = true;

        // Snap back to correct Y position (still useful)
        

        isKnockedBack = false;
    }

    protected override void Die()
    {
        OnDeath?.Invoke();
        if (possibleUpgradePrefabs.Length > 0 && Random.value <= upgradeDropChance)
        {
            GameObject upgradePrefab = possibleUpgradePrefabs[Random.Range(0, possibleUpgradePrefabs.Length)];
            Instantiate(upgradePrefab, transform.position, Quaternion.identity);
        }
        base.Die();
    }

    
}
