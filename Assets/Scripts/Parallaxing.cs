using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxing : MonoBehaviour
{
    public Transform[] backgrounds;     // Array of all the back and foregrounds to be parallaxed
    public float smoothing = 1f;        // How smooth  the parallax is going to be. Make sure this is above 0
    private float[] parallaxScales;     // The proportion of the camera's movement to move the background by
    private Transform cam;              // reference to the main cameras transform;
    private Vector3 previousCamPos;     // store position of the camera in the previous frame

    // Is called before Start ()
    void Awake ()
    {
        cam = Camera.main.transform;
    }

    // Start is called before the first frame update
    void Start ()
    {
        previousCamPos = cam.position;
        parallaxScales = new float[backgrounds.Length];

        for (int i = 0; i < backgrounds.Length; i++) {
            parallaxScales[i] = backgrounds[i].position.z * -1;
        }
    }

    // Update is called once per frame
    void Update ()
    {
        for (int i = 0; i < backgrounds.Length; i++) {
            float parallax = (previousCamPos.x - cam.position.x) * parallaxScales[i];
            float backgroundTargetPosX = backgrounds[i].position.x + parallax;
            Vector3 backgroundTargetPos = new Vector3 (backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);
            backgrounds[i].position = Vector3.Lerp (backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
        }

        previousCamPos = cam.position;
    }
}
