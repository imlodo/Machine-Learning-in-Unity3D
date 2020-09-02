using UnityEngine;

public static class TextureExtentions
{
    public static Texture2D ToTexture2D(this Texture texture)
    {
        Texture2D temp = new Texture2D(texture.width, texture.height, TextureFormat.RGB24, false, true);
        RenderTexture rt = new RenderTexture(texture.width, texture.height, 24);
        RenderTexture.active = rt;
        Graphics.Blit(texture, rt);
        temp.ReadPixels(new Rect(0, 0, texture.width, texture.width), 0, 0);
        temp.Apply();
        return temp;
    }
}