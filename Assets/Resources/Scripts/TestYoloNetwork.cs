using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Barracuda;
using ObjectDetection.YoloParser;
using System.Linq;
using System.Threading;

public class TestYoloNetwork : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var textures = Resources.LoadAll("ImageTest", typeof(Texture2D));
        ArrayList tensorArrayList = new ArrayList();
        foreach (var t in textures)
        {
            Texture2D texImage = (Texture2D)t;
            Tensor tensor = Utils.TransformInputToTensor(0,texImage, 416, 416);
            Texture textureResize = (Texture)BarracudaTextureUtils.TensorToRenderTexture(tensor);
            //Texture2D texture2D = TextureExtentions.ToTexture2D(textureResize); //Si bugga
            GetComponent<Renderer>().material.mainTexture = textureResize;
            KeyValuePair<string, Tensor> pair = new KeyValuePair<string, Tensor>(t.name, tensor);
            tensorArrayList.Add(pair);
        }
        IWorker worker = LoadYoloNetwork.getModel();
        int img_counter = 0;
        foreach (KeyValuePair<string, Tensor> t in tensorArrayList)
        {
            worker.Execute(t.Value);
            Tensor output = worker.PeekOutput();
            YoloOutputParser yoloOutParser = new YoloOutputParser();
            IList<YoloBoundingBox> boundingBox = yoloOutParser.ParseOutputs(output.AsFloats());
            //boundingBox = yoloOutParser.FilterBoundingBoxes(boundingBox, 5, YoloOutputParser.MINIMUM_CONFIDENCE);
            Texture texture = (Texture)BarracudaTextureUtils.TensorToRenderTexture(t.Value);
            int count = 0;
            int count_save = 0;
            foreach (YoloBoundingBox y in boundingBox)
            {
                Debug.Log("Name Image: " + t.Key + " " + "Prediction: " + y.Label + " Confidence: " + y.Confidence);
                //Filter person
                if (y.Label == "person")
                {
                    if (count_save >= 5)
                    {
                        break;
                    }
                    Texture2D image = (Texture2D)textures[img_counter];
                    //RenderTexture rt = new RenderTexture(416, 416, 24);
                    //RenderTexture.active = rt;
                    //Graphics.Blit(image, rt);
                    //Texture2D result = new Texture2D(416, 416, TextureFormat.RGB24, false, true);
                    //result.ReadPixels(new Rect(0, 0, 416, 416), 0, 0);
                    //result.Apply();
                    //GetComponent<Renderer>().material.mainTexture = result;
                    //Debug.Log(result.format);

                    //Utils.DrawRect(result, y, 1, 0f, 0f);
                    //Utils.SaveToFile(result, t.Key + "_" + count);
                    int x_passed = (int)y.Dimensions.X;
                    int width = (int)y.Dimensions.Width;
                    int y_passed = (int)y.Dimensions.Y;
                    int height = (int)y.Dimensions.Height;
                    if (x_passed > 0 && y_passed > 0)
                    {
                        image = Utils.Crop(image, width, height);
                        Utils.SaveToFile(image, t.Key + "_" + count);
                        Debug.Log("Name Image: " + t.Key + " " + "Prediction: " + y.Label + " Confidence: " + y.Confidence);
                        count_save += 1;
                    }
                }
                count += 1;
            }
            //Debug.Log(output.shape);
            //Debug.Log("Name Image: " + t.Key + ", Prediction: " + LoadYoloNetwork.labels[output.ArgMax()[0]]);
            //Debug.Log("Name Image: " + t.Key + ", Tensor: " + output[0] + " " + output[1]);
            t.Value.Dispose();
            img_counter += 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
