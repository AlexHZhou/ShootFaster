using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour {
    [SerializeField]
    string hoverOverSound = "ButtonHover";
    [SerializeField]
    string pressButtonSound = "ButtonPress";

    AudioManager audioManager;
    private void Start()
    {
        audioManager = AudioManager.instance;
        if (audioManager == null) Debug.LogError("No AudioManager found.");
    }

    // Use this for initialization
    public void Play()
    {
        audioManager.PlaySound(pressButtonSound);
        audioManager.StopSound("Intro");
        audioManager.PlaySound("Shop");
        Debug.Log("Starting game...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        //loads the scene after this scene (so should be ingame).
    }

    // Update is called once per frame
    public void Quit()
    {
        audioManager.PlaySound(pressButtonSound);
        Debug.Log("Exiting Game.");
        Application.Quit();
    }

    public void OnMouseOver()
    {
        audioManager.PlaySound(hoverOverSound);
    }
}
