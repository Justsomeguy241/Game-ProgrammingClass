using UnityEngine;

[CreateAssetMenu(fileName = "NewShopItem", menuName = "Shop/Item")]
public class ShopItem : ScriptableObject
{
    public string itemName;
    public string description;
    public int cost;
    public Sprite icon;

    public enum UpgradeType {Multishot, Tripshot, Quadshot, Piercing, Explosive, Charge, Knockback, SecondWind, Momentumshift  }
    public UpgradeType upgradeType;


}
