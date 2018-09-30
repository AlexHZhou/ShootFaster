using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour {
    public static GameMaster gm;
    [SerializeField]
    private int maxLives = 3;
    private static int _remainingLives;
    public static int RemainingLives
    {
        get { return _remainingLives; }
        set { _remainingLives = value; }
    }

    [SerializeField]
    private float startingMoney;
    public static float Money;


    public void Awake()
    {
        if (gm == null)
        {
            gm = GameObject.FindWithTag("GM").GetComponent<GameMaster>();
        }
       
    }
    
    public Player player;
    public Transform playerPrefab;
    public Transform enemyPrefab; 
    public Transform spawnPrefab;
    public Transform spawnPoint;
    //public AudioClip respawnAudio;
    public float spawnDelay;
    public string respawnCoundDownSoundName = "RespawnCountdown";
    public string spawnSound = "SpawnSound";

    public CameraShake camShake;

    [SerializeField]
    private GameObject gameOverUI;

    private AudioManager audioManager;
    private string gameOverSound = "GameOver";

    [SerializeField]
    private GameObject[] storeMenus;
    bool storeOpen = false;

    [SerializeField]
    private WaveSpawner waveSpawner;

    public Transform[] doors;
    private float doorRadius = 4f;
    
    //'delegate' calls a bunch of functions in other scripts.
    public delegate void StoreMenuCallback(bool active);
    public StoreMenuCallback onToggleStoreMenu;

    

    private void Start()
    {
        if (camShake == null) Debug.LogWarning("No camerashake in gm.");

        _remainingLives = maxLives;
        Money = startingMoney;

        audioManager = AudioManager.instance; //to access the static class of audiomanager
        if (audioManager == null) Debug.Log("No audio manager found!");
        else
        {
            audioManager.StopAll();
            audioManager.PlaySound("Battle");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ToggleStoreMenu();
        }
        
        
    }

    private void ToggleStoreMenu()
    {
        Debug.Log("Attempting to toggle store menu");
        for (int i = 0; i < doors.Length; i++)
        {
            Debug.Log("Trying Door: " + i);

            if (!storeOpen) //entering store
            {
                Debug.Log("Entering Store");

                int doorNumber = DoorCheckPlayer(doors[i].position, doorRadius);
                if (doorNumber != -1)
                {
                    Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
                    playerRb.bodyType = RigidbodyType2D.Kinematic;

                    storeMenus[i].SetActive(!storeMenus[i].activeSelf);
                    waveSpawner.enabled = !storeMenus[i].activeSelf;
                    onToggleStoreMenu.Invoke(storeMenus[i].activeSelf);

                    audioManager.StopAll();
                    audioManager.PlaySound("ShopMusic");
                    Debug.Log("Music changing from Battle to Shop");

                    storeOpen = !storeOpen;
                    return;
                }
                else Debug.Log("Door didn't detect jack sht.");
            }
            else //leaving store
            {
                Debug.Log("Leaving Store");

                Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
                playerRb.bodyType = RigidbodyType2D.Dynamic;

                storeMenus[i].SetActive(!storeMenus[i].activeSelf);
                waveSpawner.enabled = !storeMenus[i].activeSelf;
                onToggleStoreMenu.Invoke(storeMenus[i].activeSelf);

                audioManager.StopSound("ShopMusic");
                audioManager.PlaySound("Battle");
                Debug.Log("Music changing back to Battle");

                storeOpen = !storeOpen;
            }
            
        }

    }

    private int DoorCheckPlayer(Vector3 location, float radius)
    {
        Collider2D[] objectsInRange = Physics2D.OverlapCircleAll(location, radius);
        int i = 0;
        foreach (Collider2D col in objectsInRange)
        {
            Player playerCheck = col.GetComponent<Player>();
            if (playerCheck != null)
            {
                return i;
            }
            i++;
        }
        return -1;
    }

    public void EndGame()
    {
        audioManager.StopAll();
        audioManager.PlaySound(gameOverSound);
       
        Debug.Log("Game over.");
        waveSpawner.enabled = false;
        
        gameOverUI.SetActive(true);
    }

    public IEnumerator RespawnPlayer()  
    {
        audioManager.PlaySound(respawnCoundDownSoundName);
        yield return new WaitForSeconds(spawnDelay); // makes method type NOT void but IEnumerator

        audioManager.PlaySound(spawnSound);
        Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        GameObject spawnClone = (GameObject) Instantiate(spawnPrefab, spawnPoint.position, spawnPoint.rotation).gameObject;
        Destroy(spawnClone, 3f);
        
    }

    public static void KillPlayer(Player player)//just parsing Transform player would delete image, not other parts.
    {
        Destroy(player.gameObject);
        _remainingLives--;
        if (_remainingLives == 0) { gm.EndGame(); }//end game

        else { gm.StartCoroutine(gm.RespawnPlayer()); }
        //need theis "coroutine" for IEnumerator stuff.
    }

    //public IEnumerator RespawnEnemy()
    //{
    //    AudioSource.PlayClipAtPoint(respawnAudio, new Vector3(spawnPoint.position.x, spawnPoint.position.y, spawnPoint.position.z));
    
    //    yield return new WaitForSeconds(spawnDelay); // makes method type NOT void but IEnumerator

    //    Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
    //    GameObject spawnClone = (GameObject)Instantiate(spawnPrefab, spawnPoint.position, spawnPoint.rotation).gameObject;
    //    Destroy(spawnClone, 3f);
    //}
    public static void KillEnemy(Enemy enemy)//just parsing "Transform" would delete image, not other parts.
    {
        gm._KillEnemy(enemy);
        //gm.StartCoroutine(gm.RespawnEnemy());
        //need theis "coroutine" for IEnumerator stuff.
    }
    public void _KillEnemy(Enemy _enemy)
    {
        Money += _enemy.moneyDrop;
        audioManager.PlaySound("Buy");

        Transform deathParticles = Instantiate(_enemy.deathParticles, _enemy.transform.position, _enemy.transform.rotation); //quaternion.identity = 0 rotation
        camShake.Shake(_enemy.shakeAmt, _enemy.shakeLength);

        Destroy(_enemy.gameObject);
        Destroy(deathParticles.gameObject, 5f);

        audioManager.PlaySound(_enemy.deathSound);
        player.Score += 10;
    }


    public static void KillCow(Cow cow)//just parsing "Transform" would delete image, not other parts.
    {
        gm._KillCow(cow);
        //gm.StartCoroutine(gm.RespawnEnemy());
        //need theis "coroutine" for IEnumerator stuff.
    }
    public void _KillCow(Cow _cow)
    {
        audioManager.PlaySound("Buy");

        Transform deathParticles = Instantiate(_cow.deathParticles, _cow.transform.position, _cow.transform.rotation); //quaternion.identity = 0 rotation
        camShake.Shake(_cow.shakeAmt, _cow.shakeLength);

        Destroy(_cow.gameObject);
        Destroy(deathParticles.gameObject, 5f);

        audioManager.PlaySound(_cow.deathSound);
        player.Score -= 50;
    }
}
