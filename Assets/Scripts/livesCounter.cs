using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class livesCounter : MonoBehaviour {
    private Text lives;

    // Use this for initialization
    void Awake()
    {
        lives = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        lives.text = "Lives: " + GameMaster.RemainingLives.ToString();
    }
}
