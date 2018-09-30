using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveUIScript : MonoBehaviour {
    [SerializeField]
    UnlimitedSpawnWorks spawner;

    [SerializeField]
    Animator waveAnimator;

    [SerializeField]
    Text waveCountdownText;

    [SerializeField]
    Text waveCountText;

    private UnlimitedSpawnWorks.SpawnState previousState; //WTF YOU CAN DO THIS????

	// Use this for initialization
	void Start () {

        if (spawner == null)
        {
            Debug.LogError("No Spawner referenced.");
            this.enabled = false;
        }
        if (waveAnimator == null)
        {
            Debug.LogError("No waveAnimator referenced.");
            this.enabled = false;

        }
        if (waveCountdownText == null)
        {
            Debug.LogError("No waveCountdownText referenced.");
            this.enabled = false;
        }
        if (waveCountText == null)
        {
            Debug.LogError("No waveCountText referenced.");
            this.enabled = false;
        }

        GameMaster.gm.onToggleStoreMenu += OnStoreMenuToggle;
    }
	
	// Update is called once per frame
	void Update () {
        switch (spawner.State)
        {
            case UnlimitedSpawnWorks.SpawnState.COUNTING:
                UpdateCountingUI();
                break;
            case UnlimitedSpawnWorks.SpawnState.SPAWNING:
                UpdateSpawningUI();
                break;

                
        }
        previousState = spawner.State;
	}

    void UpdateCountingUI()
    {
        if (previousState != UnlimitedSpawnWorks.SpawnState.COUNTING)
        {
            waveAnimator.SetBool("WaveIncoming", false);
            waveAnimator.SetBool("WaveCountdown", true);
        }
        waveCountdownText.text = ((int)spawner.WaveCountdown).ToString() + " seconds to wave"; //double conversions?!?
    }
    void UpdateSpawningUI()
    {
        if (previousState != UnlimitedSpawnWorks.SpawnState.SPAWNING)
        {
            waveAnimator.SetBool("WaveCountdown", false);
            waveAnimator.SetBool("WaveIncoming", true);

            waveCountText.text = spawner.NextWave.ToString();
            
        }
    }

    void OnStoreMenuToggle(bool active)
    {
        //could hide text.
    }
}
