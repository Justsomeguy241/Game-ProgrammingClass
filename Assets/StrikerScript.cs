using UnityEngine;

public class StrikerScript : EnemyScript
{
    [SerializeField] private int blasterMinBounce = 4;
    [SerializeField] private int blasterMaxBounce = 6;
    [SerializeField] private int StrikerMinMoney = 2;
    [SerializeField] private int StrikerMaxMoney = 4;

    protected override void Start()
    {
        MinBounce = blasterMinBounce;
        MaxBounce = blasterMaxBounce;
        EnemyMinMoney = StrikerMinMoney;
        EnemyMaxMoney = StrikerMaxMoney;

        base.Start();
        SetBounceRange(MinBounce, MaxBounce); // Initial bounce range
    }
}
