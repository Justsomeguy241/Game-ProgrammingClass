using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public Vector3 direction = Vector3.up;
    public int damage;
    public bool haspierced = false;

    [Header("Explosion")]
    [SerializeField] private float explosionRadius = 2f;
    [SerializeField] private int explosionDamage = 1;
    [SerializeField] private LayerMask enemyLayer;

    public PlayerUpgradeManager playerUpgrade;
    public PlayerScript playerscript;

    public GameObject normalExplosionPrefab;
    public GameObject explosiveExplosionPrefab;
    public enum BulletType { Normal, Explosive }
    public BulletType bulletType = BulletType.Normal;

    private void Start()
    {

    }
    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyScript enemy = other.GetComponent<EnemyScript>();

            if (enemy != null)
            {
                enemy.TakeDamage(damage);


                // Knockback
                if (playerUpgrade != null && playerUpgrade.hasKnockback)
                {
                    Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
                    Collider2D col = other.GetComponent<Collider2D>();

                    if (rb != null && col != null)
                    {
                        Vector2 knockDir = (other.transform.position - transform.position).normalized;
                        enemy.ApplyKnockback(knockDir * 10f, 1.5f);
                    }


                }


                // Explosive
                if (playerUpgrade != null && playerUpgrade.hasExplosive)
                {
                    Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius, enemyLayer);
                    foreach (var hit in hits)
                    {
                        CharacterBase aoeEnemy = hit.GetComponent<CharacterBase>();
                        if (aoeEnemy != null && hit.gameObject != other.gameObject)
                        {
                            aoeEnemy.TakeDamage(explosionDamage);
                        }
                    }
                    
                }
            }

            GameObject explosionToSpawn = normalExplosionPrefab;

            switch (bulletType)
            {
                case BulletType.Explosive:
                    explosionToSpawn = explosiveExplosionPrefab;
                    break;
            }

            if (explosionToSpawn != null)
                Instantiate(explosionToSpawn, transform.position, Quaternion.identity);

            // Piercing
            if (playerUpgrade != null && playerUpgrade.hasPiercing && !haspierced)
            {
                haspierced = true;
            }
            else
            {
                Destroy(gameObject);
            }
        } 
        else
        {
            Destroy(gameObject);
        }
        
    }

    
    public void SetUpgradeManager(PlayerUpgradeManager upgradeManager)
    {
        playerUpgrade = upgradeManager;
        
    }

    public void SetPlayerScript(PlayerScript playerScript)
    {
        playerscript = playerScript;
        damage = playerScript.damage;
    }

    private IEnumerator TemporarilyDisableTrigger(Collider2D col, float duration)
    {
        col.isTrigger = false; // collide with walls
        yield return new WaitForSeconds(duration);
        col.isTrigger = true;  // back to normal behavior
    }

}
