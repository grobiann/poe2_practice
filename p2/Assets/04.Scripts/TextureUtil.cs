using System;
using UnityEngine;

public static class TextureUtil
{
    public static void SaveTexture(Texture2D texture, string name = "")
    {
        byte[] bytes = texture.EncodeToPNG();
        string path = Application.dataPath + "/../" + (name == "" ? texture.name : name) + ".png";
        System.IO.File.WriteAllBytes(path, bytes);
    }
}
