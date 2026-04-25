using UnityEngine;
using TMPro;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance { get; private set; }

    [SerializeField] private int money = 100;
    [SerializeField] private TMP_Text moneyText;

    private void Awake()
    {
        Instance = this;
    }

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