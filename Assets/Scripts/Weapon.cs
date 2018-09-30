using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
    public float fireRate = 0;
    public int Damage = 10;
    public LayerMask whatToHit;

    public int magazineSize = 6;
    public float reloadSpeed = 2f;

    public bool isAutomatic = false;

    public Transform bulletTrailPrefab;
    public float timeToSpawnEffect = 0;
    public float effectSpawnRate = 10;

    public Transform muzzleFlashPrefab;
    public Transform hitPrefab;

    public float camShakeAmt = 0.1f;
    public float cameShakeLength = 0.1f;
    CameraShake camShake;

    private float timeToFire = 0;
    private Transform firePoint;


    public string shootSound = "DefaultGunShot";
    private AudioManager audioManager;

    void Awake()
    {
        firePoint = transform.Find("FirePoint");
        if (firePoint == null) Debug.LogError("Hmm, FirePoint not found."); 

    }

	// Use this for initialization
	void Start () {
        camShake = GameMaster.gm.GetComponent<CameraShake>();
        if (camShake == null) Debug.LogError("No camera shake script found on GM object.");

        audioManager = AudioManager.instance;
        if (audioManager == null) Debug.Log("No audioManager found.");
	}

    private bool holdFiring = false;
    bool firing = true;
    bool shouldFire = false;

    void Update () {
    
        firing = Input.GetButton("Fire1");
        
        if (isAutomatic && firing) shouldFire = true;
        else if (!isAutomatic)
        {
            if (firing != holdFiring) shouldFire = true; ;
        }

        if (shouldFire && Time.time > timeToFire) //note how user holding down button checks with GetButton not GetButtonDown
        {
            timeToFire = Time.time + 1 / fireRate; //sets the next time to fire.
            Shoot();
        }

        holdFiring = firing;
        shouldFire = false;
    }

    void Shoot()
    {
        Vector3 MouseLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePosition = new Vector2(MouseLocation.x, MouseLocation.y);
        Vector2 firePointPosition = new Vector2(firePoint.position.x, firePoint.position.y);
        RaycastHit2D hit = Physics2D.Raycast(firePointPosition, mousePosition - firePointPosition, 100, whatToHit);


        Debug.DrawLine(firePointPosition, (mousePosition - firePointPosition) * 100, Color.cyan);
        //for actual hitting
        if (hit.collider != null)
        {
            Debug.DrawLine(firePointPosition, hit.point, Color.red);
          

            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                
                enemy.DamageEnemy(Damage);
            }
        }

        //for effects
        //TODO: BULLETS CANNOT PASS THRU BUILDINGS
        if (Time.time > timeToSpawnEffect)
        {
            Vector3 hitPos;
            Vector3 hitNormal;
            if (hit.collider == null) //if it doesnt hit something, just draw a line
            {
                hitPos = (mousePosition - firePointPosition) * 30;
                hitNormal = new Vector3(999, 999, 999);
            }
            else //if it does hit something, it the line only goes as far as the hit locaation
            {
                hitPos = hit.point;
                hitNormal = hit.normal;
            }

            Effect(hitPos, hitNormal);
            timeToSpawnEffect = Time.time + 1 / effectSpawnRate;
        }
    }

    void Effect(Vector3 hitPos, Vector3 hitNormal)
    {
        Transform trail = (Transform) Instantiate(bulletTrailPrefab, firePoint.position, firePoint.rotation);
        LineRenderer lr = trail.GetComponent<LineRenderer>();
        if (lr != null)
        {
            lr.SetPosition(0, firePoint.position);
            lr.SetPosition(1, hitPos);
        }

        if (hitNormal != new Vector3(9999, 9999, 9999)) //aka hit.
        {
            Transform hitEffectClone = (Transform) Instantiate(hitPrefab, hitPos, Quaternion.FromToRotation(Vector3.right, hitNormal));
            Destroy(hitEffectClone.gameObject, 0.5f);
        }

        Transform flashClone = (Transform) Instantiate(muzzleFlashPrefab, firePoint.position, firePoint.rotation);
        flashClone.parent= firePoint;
        float size = Random.Range(0.6f, 0.9f);
        flashClone.localScale = new Vector3(size, size, 0); //Z doesn't matter in 2D
        Destroy(flashClone.gameObject, 0.04f);
        Destroy(trail.gameObject, 0.04f);

        //shake cam
        camShake.Shake(camShakeAmt, cameShakeLength);

        audioManager.PlaySound(shootSound); 
    }

}
