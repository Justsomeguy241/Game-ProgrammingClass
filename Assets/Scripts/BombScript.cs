using UnityEngine;

public class BombScript : MonoBehaviour
{
    public GameObject explosionEffectPrefab;
    public float explosionRadius = 2f;
    public int explosionDamage = 1;
    public LayerMask PlayerLayer;
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Floor") || other.collider.CompareTag("Player"))
        {
            Explode();
        }
    }

    void Explode()
    {
        // Visual effect
        if (explosionEffectPrefab != null)  
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }

        // Damage enemies in radius
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius, PlayerLayer);
        foreach (var hit in hits)
        {
            PlayerScript player = hit.GetComponent<PlayerScript>();
            if (player != null)
            {
                player.TakeDamage(explosionDamage);
            }
        }

        Destroy(gameObject); // Destroy bomb after exploding
    }

    private void OnDrawGizmosSelected()
    {
        // Draw explosion radius in editor 
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
