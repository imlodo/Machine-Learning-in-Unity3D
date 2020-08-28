using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Barracuda;
using UnityEngine;

public class Utils : MonoBehaviour
{
    public const int IMAGE_SIZE = 64;
    public Texture2D picture;
    // Start is called before the first frame update
    void Start()
    {
        Color32[] pic = picture.GetPixels32();
        Debug.Log(pic.GetValue(0));
        Tensor trasformato = TransformInputToTensor(picture, IMAGE_SIZE, IMAGE_SIZE);
        Debug.Log(trasformato.DataToString());
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Questa funzione trasforma l'immagine di input in un tensore di dimensione (1 x width x height x channels)
    public static Tensor TransformInputToTensor(Texture2D texture, int width, int height)
    {
        Color[] colors = texture.GetPixels();
        float[] results = new float[(width * height) * 3];
        for (int i = 0; i < (width*height); i++)
        {
            results[i * 3] = colors[i].r;
            results[i * 3 + 1] = colors[i].g;
            results[i * 3 + 2] = colors[i].b;
        }
        return new Tensor(1, width, height, 3, results);
    }
}
