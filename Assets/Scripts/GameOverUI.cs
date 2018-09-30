using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour {
    AudioManager audioManager;

    [SerializeField]
    string mouseHoverSound = "ButtonHover";
    [SerializeField]
    string mousePressedSound = "ButtonPress";

    private void Start()
    {
        audioManager = AudioManager.instance;
        if (audioManager == null) Debug.LogError("AudioManager missing.");
    }

    public void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Q)) Quit();
        if (Input.GetKeyDown(KeyCode.R)) Retry();
    }

    public void Quit()
    {
        audioManager.PlaySound(mousePressedSound);
        Debug.Log("Application Quit.");
        Application.Quit();
    }

    public void Retry()
    {
        Debug.Log("Restarting.");
        audioManager.PlaySound(mousePressedSound);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnMouseOver()
    {
        audioManager.PlaySound(mouseHoverSound);
    }


}
