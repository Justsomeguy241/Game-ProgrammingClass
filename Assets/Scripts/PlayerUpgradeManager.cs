using System.Collections;
using UnityEngine;

public class PlayerUpgradeManager : MonoBehaviour
{
    
    public bool hasMultishot = false;
    public bool hasTripshot = false;
    public bool hasQuadshot = false;
    public bool hasPiercing = false;
    public bool hasExplosive = false;
    public bool hasChargeShot = false;
    public bool hasKnockback = false;
    public bool hasChainReaction = false;

    
    public bool hasSecondWind = false;
    public bool hasUsedSecondWind = false;
    public bool hasAdrenalineSurge = false;
    public bool hasMomentumShift = false;

    private PlayerScript playerScript;

    private void Awake()
    {
        playerScript = GetComponent<PlayerScript>();

    }
    public void ApplyTemporaryUpgrade(string upgrade, float duration)
    {
        if (upgrade == "SecondWind")
        {
            ApplyUpgrade(upgrade);
            return; // Skip timer-based removal
        }

        ApplyUpgrade(upgrade); // Activate it normally
        StartCoroutine(RemoveAfterDelay(upgrade, duration));
    }

    private IEnumerator RemoveAfterDelay(string upgrade, float delay)
    {
        yield return new WaitForSeconds(delay);
        RemoveUpgrade(upgrade);
    }

    private void RemoveUpgrade(string upgrade)
    {
        switch (upgrade)
        {
            case "Multishot":
                hasMultishot = false;
                break;
            case "Tripshot":
                hasTripshot = false;
                break;
            case "Quadshot":
                hasQuadshot = false;
                break;
            case "Piercing":
                hasPiercing = false;
                break;
            case "Explosive":
                hasExplosive = false;
                break;
            case "ChargeShot":
                hasChargeShot = false;
                break;
            case "Knockback":
                hasKnockback = false;
                break;
            case "ChainReaction":
                hasChainReaction = false;
                break;
        }

        playerScript.UpdateShipSprite();
    }

    public void ApplyUpgrade(string upgrade)
    {
        switch (upgrade)
        {
            case "Multishot":
                if (hasTripshot || hasQuadshot) return;
                hasMultishot = true;
                playerScript.UpdateShipSprite();
                break;

            case "Tripshot":

                if (hasQuadshot) return;
                hasTripshot = true;
                    hasMultishot = false;
                    playerScript.UpdateShipSprite();
                
                break;

            case "Quadshot":
                
                
                    hasQuadshot = true;
                    hasTripshot = false;
                    playerScript.UpdateShipSprite();
                
                break;

            case "Piercing":
                hasPiercing = true;
                break;

            case "Explosive":
                hasExplosive = true;
                break;

            case "ChargeShot":
                hasChargeShot = true;
                break;

            case "Knockback":
                hasKnockback = true;
                break;

            case "SecondWind":
                hasSecondWind = true;
                break;

            case "AdrenalineSurge":
                hasAdrenalineSurge = true;
                break;

            case "MomentumShift":
                hasMomentumShift = true;
                break;
            case "ChainReaction":
                hasChainReaction = true;
                break;

            default:
                Debug.LogWarning("Unknown upgrade: " + upgrade);
                break;
        }
    }

    public void TryUseSecondWind()
    {
        if (hasSecondWind && !hasUsedSecondWind)
        {
            hasUsedSecondWind = true;
            GetComponent<CharacterBase>()?.TakeDamage(-1);
        }
    }
}
