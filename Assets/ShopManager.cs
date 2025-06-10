using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShopManager : MonoBehaviour
{
    public GameObject shopPanel;

    [Header("Item Pools")]
    public List<ShopItem> upgradeItems; // Upgrade-only items
    public List<ShopItem> passiveItems; // Passive/item-only

    [Header("UI References")]
    public Transform upgradeSlotParent; // Top 3 slots
    public Transform itemSlotParent;    // Bottom 3 slots
    public GameObject itemSlotPrefab;   // Clickable button prefab with Name + Cost

    private PlayerScript player;

    private List<ShopItem> currentUpgrades = new List<ShopItem>();
    private List<ShopItem> currentItems = new List<ShopItem>();
    private HashSet<string> purchasedUpgrades = new HashSet<string>();

    private void Start()
    {
        player = FindFirstObjectByType<PlayerScript>();
        shopPanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseShop();
        }
    }

    public void OpenShop()
    {
        shopPanel.SetActive(true);
        GenerateItems();
    }

    public void CloseShop()
    {
        shopPanel.SetActive(false);
    }

    private void GenerateItems()
    {
        foreach (Transform child in upgradeSlotParent)
            Destroy(child.gameObject);
        foreach (Transform child in itemSlotParent)
            Destroy(child.gameObject);

        currentUpgrades.Clear();
        currentItems.Clear();

        int upgradesAdded = 0;
        int safetyLimit = 20;
        while (upgradesAdded < 3 && safetyLimit-- > 0)
        {
            List<ShopItem> available = upgradeItems.FindAll(item =>
            {
                if (purchasedUpgrades.Contains(item.itemName))
                    return false;

                // Prerequisites
                if (item.itemName == "Tripshot" && !purchasedUpgrades.Contains("Multishot"))
                    return false;
                if (item.itemName == "Quadshot" && !purchasedUpgrades.Contains("Tripshot"))
                    return false;

                return true;
            });

            if (available.Count == 0) break;

            ShopItem upgrade = available[Random.Range(0, available.Count)];
            currentUpgrades.Add(upgrade);

            GameObject slot = Instantiate(itemSlotPrefab, upgradeSlotParent);
            slot.transform.Find("Name").GetComponent<Text>().text = upgrade.itemName;
            slot.transform.Find("Cost").GetComponent<Text>().text = "$" + upgrade.cost;

            int index = upgradesAdded;
            slot.GetComponent<Button>().onClick.AddListener(() => BuyUpgrade(index));
            upgradesAdded++;
        }

        // Passive items (still random, no locking)
        for (int i = 0; i < 3; i++)
        {
            ShopItem item = passiveItems[Random.Range(0, passiveItems.Count)];
            currentItems.Add(item);

            GameObject slot = Instantiate(itemSlotPrefab, itemSlotParent);
            slot.transform.Find("Name").GetComponent<Text>().text = item.itemName;
            slot.transform.Find("Cost").GetComponent<Text>().text = "$" + item.cost;

            int index = i;
            slot.GetComponent<Button>().onClick.AddListener(() => BuyItem(index));
        }
    }

    public void BuyUpgrade(int index)
    {
        ShopItem item = currentUpgrades[index];

        if (player.Money >= item.cost)
        {
            player.Money -= item.cost;
            ApplyUpgrade(item);
            purchasedUpgrades.Add(item.itemName);
            CloseShop(); // optional
        }
    }

    public void BuyItem(int index)
    {
        ShopItem item = currentItems[index];

        if (player.Money >= item.cost)
        {
            player.Money -= item.cost;
            ApplyPassiveItem(item);
            // Optional: prevent repurchase of passive items too
        }
    }

    private void ApplyUpgrade(ShopItem item)
    {
        var upgrades = player.GetComponent<PlayerUpgradeManager>();

        switch (item.upgradeType)
        {
            case ShopItem.UpgradeType.Multishot:
                upgrades.hasMultishot = true;
                break;
            case ShopItem.UpgradeType.Tripshot:
                upgrades.hasTripshot = true;
                break;
            case ShopItem.UpgradeType.Quadshot:
                upgrades.hasQuadshot = true;
                break;
            case ShopItem.UpgradeType.SecondWind:
                upgrades.hasSecondWind = true;
                break;
            case ShopItem.UpgradeType.Piercing:
                upgrades.hasPiercing = true;
                break;
            case ShopItem.UpgradeType.Explosive:
                upgrades.hasExplosive = true;
                break;
            case ShopItem.UpgradeType.Knockback:
                upgrades.hasKnockback = true;
                break;
        }
    }

    private void ApplyPassiveItem(ShopItem item)
    {
        var upgrades = player.GetComponent<PlayerUpgradeManager>();

        switch (item.upgradeType)
        {
            
        }
    }
}
