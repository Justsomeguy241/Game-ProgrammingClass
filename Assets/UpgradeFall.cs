using Unity.VisualScripting;
using UnityEngine;

public class UpgradePickupFall : MonoBehaviour
{
    public float fallSpeed = 2f;
    private bool hasLanded = false;

    void Update()
    {
        if (hasLanded) return;

        // Move downward manually
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;

        // Raycast down a short distance
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.1f);

        // Check if the hit object has the "Floor" tag
        if (hit.collider != null && hit.collider.CompareTag("Floor"))
        {
            hasLanded = true;

            // Optional: snap to the top of the floor
            transform.position = new Vector3(transform.position.x, hit.point.y + 0.1f, transform.position.z);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Floor"))
        {
            hasLanded = true;
        }
    }
    void OnDrawGizmosSelected()
    {
        // Visual debug line
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * 0.1f);
    }
}
