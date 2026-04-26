using System.Collections.Generic;
using UnityEngine;
using AzizStuff;

public class WeaponHitbox : MonoBehaviour
{
    [Tooltip("Damage applied to an enemy that walks into the weapon while no swing is active. 0 disables passive damage.")]
    [SerializeField][Min(0)] int passiveDamage = 1;

    private Collider2D col;
    private bool isActive = false;
    private float currentDamage = 0f;

    // Tracks enemies already hit during the current swing.
    private HashSet<Collider2D> alreadyHitEnemies = new HashSet<Collider2D>();

    private HeroStats heroStats;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        col.isTrigger = true;
        col.enabled = true;
        heroStats = GetComponentInParent<HeroStats>();
    }

    public void EnableHitbox(float damage)
    {
        currentDamage = damage;
        alreadyHitEnemies.Clear();
        isActive = true;
    }

    public void DisableHitbox()
    {
        isActive = false;
        alreadyHitEnemies.Clear();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy")) return;

        var dmg = collision.GetComponentInParent<IDamageable>();
        if (dmg == null) return;

        if (isActive)
        {
            if (alreadyHitEnemies.Contains(collision)) return;
            alreadyHitEnemies.Add(collision);
            dmg.TakeDamage(Mathf.RoundToInt(currentDamage));
        }
        else if (passiveDamage > 0)
        {
            int scaledPassive = passiveDamage;
            if (heroStats != null)
            {
                scaledPassive = Mathf.Max(1, Mathf.RoundToInt(passiveDamage * heroStats.DamageMultiplier));
            }
            dmg.TakeDamage(scaledPassive);
        }
    }
}
