using UnityEngine;

public enum StatType
{
    Damage,
    AttackSpeed,
    WeaponSize
}

[CreateAssetMenu(menuName = "Upgrades/Stat Upgrade")]
public class StatUpgradeData : UpgradeData
{
    [Header("Stat Modifier")]
    public StatType StatToUpgrade;
    
    [Tooltip("The percentage to increase. E.g., 0.1 for a 10% increase.")]
    public float PercentIncrease = 0.1f;

    public override void Apply(Hero hero)
    {
        switch (StatToUpgrade)
        {
            case StatType.Damage:
                hero.Stats.AddDamageBuff(PercentIncrease);
                break;
            case StatType.AttackSpeed:
                hero.Stats.AddAttackSpeedBuff(PercentIncrease);
                break;
            case StatType.WeaponSize:
                hero.Stats.AddWeaponSizeBuff(PercentIncrease);
                break;
        }
        
        Debug.Log($"Applied Upgrade: {UpgradeName} (Tier {Tier}) - Increased {StatToUpgrade} by {PercentIncrease * 100}%");
    }
}
