using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yolo;

public class SelectPhoto : MonoBehaviour
{
    //Load Frame
    private static Object[] frames = Resources.LoadAll("Prefab/frames/");
    private static Object[] buttons = Resources.LoadAll("Prefab/buttonsFinal");
    private static Object pageText = Resources.Load("Prefab/text/PageText");
    private static GameObject instPageText;
    private static GameObject[] instButtons = new GameObject[buttons.Length];
    private static GameObject[] inst = new GameObject[frames.Length];
    private static Texture2D[] imagesDetected;
    private static int num_page = 0;
    private static int current_page = 0;
    private static Texture2D selectedImage = null;
    private static Main main;
    private static Color color;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public static void Initializate(Main main_passed, Texture2D[] images)
    {
        main = main_passed;
        imagesDetected = images;
        color = main.border.GetComponent<Renderer>().material.color;
        _subInit();
    }

    public static void _subInit()
    {
        main.border.GetComponent<Renderer>().material.SetColor("_Color", color);
        calculateNumPage(imagesDetected.Length);
        if (num_page > 0 && current_page == 0)
            current_page = 1;
        int i = 0;
        foreach (GameObject g in buttons)
        {
            instButtons[i] = Instantiate(g, main.canvas.transform);
            i += 1;
        }
        instPageText = (GameObject)Instantiate(pageText, main.canvas.transform);
        instPageText.GetComponent<Text>().text = "Numero pagina " + current_page + " di " + num_page;
    }
    public static void selectPhoto()
    {
        main.ChangeText("topText", "Clicca sull'immagine che ti rappresenta meglio:");
        foreach (GameObject g in inst)
            Destroy(g);
        selectedImage = null;
        int i = 0;
        foreach (GameObject g in frames)
        {
            if (inst[i] != null)
                Destroy(inst[i]);
            inst[i] = Instantiate(g);
            inst[i].name = "Frame_" + i;
            i += 1;
        }
        i = (current_page-1) * 4;
        int k = 0;
        for(int el = i; el < i+4; el+=1)
        {
            if (el > imagesDetected.Length - 1)
            {
                inst[k].SetActive(false);
            }
            else
            {
                inst[k].transform.GetChild(0).gameObject.GetComponent<Renderer>().material.mainTexture = imagesDetected[el];
                inst[k].transform.GetChild(0).gameObject.AddComponent<FrameScript>();
            }
            k += 1;
            //Debug.Log(g.name);
        }
    }

    public static KeyValuePair<int,int> nextPage()
    {
        //Debug.Log("NextPageChiamato");
        if (current_page < num_page)
        {
            current_page += 1;
            selectPhoto();
            instPageText.GetComponent<Text>().text = "Numero pagina " + current_page + " di " + num_page;
        }
        return new KeyValuePair<int, int>(num_page,current_page);
    }
    
    public static int prevPage()
    {
        //Debug.Log("PrevPageChiamato");
        if (current_page > 1)
        {
            current_page -= 1;
            selectPhoto();
            instPageText.GetComponent<Text>().text = "Numero pagina " + current_page + " di " + num_page;
        }
        return current_page;
    }

    private static void calculateNumPage(int num_elements_detected)
    {
        int module = (num_elements_detected % 4);
        if (module == 0)
            num_page = num_elements_detected / 4;
        else
            num_page = (num_elements_detected / 4) + 1;
        
        //Debug.Log("Num_page: " + num_page);
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
        foreach (GameObject g in instButtons)
            Destroy(g);
        Destroy(instPageText);
    }

    public static void ripetiProcessoRilevamento()
    {
        foreach (GameObject g in inst)
            Destroy(g);
        foreach (GameObject g in instButtons)
            Destroy(g);
        Destroy(instPageText);
        main.ShowObject("initialFrame");
        main.ShowObject("box");
        main.ShowObject("bottomTextStatic");
        main.ShowObject("bottomTextDinamic");
        main.Initialize();
    }
}
