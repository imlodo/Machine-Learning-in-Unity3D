using System.Collections;
using System.Collections.Generic;
using Unity.Barracuda;
using UnityEngine;

public class LoadYoloNetwork : MonoBehaviour
{
    private static NNModel modelAsset = (NNModel)Resources.Load("NeuralNetwork/tiny_yolo");
    public static string[] labels = new string[]
    {
        "aeroplane", "bicycle", "bird", "boat", "bottle",
         "bus", "car", "cat", "chair", "cow",
         "diningtable", "dog", "horse", "motorbike", "person",
         "pottedplant", "sheep", "sofa", "train", "tvmonitor"
    };
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static IWorker getModel()
    {
        Model m_RuntimeModel = ModelLoader.Load(model: modelAsset);
        //Debug.Log(m_RuntimeModel);
        IWorker m_Worker = WorkerFactory.CreateWorker(m_RuntimeModel, WorkerFactory.Device.GPU);
        //Debug.Log(m_Worker);
        return m_Worker;
    }
}
