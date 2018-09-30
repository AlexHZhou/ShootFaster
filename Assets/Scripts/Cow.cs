using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cow : MonoBehaviour {

    [System.Serializable]
    public class CowStats
    {

        public float maxHealth = 12;
        private float _curHealth;
        public float damage = 4;

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

    CowStats enemyStats = new CowStats();

    public Transform deathParticles;
    public float shakeAmt = 0.1f;
    public float shakeLength = 0.1f;

    [Header("Optional: ")]
    [SerializeField]
    private StatusIndicator statusIndicator;
    
    public string deathSound = "";


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

    public void DamageCow(float damage)
    {

        enemyStats.curHealth -= damage; //not _curHealth?
        if (enemyStats.curHealth <= 0)
        {
            GameMaster.KillCow(this);
        }

        if (statusIndicator != null)
        {
            statusIndicator.SetHealth(enemyStats.curHealth, enemyStats.maxHealth);
        }
    }


    private void OnDestroy()
    {
        GameMaster.gm.onToggleStoreMenu -= OnStoreMenuToggle;
    }


    void OnStoreMenuToggle(bool active)
    {
        ////handles what happens when upgrade menu is toggled
        //try
        //{
        //    GetComponent<EnemyAI>().enabled = !active;
        //}
        //catch Debug.Log("No enemies found to freeze in shop.");
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
    }
}
