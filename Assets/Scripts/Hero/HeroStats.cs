using UnityEngine;

public class HeroStats : MonoBehaviour
{
    [Header("Damage")]
    public float BaseDamage = 10f;
    public float DamageMultiplier = 1f; 
    public float Damage => BaseDamage * DamageMultiplier;

    [Header("Attack Speed")]
    public float BaseAttackSpeed = 1f;
    public float AttackSpeedMultiplier = 1f;
    public float AttackSpeed => BaseAttackSpeed * AttackSpeedMultiplier;

    [Header("Weapon Size")]
    public float BaseWeaponSize = 1f;
    public float WeaponSizeMultiplier = 1f;
    public float WeaponSize => BaseWeaponSize * WeaponSizeMultiplier;

    public void AddDamageBuff(float percent) => DamageMultiplier += percent;
    public void RemoveDamageBuff(float percent) => DamageMultiplier -= percent;

    public void AddAttackSpeedBuff(float percent) => AttackSpeedMultiplier += percent;
    public void RemoveAttackSpeedBuff(float percent) => AttackSpeedMultiplier -= percent;

    public void AddWeaponSizeBuff(float percent) => WeaponSizeMultiplier += percent;
    public void RemoveWeaponSizeBuff(float percent) => WeaponSizeMultiplier -= percent;
}
