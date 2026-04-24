using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Attack System/Flip Action")]
public class FlipActionData : WeaponActionData
{
    [Tooltip("How far should the sword visually offset during the flip")]
    public float FlipHeightOffset = 1f;

    public override IEnumerator Execute(WeaponController weapon, HeroStats stats)
    {
        float duration = BaseDuration / stats.AttackSpeed;
        float elapsed = 0f;

        // Ensure hitbox is OFF during a flip
        weapon.Hitbox.DisableHitbox();

        // Let's assume we want to flip the angle by 180 degrees
        Quaternion startRotation = weapon.WeaponPivot.localRotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(0, 0, 180f);

        Vector3 originalLocalPos = weapon.WeaponPivot.localPosition;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            
            // Visual arc over the hero (assuming 2D top-down or side-view)
            // It modifies the local position of the pivot or sword during the flip
            float arcHeight = Mathf.Sin(t * Mathf.PI) * FlipHeightOffset;
            weapon.WeaponPivot.localRotation = Quaternion.Lerp(startRotation, endRotation, t);
            
            // Move it up/forward visually and then back down
            weapon.WeaponPivot.localPosition = originalLocalPos + weapon.WeaponPivot.up * arcHeight;
            
            yield return null;
        }

        weapon.WeaponPivot.localRotation = endRotation;
        weapon.WeaponPivot.localPosition = originalLocalPos;
    }
}
