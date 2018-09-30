using UnityEngine;
using UnityEngine.UI;

public class GunShop : MonoBehaviour {
    [SerializeField] private Text damageText;
    [SerializeField] private Text buttonDamageUpgrade;
    int damageUpgrade = 1;
    float damageUpgradeCost = 25f;

    [SerializeField] private Text fireRateText;
    [SerializeField] private Text buttonFireRateUpgrade;
    float fireRateUpgrade = 1.5f;
    float fireRateUpgradeCost = 30f;
    
    private Weapon gun;
    
    public string buySound;
    public string notEnoughMoneySound;


    private void OnEnable()
    {
        gun = FindObjectOfType<Weapon>();
        UpdateValues();
    }

    void UpdateValues()
    {
        damageText.text = "DAMAGE: " + gun.Damage.ToString();
        buttonDamageUpgrade.text = "UPGRADE" + "\n" + "(" + damageUpgradeCost + ")";

        fireRateText.text = "FIRE RATE: " + gun.fireRate.ToString();
        buttonFireRateUpgrade.text = "UPGRADE" + "\n" + "(" + fireRateUpgradeCost + ")";
    }

    public void UpgradeDMG()
    {
        if (GameMaster.Money < damageUpgradeCost)
        {
            AudioManager.instance.PlaySound(notEnoughMoneySound);
            return;
        }
        GameMaster.Money -= damageUpgradeCost;
        damageUpgradeCost += 20;
        gun.Damage += damageUpgrade;
        UpdateValues();
        AudioManager.instance.PlaySound(buySound);
    }
    public void UpgradeFR()
    {
        if (GameMaster.Money < fireRateUpgradeCost)
        {
            AudioManager.instance.PlaySound(notEnoughMoneySound);
            return;
        }
        GameMaster.Money -= fireRateUpgradeCost;
        fireRateUpgradeCost += 20;
        gun.fireRate += fireRateUpgrade;
        UpdateValues();
        AudioManager.instance.PlaySound(buySound);
    }
}
