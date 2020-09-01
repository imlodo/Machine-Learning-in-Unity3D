using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectPhoto : MonoBehaviour
{
    //Load Frame
    private static Object[] frames = Resources.LoadAll("Prefab/frames/");
    private static string[] page = { "1", "2", "3"};
    private static string current_page = page[0];
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static Texture2D selectPhoto()
    {
        foreach(GameObject g in frames)
        {
            Debug.Log(g.name);
        }
        return null;
    }
}
