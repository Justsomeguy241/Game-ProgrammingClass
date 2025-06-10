using UnityEngine;

public class UpgradePickup : MonoBehaviour
{
    [Tooltip("List of possible temporary upgrades")]
    public string[] possibleUpgrades = {
    "Multishot",
    "Tripshot",
    "Quadshot",
    "Piercing",
    "Explosive",
    "ChargeShot",
    "Knockback",
    "SecondWind" 
};


    public float duration = 10f; // Duration of the temporary upgrade

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerUpgradeManager manager = other.GetComponent<PlayerUpgradeManager>();
            if (manager != null)
            {
                string selectedUpgrade = possibleUpgrades[Random.Range(0, possibleUpgrades.Length)];
                manager.ApplyTemporaryUpgrade(selectedUpgrade, duration);
            }

            Destroy(gameObject); // Remove the pickup from the world
        }


    }
}
