using UnityEngine;

[RequireComponent(typeof(HeroStats))]
public class Hero : MonoBehaviour
{
    private int maxHealth;
    private int currentHealth;

    public HeroStats Stats { get; private set; }

    private void Awake()
    {
        Stats = GetComponent<HeroStats>();
    }

    private void Start()
    {
        maxHealth = 100;
        currentHealth = maxHealth;
    }

    public void GetDamage(float damage)
    {
        currentHealth -= (int)damage;
        CheckHealth();
    }

    private void CheckHealth()
    {
        if (currentHealth <= 0)
        {
            Debug.Log("Hero is dead!");
            Time.timeScale = 0;
        }
    }
}
