using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]

public class tiling : MonoBehaviour {
    public int checkX = 2; //offset so there are no weird errors in generating new tiles
    public Vector3 offsetNew;

    //used to check if need to instantiate stuff
    public bool hasARightBody = false;
    public bool hasALeftBody = false;

    //used if object is not tilabled
    public bool reverseScale = false;

    //width of texture
    private float spriteWidth = 0f;

    private Camera cam;
    private Transform myTransform;

    void Awake()
    {
        cam = Camera.main;
        myTransform = transform;
    }
    
	// Use this for initialization
	void Start () {
        SpriteRenderer sRenderer = GetComponent<SpriteRenderer>();
        spriteWidth = sRenderer.sprite.bounds.size.x;
	}
	
	// Update is called once per frame
	void Update () {
        //does it still need bodies
        if (!hasALeftBody || !hasARightBody)
        {
            //calculates camera's extent (half width of what camera can see)
            //in coordinates, not pixels
            float camHorizontal = cam.orthographicSize * Screen.width / Screen.height;
           
            //calculate xPosition where camera can see edge of sprite (foreground)
            float edgeVisibleRight = (myTransform.position.x + spriteWidth / 2) - camHorizontal;
            float edgeVisibleLeft = (myTransform.position.x - spriteWidth / 2) + camHorizontal;

            //checking if we can see edge and then calling MakeNewBuddy if needed
            if (cam.transform.position.x > edgeVisibleRight - checkX && !hasARightBody)
            {
                MakeNewBuddy(1);
                hasARightBody = true;
            }
            else if (cam.transform.position.x < edgeVisibleLeft + checkX && !hasALeftBody) {
                MakeNewBuddy(-1);
                hasALeftBody = true;
            }
        }
    }

    void MakeNewBuddy(int rightOrLeft)
    {
        //calculating position of new buddy
        Vector3 newPosition = new Vector3(myTransform.position.x + spriteWidth * rightOrLeft, myTransform.position.y, myTransform.position.z);

        //Instantiating new buddy (makes a clone of original) in format Transform
        Transform newBuddy = Instantiate(myTransform, newPosition, myTransform.rotation) as Transform;
        //Transform newBuddy = (Transform)Instantiate(myTransform, newPosition, myTransform.rotation);
        //both work

        //if not tilable, invert the x to make it prettier.
        if (reverseScale)
        {
            newBuddy.localScale = new Vector3(newBuddy.localScale.x * -1, newBuddy.localScale.y, newBuddy.localScale.z);
            //the inverting the X makes the tiling appear much smoother.
        }
        newBuddy.parent = myTransform.parent;

        if (rightOrLeft > 0)
        {
            newBuddy.GetComponent<tiling>().hasALeftBody = true;
        }
        else newBuddy.GetComponent<tiling>().hasARightBody = true;
    }
}
