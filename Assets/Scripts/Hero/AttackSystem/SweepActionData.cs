using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Attack System/Sweep Action")]
public class SweepActionData : WeaponActionData
{
    public float Angle = 90f;
    [Tooltip("-1 for clockwise, 1 for anti-clockwise")]
    public int Direction = -1;
    public float DamageMultiplier = 1f;

    public override IEnumerator Execute(WeaponController weapon, HeroStats stats)
    {
        // Calculate modified duration based on Attack Speed
        float duration = BaseDuration / stats.AttackSpeed.Value;
        float elapsed = 0f;

        Quaternion startRotation = weapon.WeaponPivot.localRotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(0, 0, Angle * Direction);

        // Turn ON the hitbox to deal damage
        weapon.Hitbox.EnableHitbox(stats.Damage.Value * DamageMultiplier);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            weapon.WeaponPivot.localRotation = Quaternion.Lerp(startRotation, endRotation, t);
            yield return null;
        }

        weapon.WeaponPivot.localRotation = endRotation;
        weapon.Hitbox.DisableHitbox();
    }
}
