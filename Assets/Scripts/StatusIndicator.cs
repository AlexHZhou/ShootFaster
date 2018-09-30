using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusIndicator : MonoBehaviour {

    //class is named "StatusIndicator"
    [SerializeField]
    private RectTransform healthBarRect;
    [SerializeField]
    private Text healthText;

    void Start()
    {
        if (healthBarRect == null) Debug.LogError("STATUS INDICATOR: no health bar object referenced");
        //if (healthText == null) Debug.LogError("STATUS INDICATOR: no health text object referenced");

    }

    public void SetHealth(float _cur, float _max)
    {
        float _percentHP = (float)_cur / _max;
        healthBarRect.localScale = new Vector3(_percentHP, healthBarRect.localScale.x, healthBarRect.localScale.y);
        //potentially add color (gradient).
        try
        {
            healthText.text = _cur + "/" + _max + "HP";
        }
        catch { Debug.Log("No heathtext found."); }

        //NEXT:
        //Set healthBarRect and healthText via Unity.
        //Add in maxHealth, currentHealth variable to Enemy (in top and in constructor)

    }
}
