using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class moneyCounter : MonoBehaviour {
    private Text moneyText;

    // Use this for initialization
    void Awake()
    {
        moneyText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        moneyText.text = "Money: " + GameMaster.Money.ToString();
    }
}
