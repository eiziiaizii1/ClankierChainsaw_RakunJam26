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

    private void Start()
    {
        if (hero != null && hero.Stats != null && weaponController != null)
        {
            weaponController.UpdateWeaponSizeVisuals(hero.Stats.WeaponSizeLevel);
            hero.Stats.OnWeaponSizeChanged += weaponController.UpdateWeaponSizeVisuals;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            TryPerformAction(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            TryPerformAction(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {   
            TryPerformAction(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            TryPerformAction(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            TryPerformAction(4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            TryPerformAction(5);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            TryPerformAction(6);
        }   
    }
    
    public void TryPerformAction(int attackID)
    {
        if (attackID < 0 || attackID >= AvailableActions.Count || 
            AvailableActions[attackID] == null || 
            weaponController == null || 
            weaponController.IsAttacking) {
                Debug.Log("Invalid attack attempt");
                return;
            }
        
        weaponController.PerformAction(AvailableActions[attackID], hero.Stats);
        if (hero != null)
        {
            hero.PlayTurnSound();
        }
    }
}
