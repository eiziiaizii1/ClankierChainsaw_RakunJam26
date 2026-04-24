using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Hero))]
public class AttackManager : MonoBehaviour
{
    public WeaponController weaponController;
    public Hero hero;

    [Tooltip("List of available actions that the player can perform")]
    public List<WeaponActionData> AvailableActions = new List<WeaponActionData>();

    private void Awake()
    {
        hero = GetComponent<Hero>();
        if (weaponController == null)
        {
            weaponController = GetComponentInChildren<WeaponController>();
        }
    }

    private void Update()
    {
        // Example input logic: 
        // Press 1 for first attack, 2 for second attack
        if (Input.GetKeyDown(KeyCode.Alpha1) && AvailableActions.Count > 0)
        {
            TryPerformAction(AvailableActions[0]);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && AvailableActions.Count > 1)
        {
            TryPerformAction(AvailableActions[1]);
        }
    }

    public void TryPerformAction(WeaponActionData actionData)
    {
        if (actionData != null && weaponController != null && !weaponController.IsAttacking)
        {
            weaponController.PerformAction(actionData, hero.Stats);
        }
    }
}
