using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadCamera : MonoBehaviour
{
    private static WebCamTexture backCam;

    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public static WebCamTexture loadCamera()
    {
        if (backCam == null)
            backCam = new WebCamTexture();
        return backCam;
    }
}
