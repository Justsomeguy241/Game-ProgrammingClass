using UnityEngine;

public class HealthBarUI : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    public Sprite fullHPBarSprite;
    public Sprite twoHPBarSprite;
    public Sprite oneHPBarSprite;
    public Sprite emptyBarSprite;

    public void UpdateHealthBar(int currentHP)
    {
        switch (currentHP)
        {
            case 3:
                spriteRenderer.sprite = fullHPBarSprite;
                break;
            case 2:
                spriteRenderer.sprite = twoHPBarSprite;
                break;
            case 1:
                spriteRenderer.sprite = oneHPBarSprite;
                break;
            case 0:
            default:
                spriteRenderer.sprite = emptyBarSprite;
                break;
        }
    }
}
