using UnityEngine;

// remove it from the player and add this script to wapon to make it apply the damage

namespace AzizStuff
{
    [DisallowMultipleComponent]
    public class DamageDealer : MonoBehaviour
    {
        [Tooltip("Damage applied per contact. Enemies currently one-shot regardless of amount.")]
        [Min(0)][SerializeField] int damage = 1;

        [Tooltip("Respond to trigger contacts (Is Trigger = on).")]
        [SerializeField] bool onTrigger = true;

        [Tooltip("Respond to physical collisions (Is Trigger = off).")]
        [SerializeField] bool onCollision = true;

        void OnTriggerEnter2D(Collider2D other)
        {
            if (onTrigger) Deal(other);
        }

        void OnCollisionEnter2D(Collision2D col)
        {
            if (onCollision) Deal(col.collider);
        }

        void Deal(Collider2D other)
        {
            var target = other.GetComponentInParent<IDamageable>();
            if (target != null) target.TakeDamage(damage);
        }
    }
}
