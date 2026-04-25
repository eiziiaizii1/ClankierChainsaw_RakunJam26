using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Rullete : MonoBehaviour
{
    [SerializeField] private Image[] optionsBackGround;
    [SerializeField] private TMP_Text[] optionsText;
    [SerializeField] private UpgradeManager upgradeManager;
    [SerializeField] private Animator rulleteAnimator;
    [SerializeField] private Animator marketAnimator;

    private UpgradeData[] currentOptions;
    
    public void StartRullete(Color backgroundcolor, UpgradeData[] options)
    {
        currentOptions = options;

        foreach (var n in optionsBackGround) {
            n.color = backgroundcolor;
        }
        for (int i = 0; i < options.Length; i++) {
            if (optionsText[i] != null && options[i] != null)
            {
                optionsText[i].text = options[i].Description;
            }
        }
    }
    
    public void SelectOption(int index)
    {
        if (currentOptions == null || index < 0 || index >= currentOptions.Length) return;

        UpgradeData selectedUpgrade = currentOptions[index];
        
        if (selectedUpgrade != null && upgradeManager != null)
        {
            upgradeManager.AddUpgrade(selectedUpgrade);
            rulleteAnimator.SetTrigger("StopRullete");
            marketAnimator.SetTrigger("StopRullete");
        }
    }
}