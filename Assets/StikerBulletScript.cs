using UnityEngine;

public class StikerBulletScript : MonoBehaviour
{

    private StrikerScript strikerScript;
    private int damage = 1;
    public Vector3 direction = Vector3.down;
    public float speed = 10;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
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
    
}
