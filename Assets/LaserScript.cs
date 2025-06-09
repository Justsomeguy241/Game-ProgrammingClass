using UnityEngine;

public class LaserScript : MonoBehaviour
{

    public LancerScript lancerscript;
    public int damage;

    private void Start()
    {
       
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerScript player = collision.GetComponent<PlayerScript>();

            if ( player != null)
            {
                player.TakeDamage(damage);
            }
            {
                
            }
        }
    }

    public void SetLancerScript(LancerScript lancer)
    {
        lancerscript = lancer;
        damage = lancerscript.damage;
    }
}
