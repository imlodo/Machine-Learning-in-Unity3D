using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        WebCamTexture backCam = LoadCamera.loadCamera();
        GetComponent<Renderer>().material.mainTexture = backCam;

        if (!backCam.isPlaying)
            backCam.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
