using UnityEngine;

[CreateAssetMenu(menuName = "Powerups/Shield")]
public class ShieldBuff : PowerupEffectBase
{
    public int HitAmount;

    public override void ApplyEffect(GameObject gameObject)
    {
        gameObject.GetComponentInChildren<Shield>().ApplyShield(HitAmount);
    }
}
