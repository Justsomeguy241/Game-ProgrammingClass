using UnityEngine;

public class LancerScript : EnemyScript
{
    public float chargeDuration = 2f;
    public float laserDuration = 3f;
    public float laserCooldown = 4f;
    public GameObject chargeEffect;
    public GameObject laserBeamPrefab;

    public float stateTimer = 3f;
    private GameObject beam;
    private GameObject activeChargeEffect;

    private enum AttackState { Idle, Charging, Firing, Cooldown }
    [SerializeField] private AttackState currentState = AttackState.Idle;

    [SerializeField] private int blasterMinBounce = 5;
    [SerializeField] private int blasterMaxBounce = 7;
    [SerializeField] private int lancerMinMoney = 5;
    [SerializeField] private int lancerMaxMoney = 7;

    protected override void Start()
    {
        MinBounce = blasterMinBounce;
        MaxBounce = blasterMaxBounce;
        EnemyMinMoney = lancerMinMoney;
        EnemyMaxMoney = lancerMaxMoney;

        base.Start();
        SetBounceRange(MinBounce, MaxBounce); // Initial bounce range
    }

    protected override void Update()
    {
        base.Update(); // Handles bouncing and moving down
        HandleLaserAttack();
    }

    private void HandleLaserAttack()
    {
        stateTimer -= Time.deltaTime;

        switch (currentState)
        {
            case AttackState.Idle:
                if (stateTimer <= 0 && currentState ==  AttackState.Idle)
                {
                    StartCharging();
                }
                break;

            case AttackState.Charging:
                if (stateTimer <= 0 && currentState == AttackState.Charging)
                {
                    FireLaser();
                }
                break;

            case AttackState.Firing:
                if (stateTimer <= 0 && currentState == AttackState.Firing)
                {
                    EndLaser();
                }
                break;

            case AttackState.Cooldown:
                if (stateTimer <= 0 && currentState == AttackState.Cooldown)
                {
                    currentState = AttackState.Idle;
                    stateTimer = Random.Range(minFireInterval, maxFireInterval);
                }
                break;
        }
    }

    private void StartCharging()
    {
        currentState = AttackState.Charging;
        stateTimer = chargeDuration;

        if (chargeEffect != null)
        {
            activeChargeEffect = Instantiate(chargeEffect, firePoint.position, Quaternion.identity);
            ChargeEffectScript chargescript = activeChargeEffect.GetComponent<ChargeEffectScript>();
            if (chargescript != null) 
            {
                chargescript.SetLancerScript(this);
            }
        }
    }

    private void FireLaser()
    {
        currentState = AttackState.Firing;
        stateTimer = laserDuration;

        if (laserBeamPrefab != null)
        {
            beam = Instantiate(laserBeamPrefab, firePoint.position, firePoint.rotation);
            LaserScript beamScript = beam.GetComponent<LaserScript>();
            if (beamScript != null)
            {
                beamScript.SetLancerScript(this); 
            }

            Destroy(activeChargeEffect);
            
        }
    }

    private void EndLaser()
    {
        if (beam!= null)
        {
            Destroy(beam);
            beam = null;
        }

        currentState = AttackState.Cooldown;
        stateTimer = laserCooldown;
    }

    private bool isShooting = false;

    public void SetIsShooting(bool state)
    {
        isShooting = state;
    }

    protected override bool CanDescend()
    {
        return !isShooting;
    }

    protected override void Die()
    {
        base.Die();
        Destroy(beam);
        Destroy(activeChargeEffect);
    }
}
