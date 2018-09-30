using UnityEngine;

public class PlayerStats : MonoBehaviour {

    public static PlayerStats instance;

    public float maxHealth = 10;
    private float _curHealth;
    public float curHealth
    {
        get { return _curHealth; }
        set { _curHealth = Mathf.Clamp(value, 0, maxHealth); }
    }
    public float healthRegenRate = 1;
    public float healthRegenAmount= 1;

    public float moveSpeed = 10f;
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
   
}
