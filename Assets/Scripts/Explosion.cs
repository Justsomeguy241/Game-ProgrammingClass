using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float lifetime = 0.5f;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }
}
