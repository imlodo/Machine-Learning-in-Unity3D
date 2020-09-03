using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using Unity.Barracuda;
using Yolo;

public class DetectGender : MonoBehaviour
{
    private static Texture2D img;
    private static Main main;
    private static Object FinalFrame = Resources.Load("Prefab/finalFrames/SelectedImage");
    private static Object[] buttons = Resources.LoadAll("Prefab/buttons/");
    private static GameObject[] instButton = new GameObject[buttons.Length];
    private static GameObject g;
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
        main = main_passed;
        g = (GameObject) Instantiate(FinalFrame);
        g.GetComponent<Renderer>().material.mainTexture = img;
        int count = 0;
        foreach(GameObject gButton in buttons)
        {
            Canvas c = main.canvas;
            instButton[count] = Instantiate(gButton, c.transform);
            count += 1;
        }
        //Debug.Log(img);
    }

    public static void back()
    {
        Destroy(g);
        foreach(GameObject gButton in instButton)
        {
            Destroy(gButton);
        }
        SelectPhoto._subInit();
        SelectPhoto.selectPhoto();
    }

    public static void detect()
    {
        Tensor tensor = Utils.TransformInputToTensor(1, img, Utils.IMAGE_SIZE, Utils.IMAGE_SIZE);
        tensor = Utils.NormalizeTensor(tensor);
        IWorker worker = LoadNeuralNetwork.getModel();
        worker.Execute(tensor);
        Tensor output = worker.PeekOutput();
        string prediction = LoadNeuralNetwork.label[output.ArgMax()[0]];
        Debug.Log("Prediction: " + prediction);
        if(prediction == LoadNeuralNetwork.label[0])
        {
            main.ChangeText("topText", "La rete neurale GenderCNN ti ha rilevato come: Uomo");
            main.border.GetComponent<Renderer>().material.SetColor("_Color", Color.cyan);
        }
        else
        {
            main.ChangeText("topText", "La rete neurale GenderCNN ti ha rilevato come: Donna");
            main.border.GetComponent<Renderer>().material.SetColor("_Color", Color.magenta);
        }
        tensor.Dispose();
    }
}
