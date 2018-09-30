using UnityEngine;
using UnityEngine.UI;

public class ShopToolTipAlert : MonoBehaviour
{
    
    public Transform cityBound;
    public Player player;
    public Text thisText;
    public Text count;
    public bool pausedBetweenWaves;

    public void Awake()
    {
        thisText = GetComponent<Text>();
    }

    public void FixedUpdate()
    {
        if (player == null) return;

        if (player.transform.position.x > cityBound.position.x)
        {
            thisText.text = "";
            count.enabled = true;
        }
        else
        {
            thisText.text = "Press <i>Enter</i> to enter a store.";
            count.enabled = false;
        }
    }
}
