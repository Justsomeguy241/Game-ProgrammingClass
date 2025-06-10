using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerScript : CharacterBase
{
    public PlayerUpgradeManager upgradeManager;
    public PlayerScript playerscript;

    private float chargetime = 0f;
    private bool ischarging = false;
    public int Money;

    public GameObject bulletPrefab;
    public GameObject ChargeShotPreFab;

    public List<Transform> firePoints = new List<Transform>();
    public GameObject chargeEffectPrefab;

    private bool hasPlayedChargeLoop = false;
    private List<GameObject> activeChargeEffects = new List<GameObject>();
    public HealthBarUI healthBarUI;


    [Header("Spirtes")]
    public Sprite baseSprite;
    public Sprite multishotSprite;
    public Sprite tripshotSprite;
    public Sprite quadshotSprite;
    private SpriteRenderer spriteRenderer;

    private float nextFireTime = 0f;

    [Header("Adrenaline Surge")]
    public float adrenalineDuration = 3f;
    public float speedBoostAmount = 3f;
    public int damageBoostAmount = 1;

    private bool isAdrenalineActive = false;
    private float originalSpeed;
    private int originalDamage;


    protected override void Start()
    {
        

        base.Start();   

        playerscript = GetComponent<PlayerScript>();
        upgradeManager = GetComponent<PlayerUpgradeManager>();
        spriteRenderer = GetComponent<SpriteRenderer>(); 
        spriteRenderer.sprite = baseSprite; // Set default


        healthBarUI.UpdateHealthBar(3);
    }

    void Update()
    {
        HandleInput();

        Vector3 mousePos = Input.mousePosition;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        worldPos.z = 0; // Important for 2D games to keep it on the correct plane

        if (Input.GetKeyDown(KeyCode.Escape))
        {
           Application.Quit();
        }

        
        
    }

    void HandleInput()
    {
        // Movement
        float moveX = Input.GetAxisRaw("Horizontal");
        Vector3 movement = new Vector3(moveX, 0f, 0f) * movementSpeed * Time.deltaTime;
        transform.position += movement;

        // Regular Shoot
        if (Input.GetKeyDown(KeyCode.Space) && !upgradeManager.hasChargeShot)
        {
            Shoot();
        }

        // Charge Shot handling
        if (upgradeManager.hasChargeShot)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                ischarging = true;
                chargetime += Time.deltaTime;

                // Instantiate charge effects if not already spawned
                if (activeChargeEffects.Count == 0)
                {
                    foreach (Transform firePoint in GetActiveFirePoints())
                    {
                        GameObject effect = Instantiate(chargeEffectPrefab, firePoint.position, Quaternion.identity, firePoint);
                        Animator anim = effect.GetComponent<Animator>();
                        if (anim != null) anim.Play("ChargeStart");

                        activeChargeEffects.Add(effect);
                    }

                    hasPlayedChargeLoop = false;
                }

                // Switch to Charging loop animation
                if (chargetime >= 0.7f && !hasPlayedChargeLoop)
                {
                    foreach (GameObject effect in activeChargeEffects)
                    {
                        Animator anim = effect.GetComponent<Animator>();
                        if (anim != null) anim.Play("Charging");
                    }
                    hasPlayedChargeLoop = true;
                }
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                if (chargetime >= 1f)
                {
                    ChargeShot();
                }
                else
                {
                    Shoot();
                }

                ischarging = false;
                chargetime = 0f;
                hasPlayedChargeLoop = false;

                // Destroy charge effects
                foreach (GameObject effect in activeChargeEffects)
                {
                    Destroy(effect);
                }
                activeChargeEffects.Clear();
            }
        }
    }

    void Shoot()
    {
        foreach (Transform point in GetActiveFirePoints())
        {
            GameObject bullet = Instantiate(bulletPrefab, point.position, Quaternion.identity);
            Bullet bulletScript = bullet.GetComponent<Bullet>();

            if (bulletScript != null)
            {
                // Assign upgradeManager reference
                bulletScript.SetUpgradeManager(upgradeManager);
                bulletScript.SetPlayerScript(playerscript);

                // Determine bullet type
                if (upgradeManager.hasExplosive)
                {
                    bulletScript.bulletType = Bullet.BulletType.Explosive;
                }
                else
                {
                    bulletScript.bulletType = Bullet.BulletType.Normal;
                }
            }
            else
            {
                Debug.LogWarning("Bullet prefab is missing Bullet script!");
            }
        }
    }



    void ChargeShot()
    {
        foreach (Transform point in GetActiveFirePoints())
        {
            GameObject shot = Instantiate(ChargeShotPreFab, point.position, Quaternion.identity);
            ChargeBulletScript bulletScript = shot.GetComponent<ChargeBulletScript>();
            if (bulletScript != null)
            {
                bulletScript.SetUpgradeManager(upgradeManager);

                if (upgradeManager.hasExplosive)
                {
                    bulletScript.bulletType = ChargeBulletScript.BulletType.Charge_Explosive;
                }
                else
                {
                    bulletScript.bulletType = ChargeBulletScript.BulletType.Charge;
                }
            }
        }
    }


    List<Transform> GetActiveFirePoints()
    {
        List<Transform> selectedPoints = new();

        // Safety check
        if (firePoints.Count < 5)
        {
            Debug.LogWarning("Not enough fire points assigned to PlayerScript.");
            return selectedPoints;
        }


        if (upgradeManager.hasQuadshot)
        {
            selectedPoints.Add(firePoints[0]); // Far Left
            selectedPoints.Add(firePoints[1]); // Left
            selectedPoints.Add(firePoints[3]); // Right
            selectedPoints.Add(firePoints[4]); // Far Right
        }
        else if (upgradeManager.hasTripshot)
        {
            selectedPoints.Add(firePoints[0]); // Far Left
            selectedPoints.Add(firePoints[2]); // Center
            selectedPoints.Add(firePoints[4]); // Far Right
        }
        else if (upgradeManager.hasMultishot)
        {
            selectedPoints.Add(firePoints[1]); // Left
            selectedPoints.Add(firePoints[3]); // Right
        }
        else
        {
            selectedPoints.Add(firePoints[2]); // Center
        }

        return selectedPoints;
    }

    public void UpdateShipSprite()
    {
        if (upgradeManager.hasQuadshot)
            spriteRenderer.sprite = quadshotSprite;
        else if (upgradeManager.hasTripshot)
            spriteRenderer.sprite = tripshotSprite;
        else if (upgradeManager.hasMultishot)
            spriteRenderer.sprite = multishotSprite;
        else
            spriteRenderer.sprite = baseSprite;
    }

    public override void TakeDamage(int amount)
    {
        if (upgradeManager.hasAdrenalineSurge)
        {
            ActivateAdrenalineSurge();
        }

        ;
        base.TakeDamage(amount);
        healthBarUI.UpdateHealthBar(currentHP);

    }

    void ActivateAdrenalineSurge()
    {
        if (isAdrenalineActive) return;

        isAdrenalineActive = true;

        originalSpeed = movementSpeed;
        originalDamage = damage;

        movementSpeed += speedBoostAmount;
        damage += damageBoostAmount;

        StartCoroutine(ResetAdrenalineSurge());
    }
    protected override void Die()
    {
        if (upgradeManager.hasSecondWind && !upgradeManager.hasUsedSecondWind)
        {
            upgradeManager.hasUsedSecondWind = true;
            currentHP = 1;
        }
        else
        {
            // Stop enemy spawning
            WaveManager waveManager = FindFirstObjectByType<WaveManager>();
            if (waveManager != null)
            {
                waveManager.StopAllCoroutines(); // Stops further spawning
            }
         

            // Show Game Over screen
            GameOverManager gameOverManager = FindFirstObjectByType<GameOverManager>();
            if (gameOverManager != null)
            {
                gameOverManager.ShowGameOver(); 
            }

            // Optionally disable the player object
            gameObject.SetActive(false);
        }
    }

    System.Collections.IEnumerator ResetAdrenalineSurge()
    {
        yield return new WaitForSeconds(adrenalineDuration);

        movementSpeed = originalSpeed;
        damage = originalDamage;
        isAdrenalineActive = false;
    }
}
