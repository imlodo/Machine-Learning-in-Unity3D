using System.Collections;
using System.Collections.Generic;
using Unity.Barracuda;
using UnityEngine;

public class TestNeuralNetwork : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var textures = Resources.LoadAll("ImageTest", typeof(Texture2D));
        ArrayList tensorArrayList = new ArrayList();
        foreach (var t in textures)
        {
            Texture2D texImage = (Texture2D)t;
            
            Tensor tensor = Utils.TransformInputToTensor(texImage,Utils.IMAGE_SIZE,Utils.IMAGE_SIZE);
            KeyValuePair<string,Tensor> pair = new KeyValuePair<string, Tensor>(t.name, tensor);
            tensorArrayList.Add(pair);
        }
        IWorker worker = LoadNeuralNetwork.getModel();
        foreach (KeyValuePair<string,Tensor> t in tensorArrayList)
        {
            worker.Execute(t.Value);
            Tensor output = worker.PeekOutput();
            Debug.Log("Name Image: " + t.Key + ", Prediction: " + LoadNeuralNetwork.label[output.ArgMax()[0]]);
            //Debug.Log("Name Image: " + t.Key + ", Tensor: " + output[0] + " " + output[1]);
            t.Value.Dispose();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
