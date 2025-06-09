using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    public float movementSpeed;
    public int maxHP;
    public int damage;
    public float FireRate;
    protected int currentHP;

    protected virtual void Start()
    {
        currentHP = maxHP;
    }

    public virtual void TakeDamage(int amount)
    {
        currentHP -= amount;
        if (currentHP <= 0)
        {
            Die();
        }

        Debug.Log("take " + amount + " damage");
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}
