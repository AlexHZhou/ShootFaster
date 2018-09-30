using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(EnemyAI))]
public class Enemy : MonoBehaviour {

    [System.Serializable]
    public class EnemyStats
    {
        
        public float maxHealth = 12;
        private float _curHealth;
        public float damage = 20;

        public float curHealth
        {
            get { return _curHealth; }
            set { _curHealth = Mathf.Clamp(value, 0, maxHealth); } //ensures that changes keep hp in acceptable ranges
        }

        public void Init()
        {
            curHealth = maxHealth;
        }
    }

    EnemyStats enemyStats = new EnemyStats();

    public Transform deathParticles;
    public float shakeAmt = 0.1f;
    public float shakeLength = 0.1f;

    [Header("Optional: ")]
    [SerializeField]
    private StatusIndicator statusIndicator;
    
    [SerializeField]
    public Sprite[] damageSkins;
    private int skinIndex = 0;

    public string deathSound = "Explosion";

    public float moneyDrop = 2f;


    void Start()
    {
        enemyStats.Init();
        if (statusIndicator != null)
        {
            statusIndicator.SetHealth(enemyStats.curHealth, enemyStats.maxHealth);
        }
        if (deathParticles == null) Debug.LogWarning("No death particles referenced in enemy.");

        GameMaster.gm.onToggleStoreMenu += OnStoreMenuToggle;
    }

    public void DamageEnemy(float damage)
    {

        enemyStats.curHealth -= damage; //not _curHealth?


        float hpPercent = enemyStats.curHealth / enemyStats.maxHealth;
        if (enemyStats.curHealth <= 0) GameMaster.KillEnemy(this);
        else if (hpPercent <= .30)
        {
            skinIndex = 2;
        }
        else if (hpPercent <= .70)
        {
            skinIndex = 1;
        }
        else skinIndex = 0;
        
        GetComponent<SpriteRenderer>().sprite = damageSkins[skinIndex];

        if (statusIndicator != null)
        {
            statusIndicator.SetHealth(enemyStats.curHealth, enemyStats.maxHealth);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player _player = collision.collider.GetComponent<Player>();
        if (_player != null)
        {
            _player.DamagePlayer(enemyStats.damage);
            DamageEnemy(100);
        }
        Cow _cow = collision.collider.GetComponent<Cow>();
        if (_cow != null)
        {
            _cow.DamageCow(enemyStats.damage);
            DamageEnemy(100);
        }

    }

    private void OnDestroy()
    {
        GameMaster.gm.onToggleStoreMenu -= OnStoreMenuToggle;
    }
 

    void OnStoreMenuToggle(bool active)
    {
        //handles what happens when upgrade menu is toggled
        GetComponent<EnemyAI>().enabled = !active;
        GetComponent<Rigidbody2D>().velocity= Vector3.zero;
    }
}

