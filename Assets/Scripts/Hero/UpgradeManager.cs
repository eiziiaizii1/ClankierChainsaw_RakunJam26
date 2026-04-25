using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Hero))]
public class UpgradeManager : MonoBehaviour
{
    public Hero Hero { get; private set; }

    private void Awake()
    {
        Hero = GetComponent<Hero>();
    }

    public void AddUpgrade(UpgradeData upgrade)
    {
        if (upgrade == null) return;

        upgrade.Apply(Hero);
    }
}
