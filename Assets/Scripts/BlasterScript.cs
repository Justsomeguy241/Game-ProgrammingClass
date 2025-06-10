using UnityEngine;

public class BlasterScript : EnemyScript
{

    [SerializeField] private int blasterMinBounce = 3;
    [SerializeField] private int blasterMaxBounce = 5;
    [SerializeField] private int BlasterMinMoney = 3;
    [SerializeField] private int BlasterMaxMoney = 7;

    protected override void Start()
    {
        MinBounce = blasterMinBounce;
        MaxBounce = blasterMaxBounce;
        EnemyMinMoney = BlasterMinMoney;
        EnemyMaxMoney = BlasterMaxMoney;

        base.Start();
        SetBounceRange(MinBounce, MaxBounce); // Initial bounce range
    }


}
