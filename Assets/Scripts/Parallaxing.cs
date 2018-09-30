using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxing : MonoBehaviour {

    public Transform[] backgrounds;
    private float[] parallaxScales; // the proportion of the camera movement for each background
    public float smoothing = 1f; //how smooth the parallaxing is. Make sure to set this above 0.

    private Transform cam; //reference to main camera transform.
    private Vector3 previousCamPos; //stores position of camera in previous frame.

    //is called before Start() and after game objects are set up.
    //great for references.
    void Awake()
    {
        cam = Camera.main.transform;
    }

    // Use this for initialization
    void Start()
    {
        previousCamPos = cam.position;

        parallaxScales = new float[backgrounds.Length];
        for (int i = 0; i < backgrounds.Length; i++)
        {
            parallaxScales[i] = backgrounds[i].position.z * -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < backgrounds.Length; i++)
        {
            float parallax = (previousCamPos.x - cam.position.x) * parallaxScales[i];

            float backgroundTargetX = backgrounds[i].position.x + parallax;
            Vector3 backgroundTargetPosition = new Vector3(backgroundTargetX, backgrounds[i].position.y, backgrounds[i].position.z);

            //slowly transitions between current pos and target pos using technique called lerp.
            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPosition, smoothing * Time.deltaTime);
        }

        previousCamPos = cam.position;
    }
}
