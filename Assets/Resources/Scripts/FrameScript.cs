using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        //Debug.Log(gameObject.transform.parent.name);
        string name_frame = gameObject.transform.parent.name;
        string numb = name_frame[name_frame.Length - 1] + "";
        SelectPhoto.setFrame(Int32.Parse(numb));
    }
}
