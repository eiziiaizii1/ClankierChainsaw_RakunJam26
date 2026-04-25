using System.Collections.Generic;
using UnityEngine;
using AzizStuff;

[RequireComponent(typeof(Collider2D))]
public class WeaponHitbox : MonoBehaviour
{
    private Collider2D col;
    private bool isActive = false;
    private float currentDamage = 0f;

    // Keep track of enemies already hit during a single swing
    private HashSet<Collider2D> alreadyHitEnemies = new HashSet<Collider2D>();

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        col.isTrigger = true;
        col.enabled = false;
    }

    public void EnableHitbox(float damage)
    {
        currentDamage = damage;
        alreadyHitEnemies.Clear();
        isActive = true;
        col.enabled = true;
    }

    public void DisableHitbox()
    {
        isActive = false;
        col.enabled = false;
        alreadyHitEnemies.Clear();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isActive) return;

        // Check if we hit an enemy (you can check tag, layer, or component)
        if (collision.CompareTag("Enemy"))
        {
            if (!alreadyHitEnemies.Contains(collision))
            {
                alreadyHitEnemies.Add(collision);

                var dmg = collision.GetComponentInParent<IDamageable>();
                if (dmg != null) dmg.TakeDamage(Mathf.RoundToInt(currentDamage));
            }
        }
    }
}
