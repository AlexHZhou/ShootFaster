using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wall : MonoBehaviour {
    BoxCollider2D box;

    public void Start()
    {
        box = GetComponent<BoxCollider2D>();
    }

    public void FixedUpdate()
    {
        if (WaveSpawner._state != "COUNTING") box.isTrigger = true;
        else box.isTrigger = false;
    }
}
