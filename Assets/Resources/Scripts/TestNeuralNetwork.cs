using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Barracuda;
using UnityEngine;

public class TestNeuralNetwork : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var textures = Resources.LoadAll("ImageTest", typeof(Texture2D));
        ArrayList tensorArrayList = new ArrayList();
        foreach (Texture t in textures)
        {
            Texture2D texImage = (Texture2D) t;
            texImage = Utils.Resize(texImage, 64, 64);
            Tensor tensor = new Tensor(texImage,3);
            tensor = Utils.NormalizeTensor(tensor);
            //string tmp = "";
            //foreach (float el in tensor.AsFloats())
            //    tmp += el + "   ";
            //Debug.Log(t.name + " " + tmp);
            //Tensor tensor = ToTensor(texImage);
            //Tensor tensor = Utils.TransformInputToTensor(1,texImage,Utils.IMAGE_SIZE,Utils.IMAGE_SIZE);
            KeyValuePair<string,Tensor> pair = new KeyValuePair<string, Tensor>(t.name, tensor);
            tensorArrayList.Add(pair);
        }
        IWorker worker = LoadNeuralNetwork.getModel();
        foreach (KeyValuePair<string,Tensor> t in tensorArrayList)
        {
            worker.SetInput(t.Value);
            worker.Execute();
            Tensor output = worker.PeekOutput();
            //for (int i = 0; i < output.length; i+=1)
            //{
            //    Debug.Log("Name Image: " + t.Key + " | " + output[i]);
            //}
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
