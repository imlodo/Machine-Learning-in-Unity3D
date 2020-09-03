using System.Collections.Generic;
using Unity.Barracuda;
using UnityEngine;
using System.IO;
using System.Drawing;
using Color = UnityEngine.Color;

public class Utils : MonoBehaviour
{
    public const int IMAGE_SIZE = 64;
    public const int IMAGE_SIZE_YOLO = 416;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //Questa funzione trasforma l'immagine di input in un tensore di dimensione (1 x width x height x channels)
    public static Tensor TransformInputToTensor(int batch, Texture2D texture, int width, int height)
    {
        Texture2D textureTmp = Resize(texture, width, height);
        Tensor t = new Tensor(textureTmp, 3);
        //var shape = t.shape;
        //Debug.Log(shape + " or " + shape.batch + shape.height + shape.width + shape.channels);
        return t;
    }

    public static Texture2D Resize(Texture2D texture2D, int targetX, int targetY)
    {
        RenderTexture rt = new RenderTexture(targetX, targetY, 24);
        RenderTexture.active = rt;
        Graphics.Blit(texture2D, rt);
        Texture2D result = new Texture2D(targetX, targetY);
        result.ReadPixels(new Rect(0, 0, targetX, targetY), 0, 0);
        result.Apply();
        return result;
    }

    public static Texture2D ResizeNormalTexture(Texture texture, int targetX, int targetY)
    {
        RenderTexture rt = new RenderTexture(targetX, targetY, 24);
        RenderTexture.active = rt;
        Graphics.Blit(texture, rt);
        Texture2D result = new Texture2D(targetX, targetY);
        result.ReadPixels(new Rect(0, 0, targetX, targetY), 0, 0);
        result.Apply();
        return result;
    }

    public static Texture2D Crop(Texture2D source, int targetWidth, int targetHeight)
    {
        int sourceWidth = source.width;
        int sourceHeight = source.height;
        float sourceAspect = (float)sourceWidth / sourceHeight;
        float targetAspect = (float)targetWidth / targetHeight;
        int xOffset = 0;
        int yOffset = 0;
        float factor = 1;
        if (sourceAspect > targetAspect)
        { // crop width
            factor = (float)targetHeight / sourceHeight;
            xOffset = (int)((sourceWidth - sourceHeight * targetAspect) * 0.5f);
        }
        else
        { // crop height
            factor = (float)targetWidth / sourceWidth;
            yOffset = (int)((sourceHeight - sourceWidth / targetAspect) * 0.5f);
        }
        Color32[] data = source.GetPixels32();
        Color32[] data2 = new Color32[targetWidth * targetHeight];
        for (int y = 0; y < targetHeight; y++)
        {
            for (int x = 0; x < targetWidth; x++)
            {
                var p = new Vector2(Mathf.Clamp(xOffset + x / factor, 0, sourceWidth - 1), Mathf.Clamp(yOffset + y / factor, 0, sourceHeight - 1));
                // bilinear filtering
                var c11 = data[Mathf.FloorToInt(p.x) + sourceWidth * (Mathf.FloorToInt(p.y))];
                var c12 = data[Mathf.FloorToInt(p.x) + sourceWidth * (Mathf.CeilToInt(p.y))];
                var c21 = data[Mathf.CeilToInt(p.x) + sourceWidth * (Mathf.FloorToInt(p.y))];
                var c22 = data[Mathf.CeilToInt(p.x) + sourceWidth * (Mathf.CeilToInt(p.y))];
                var f = new Vector2(Mathf.Repeat(p.x, 1f), Mathf.Repeat(p.y, 1f));
                data2[x + y * targetWidth] = Color.Lerp(Color.Lerp(c11, c12, p.y), Color.Lerp(c21, c22, p.y), p.x);
            }
        }

        var tex = new Texture2D(targetWidth, targetHeight);
        tex.SetPixels32(data2);
        tex.Apply(true);
        return tex;
    }
    public static void SaveToFile(Texture2D texture,string name)
    {
        var filePath = Application.dataPath + "/Resources/ImageTest/Output/" + name+".png";
        //Debug.LogError(filePath);
        File.WriteAllBytes(
            filePath, texture.EncodeToPNG());
    }

    public static Tensor NormalizeTensor(Tensor tensore)
    {
        float[] IMAGE_MEAN = { (float)0.485, (float)0.456, (float)0.406 };
        float[] IMAGE_STD = { (float)0.229, (float)0.224, (float)0.225 };
        Tensor temp = new Tensor(1, 64, 64, 3);

        for (int y = 0; y < 64; y++)
        {
            for (int x = 0; x < 64; x++)
            {
                float r = (tensore[0, y, x, 0] - IMAGE_MEAN[0]) / IMAGE_STD[0];
                float g = (tensore[0, y, x, 1] - IMAGE_MEAN[1]) / IMAGE_STD[1];
                float b = (tensore[0, y, x, 2] - IMAGE_MEAN[2]) / IMAGE_STD[2];
                temp[0, y, x, 0] = r;
                temp[0, y, x, 1] = g;
                temp[0, y, x, 2] = b;

            }
        }

        return temp;

    }

}
