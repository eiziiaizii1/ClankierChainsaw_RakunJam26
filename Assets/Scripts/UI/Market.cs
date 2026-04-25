using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Market : MonoBehaviour
{

    [Header("Tier1")]
    [SerializeField] private TMP_Text Tier1CostText;
    [SerializeField] private List<UpgradeData> Tier1Upgrades = new List<UpgradeData>();

    [Header("Tier2")]
    [SerializeField] private TMP_Text Tier2CostText;
    [SerializeField] private List<UpgradeData> Tier2Upgrades = new List<UpgradeData>();

    [Header("Tier3")]
    [SerializeField] private TMP_Text Tier3CostText;
    [SerializeField] private List<UpgradeData> Tier3Upgrades = new List<UpgradeData>();

    [Header("References")]
    [SerializeField] private MoneyManager moneyManager;
    [SerializeField] private Rullete rullete;
    [SerializeField] private Animator marketAnimator;
    [SerializeField] private Animator rulleteAnimator;

    [Header("Costs")]
    public int[] TierCosts = {10, 20, 30};
    [SerializeField] private float costMultiplier = 1.3f;

    [Header("Tier Colors")]
    [SerializeField] private Color tier1Color = Color.white;
    [SerializeField] private Color tier2Color = Color.yellow;
    [SerializeField] private Color tier3Color = new Color(1f, 0.5f, 0f); // Orange

    public void BuyUpgrade(int tier)
    {
        if (tier < 1 || tier > 3) return;

        int cost = TierCosts[tier - 1];
        
        if (moneyManager != null && moneyManager.TrySpendMoney(cost))
        {
            TierCosts[tier - 1] = Mathf.RoundToInt(cost * costMultiplier);
            
            List<UpgradeData> sourceList = null;
            Color tierColor = Color.white;

            switch (tier)
            {
                case 1: sourceList = Tier1Upgrades; tierColor = tier1Color; break;
                case 2: sourceList = Tier2Upgrades; tierColor = tier2Color; break;
                case 3: sourceList = Tier3Upgrades; tierColor = tier3Color; break;
            }

            if (sourceList == null || sourceList.Count == 0)
            {
                Debug.LogWarning("No upgrades available for this tier!");
                return;
            }

            UpgradeData[] selectedOptions = new UpgradeData[3];
            List<UpgradeData> tempSource = new List<UpgradeData>(sourceList);

            for (int i = 0; i < 3; i++)
            {
                if (tempSource.Count > 0)
                {
                    int randomIndex = Random.Range(0, tempSource.Count);
                    selectedOptions[i] = tempSource[randomIndex];
                    // Remove to ensure unique options if possible
                    tempSource.RemoveAt(randomIndex);
                }
                else
                {
                    // Fallback if there are less than 3 total upgrades in the list
                    selectedOptions[i] = sourceList[Random.Range(0, sourceList.Count)];
                }
            }

            if (rullete != null)
            {
                rullete.StartRullete(tierColor, selectedOptions);
                rulleteAnimator.SetTrigger("StartRullete");
                marketAnimator.SetTrigger("StartRullete");
            }
        }
    }
}
