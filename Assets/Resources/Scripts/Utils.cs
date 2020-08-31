using System.Collections.Generic;
using Unity.Barracuda;
using UnityEngine;
using System.IO;
using ObjectDetection.YoloParser;
using System.Drawing;
using ProtoTurtle.BitmapDrawing;
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
    //Questa funzione trasforma l'immagine di input in un tensore di dimensione (1 x width x height x channels)
    //public static Tensor TransformInputToTensor(int batch, Texture2D texture, int width, int height)
    //{
    //    Color[] colors = texture.GetPixels();
    //    float[] results = new float[(width * height) * 3];
    //    for (int i = 0; i < (width * height); i++)
    //    {
    //        results[i * 3] = colors[i].r;
    //        results[i * 3 + 1] = colors[i].g;
    //        results[i * 3 + 2] = colors[i].b;
    //    }
    //    return new Tensor(batch, width, height, 3, results);
    //}

    private static Texture2D Resize(Texture2D texture2D, int targetX, int targetY)
    {
        RenderTexture rt = new RenderTexture(targetX, targetY, 24);
        RenderTexture.active = rt;
        Graphics.Blit(texture2D, rt);
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
    public static void DrawRect(Texture2D texture, YoloBoundingBox outline, float scaleFactor, float shiftX, float shiftY)
    {
        var x = outline.Dimensions.X * scaleFactor + shiftX;
        var width = outline.Dimensions.Width * scaleFactor;
        var y = outline.Dimensions.Y * scaleFactor + shiftY;
        var height = outline.Dimensions.Height * scaleFactor;
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.DrawRectangle(new Rect(x, y, width, height), Color.red);
        texture.Apply();
    }

    public static void LogDetectedObjects(string imageName, IList<YoloBoundingBox> boundingBoxes)
    {
        Debug.Log($".....The objects in the image {imageName} are detected as below....");

        foreach (var box in boundingBoxes)
        {
            Debug.Log($"{box.Label} and its Confidence score: {box.Confidence}");
        }
    }
    public static void SaveToFile(Texture2D texture,string name)
    {
        var filePath = Application.dataPath + "/Resources/ImageTest/Output/" + name+".png";
        //Debug.LogError(filePath);
        File.WriteAllBytes(
            filePath, texture.EncodeToPNG());
    }

}
