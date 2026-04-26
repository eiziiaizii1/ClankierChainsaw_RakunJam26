using UnityEngine;
using AzizStuff;

public class CheatManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Hero hero;

    [Header("Cheat Settings")]
    [SerializeField] private int healAmount = 0000;
    [SerializeField] private int moneyAmount = 200;

    [Header("Time Cheats")]
    [SerializeField] private float slowTimeScale = 0.2f;
    [SerializeField] private float fastTimeScale = 3.0f;

    private void Update()
    {
        // Optional keyboard shortcuts for cheats
        if (Input.GetKeyDown(KeyCode.H))
        {
            HealCheat();
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            MoneyCheat();
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInvincibility();
        }
        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            SlowTime();
        }
        if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            FastTime();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            FreezeTime();
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            NormalTime();
        }
    }

    public void HealCheat()
    {
        if (hero != null)
        {
            hero.Heal(healAmount);
            Debug.Log($"[Cheat] Healed hero for {healAmount}");
        }
        else
        {
            Debug.LogWarning("[Cheat] Hero reference not assigned in CheatManager!");
        }
    }

    public void MoneyCheat()
    {
        if (MoneyManager.Instance != null)
        {
            MoneyManager.Instance.AddMoney(moneyAmount);
            Debug.Log($"[Cheat] Added {moneyAmount} money");
        }
        else
        {
            Debug.LogWarning("[Cheat] MoneyManager.Instance is null!");
        }
    }

    public void ToggleInvincibility()
    {
        if (hero != null)
        {
            hero.IsInvincible = !hero.IsInvincible;
            Debug.Log($"[Cheat] Invincibility is now: {hero.IsInvincible}");
        }
        else
        {
            Debug.LogWarning("[Cheat] Hero reference not assigned in CheatManager!");
        }
    }

    private void SetTimeScale(float scale)
    {
        Time.timeScale = scale;
        if (scale > 0)
        {
            Time.fixedDeltaTime = 0.02f * scale; // Keep physics smooth
        }
    }

    public void SlowTime()
    {
        SetTimeScale(slowTimeScale);
        Debug.Log($"[Cheat] Time Slowed: {Time.timeScale}");
    }

    public void FastTime()
    {
        SetTimeScale(fastTimeScale);
        Debug.Log($"[Cheat] Time Fast: {Time.timeScale}");
    }

    public void FreezeTime()
    {
        SetTimeScale(0f);
        Debug.Log("[Cheat] Time Frozen");
    }

    public void NormalTime()
    {
        SetTimeScale(1f);
        Debug.Log("[Cheat] Time Normal");
    }
}
