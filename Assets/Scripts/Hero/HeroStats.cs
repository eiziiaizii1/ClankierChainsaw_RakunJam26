using UnityEngine;

public class HeroStats : MonoBehaviour
{
    public CharacterStat Damage;
    public CharacterStat AttackSpeed;

    private void Awake()
    {
        Damage = new CharacterStat(10f);
        AttackSpeed = new CharacterStat(1f); // 1.0 is default 100% speed
    }
}
