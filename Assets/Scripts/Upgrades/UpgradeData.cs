using UnityEngine;

public abstract class UpgradeData : ScriptableObject
{
    [Header("Upgrade Info")]
    public string UpgradeName;
    [TextArea]
    public string Description;
    
    [Tooltip("Tier 1, 2, or 3")]
    [Range(1, 3)]
    public int Tier = 1;

    // This is the polymorphic method. Every specific upgrade will define how it applies itself.
    public abstract void Apply(Hero hero);
}
