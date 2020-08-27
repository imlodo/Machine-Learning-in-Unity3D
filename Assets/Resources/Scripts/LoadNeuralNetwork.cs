using System.Collections;
using System.Collections.Generic;
using Unity.Barracuda;
using UnityEngine;

public class LoadNeuralNetwork : MonoBehaviour
{
    private static NNModel modelAsset = (NNModel)Resources.Load("NeuralNetwork/gender");
    public static string[] label = { "Man", "Woman" };
    void Start()
    {
    }
    void Update()
    {
    }

    public static IWorker getModel()
    {
        Model m_RuntimeModel = ModelLoader.Load(model: modelAsset);
        IWorker m_Worker = WorkerFactory.CreateWorker(WorkerFactory.Type.ComputePrecompiled, m_RuntimeModel);
        return m_Worker;
    }
}
