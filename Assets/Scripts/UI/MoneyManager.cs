using UnityEngine;
using TMPro;

public class MoneyManager : MonoBehaviour
{
    [SerializeField] private int money = 100;
    [SerializeField] private TMP_Text moneyText;

    private void Start() {
        UpdateMoneyText();
    }
    
    public void AddMoney(int amount)
    {
        money += amount;
        UpdateMoneyText();
    }

    public void UpdateMoneyText()
    {
        if (moneyText != null) moneyText.text = money.ToString();
    }

    public bool TrySpendMoney(int amount)
    {
        if (money >= amount)
        {
            money -= amount;
            UpdateMoneyText();
            return true;
        }
        else
        {
            Debug.Log($"Not enough money to spend {amount}");
            return false;
        }
    }
}