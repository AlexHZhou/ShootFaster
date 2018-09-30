    
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

[RequireComponent(typeof (Platformer2DUserControl))]
public class Player : MonoBehaviour
{
    public int fallBoundary = -20;

    public string deathSound = "PlayerDeathSound";
    public string damagedSound = "PlayerDamagedSound";

    [SerializeField]
    private StatusIndicator statusIndicator;

    private AudioManager audioManager;

    private PlayerStats stats;

    public int Score;

    void Start()
    {
        stats = PlayerStats.instance;
        stats.curHealth = stats.maxHealth;

        if (statusIndicator == null) Debug.LogError("No status indicator reference on player.");
        else statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);

        audioManager = AudioManager.instance;
        if (audioManager == null) Debug.Log("No AudioManager in scene.");

        GameMaster.gm.onToggleStoreMenu += OnStoreMenuToggle;
        //makes player modifyable from the gm toggle store method.

        InvokeRepeating("RegenHealth", 1f/stats.healthRegenRate, stats.healthRegenRate);
    }

    void Update()
    {
        if (transform.position.y <= fallBoundary)
        {
            DamagePlayer(100);
        }
    }

    void RegenHealth()
    {
        stats.curHealth += stats.healthRegenAmount;
        statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
    }

    public void DamagePlayer(float damage)
    {
        stats.curHealth -= damage;
        if (stats.curHealth <= 0)
        {
            audioManager.PlaySound(deathSound);
            GameMaster.KillPlayer(this);
        }
        else audioManager.PlaySound(damagedSound);
        //damaged sound currently not in game.
        statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
    }

    void OnStoreMenuToggle(bool active)
    {
        //handles what happens when upgrade menu is toggled
        GetComponent<Platformer2DUserControl>().enabled = !active;
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        Weapon _weapon = GetComponentInChildren<Weapon>();
        if (_weapon != null) _weapon.enabled = !active;
    }

    private void OnDestroy()
    {
        GameMaster.gm.onToggleStoreMenu -= OnStoreMenuToggle;
    }
}
