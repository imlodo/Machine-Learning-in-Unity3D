using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using UnityEngine;
using Yolo;

public class DetectGender : MonoBehaviour
{
    private static Texture2D img;
    private static Main main;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }
    public static void Init(Texture2D imageSelected, Main main_passed)
    {
        img = imageSelected;
        Debug.Log(img);
        main = main_passed;
    }

    public void EvalutateGender()
    {

    }
}
