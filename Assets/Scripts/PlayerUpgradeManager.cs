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

    public void ApplyUpgrade(string upgrade)
    {
        switch (upgrade)
        {
            case "Multishot":
                hasMultishot = true;
                playerScript.UpdateShipSprite();
                break;

            case "Tripshot":
                if (hasMultishot)
                {
                    hasTripshot = true;
                    hasMultishot = false;
                    playerScript.UpdateShipSprite();
                }
                break;

            case "Quadshot":
                if (hasTripshot)
                {
                    hasQuadshot = true;
                    hasTripshot = false;
                    playerScript.UpdateShipSprite();
                }
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
