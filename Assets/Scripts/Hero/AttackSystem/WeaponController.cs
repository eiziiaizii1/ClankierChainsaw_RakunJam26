using System.Collections;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Tooltip("The transform that acts as the center of rotation (e.g. Hero center)")]
    public Transform WeaponPivot;
    public WeaponHitbox Hitbox;

    public bool IsAttacking { get; private set; }

    private void Awake()
    {
        if (WeaponPivot == null) WeaponPivot = transform;
        if (Hitbox == null) Hitbox = GetComponentInChildren<WeaponHitbox>();
    }

    public void PerformAction(WeaponActionData action, HeroStats stats)
    {
        if (IsAttacking) return;
        StartCoroutine(ActionCoroutine(action, stats));
    }

    public void UpdateWeaponSizeVisuals(int level)
    {
        if (WeaponPivot != null)
        {
            Animator animator = WeaponPivot.GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetInteger("saw size", level);
            }
        }
    }

    private IEnumerator ActionCoroutine(WeaponActionData action, HeroStats stats)
    {
        IsAttacking = true;
        
        
        // Execute the custom logic inside the ScriptableObject
        yield return StartCoroutine(action.Execute(this, stats));
        
        IsAttacking = false;
    }
}
