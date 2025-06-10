using UnityEngine;

public class LaserScript : MonoBehaviour
{
    public LancerScript lancerscript;
    public int damage;
    public Vector3 offset;

    private void Update()
    {
        if (lancerscript != null)
        {
            // Follow only the Lancer's X position (not Y)
            Vector3 newPosition = transform.position;
            newPosition.x = lancerscript.transform.position.x + offset.x;
            transform.position = newPosition;

            // Ensure Lancer knows it's still shooting
            lancerscript.SetIsShooting(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerScript player = collision.GetComponent<PlayerScript>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
        }
    }

    private void OnDestroy()
    {
        // Let the Lancer resume descending if the laser is destroyed
        if (lancerscript != null)
        {
            lancerscript.SetIsShooting(false);
        }
    }

    public void SetLancerScript(LancerScript lancer)
    {
        lancerscript = lancer;
        damage = lancer.damage;
    }

    
}
