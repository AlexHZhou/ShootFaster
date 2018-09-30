using UnityEngine;
using UnityEngine.UI;

public class HospitalStore : MonoBehaviour {


    [SerializeField] private Text healthText;
    [SerializeField] private Text regenText;
    [SerializeField] private Text healText;
    [SerializeField] private Text lifeText;
    [SerializeField] private float regenUpgrade = 1f;
    float regenCost = 20f;
    [SerializeField] private float healthUpgrade = 10f;
    float hpCost = 10f;
    private PlayerStats stats;

    [SerializeField] private Text hpCostText;
    [SerializeField] private Text regenCostText;

    public string buySound;
    public string notEnoughMoneySound;

    private void OnEnable()
    {
        stats = PlayerStats.instance;
        UpdateValues();
    }

    void UpdateValues()
    {
        healthText.text = "MAX HP: " + stats.maxHealth.ToString();
        hpCostText.text = "UPGRADE" + "\n" + "(" + hpCost + ")";
        regenText.text = "HP REGEN: " + stats.healthRegenAmount.ToString();
        regenCostText.text = "UPGRADE" + "\n" + "(" + regenCost + ")";
        lifeText.text = "Buy a life: ";
    }

    public void UpgradeHealth()
    {
        if (GameMaster.Money < hpCost)
        {
            AudioManager.instance.PlaySound(notEnoughMoneySound);
            return;
        }
        GameMaster.Money -= hpCost;
        hpCost += 10;
        stats.maxHealth += healthUpgrade;
        UpdateValues();
        AudioManager.instance.PlaySound(buySound);
    }
    public void UpgradeRegen()
    {
        if (GameMaster.Money < regenCost)
        {
            AudioManager.instance.PlaySound(notEnoughMoneySound);
            return;
        }
        GameMaster.Money -= regenCost;
        regenCost += 30f;
        stats.healthRegenAmount += regenUpgrade;
        UpdateValues();
        AudioManager.instance.PlaySound(buySound);
    }

    public void Heal()
    {
        if (GameMaster.Money < 20)
        {
            AudioManager.instance.PlaySound(notEnoughMoneySound);
            return;
        }
        GameMaster.Money -= 20;
        stats.curHealth = stats.maxHealth;
        UpdateValues();
        AudioManager.instance.PlaySound(buySound);
    }

    public void BuyLife()
    {
        if (GameMaster.Money < 100)
        {
            AudioManager.instance.PlaySound(notEnoughMoneySound);
            return;
        }
        GameMaster.Money -= 100;
        GameMaster.RemainingLives++;
        UpdateValues();
        AudioManager.instance.PlaySound(buySound);
    }
}
