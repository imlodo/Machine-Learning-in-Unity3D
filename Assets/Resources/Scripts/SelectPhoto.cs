using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yolo;

public class SelectPhoto : MonoBehaviour
{
    //Load Frame
    private static Object[] frames = Resources.LoadAll("Prefab/frames/");
    private static GameObject[] inst = new GameObject[frames.Length];
    private static int num_page = 0;
    private static int current_page = 0;
    private static Texture2D selectedImage = null;
    private static Main main;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public static void Initializate(Main main_passed)
    {
        main = main_passed;
    }
    public static Texture2D selectPhoto(Texture2D[] imagesDetected)
    {
        selectedImage = null;
        calculateNumPage(imagesDetected.Length);
        int i = 0;
        foreach (GameObject g in frames)
        {
            if (inst[i] != null)
                Destroy(inst[i]);
            inst[i] = Instantiate(g);
            inst[i].name = "Frame_" + i;
            inst[i].transform.GetChild(0).gameObject.GetComponent<Renderer>().material.mainTexture = imagesDetected[i];
            inst[i].transform.GetChild(0).gameObject.AddComponent<FrameScript>();
            i += 1;
            //Debug.Log(g.name);
        }
        return null;
    }

    public void nextPage()
    {
        if(current_page < num_page)
            current_page += 1;
    }
    
    public void prevPage()
    {
        if (current_page > 1)
            current_page -= 1;
    }

    private static void calculateNumPage(int num_elements_detected)
    {
        int module = (num_elements_detected % 4);
        if (module == 0)
            num_page = num_elements_detected / 4;
        else
            num_page = (num_elements_detected / 4) + 1;
        
        Debug.Log("Num_page: " + num_page);
    }

    public static void setFrame(int index)
    {
        selectedImage = (Texture2D) inst[index].transform.GetChild(0).gameObject.GetComponent<Renderer>().material.mainTexture;
        if (selectedImage != null)
        {
            DetectGender.Init(selectedImage, main);
            selectedImage = null;
        }
        main.HiddenObject("bottomTextStatic");
        main.HiddenObject("bottomTextDinamic");
        main.ChangeText("topText", "Hai selezionato la seguente immagine:");
        foreach (GameObject g in inst)
            Destroy(g);
    }
}
