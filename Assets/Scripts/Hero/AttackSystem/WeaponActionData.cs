using System.Collections;
using UnityEngine;

public abstract class WeaponActionData : ScriptableObject
{
    public float BaseDuration = 0.5f;

    // The execution logic for the specific action type
    public abstract IEnumerator Execute(WeaponController weapon, HeroStats stats);
}
