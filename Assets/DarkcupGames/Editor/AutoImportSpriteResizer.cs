using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class AutoImportSpriteResizer : AssetPostprocessor
{
    public const string END_FILE_NAME = " - MUL4";
    public const string FOLDER_NAME = "MUL4";
    public static List<int> dimensions = null;

    public void Init()
    {
        dimensions = new List<int>();
        for (int i = 0; i < 20; i++)
        {
            dimensions.Add((int)Mathf.Pow(2, i));
        }
    }

    void OnPostprocessSprites(Texture2D texture, Sprite[] sprites)
    {
        if (texture.name.EndsWith(END_FILE_NAME)) return;
        if (assetPath.Contains(FOLDER_NAME) == false) return;

        string extension = Path.GetExtension(assetPath);
        string path = assetPath.Replace(extension, "");
        string newPath = path + END_FILE_NAME + extension;
        for (int i = 0; i < sprites.Length; i++)
        {
            int x = GetMultiple4((int)sprites[i].rect.width);
            int y = GetMultiple4((int)sprites[i].rect.height);
            SaveFile(sprites[i], x, y, newPath);
            File.Delete(assetPath);
            Debug.Log($"Create MUL4 Sprite {texture.name} with dimension {x} {y}");
        }
    }

    public int GetPOTSize(int number)
    {
        if (dimensions == null) Init();
        for (int i = 0; i < dimensions.Count; i++)
        {
            if (number < dimensions[i]) return dimensions[i];
        }
        Debug.LogError($"number too big to put to POT");
        return number;
    }

    public int GetMultiple4(int number)
    {
        if (number % 4 == 0) return number;
        int result = number / 4;
        return (result + 1) * 4;
    }

    public void SaveFile(Sprite sprite, int newWidth, int newHeight, string savePath)
    {
        Texture2D texture = new Texture2D(newWidth, newHeight);
        for (int i = 0; i < texture.width; i++)
        {
            for (int j = 0; j < texture.height; j++)
            {
                if (i < sprite.texture.width && j < sprite.texture.height)
                {
                    texture.SetPixel(i, j, sprite.texture.GetPixel(i, j));
                }
                else
                {
                    texture.SetPixel(i, j, Color.clear);
                }
            }
        }
        texture.Apply();
        byte[] itemBGBytes;
        if (savePath.EndsWith("png"))
        {
            itemBGBytes = texture.EncodeToPNG();
        }
        else if (savePath.EndsWith("jpg") || savePath.EndsWith("jpeg"))
        {
            itemBGBytes = texture.EncodeToJPG();
        }
        else
        {
            Debug.LogError("file format not support");
            return;
        }
        File.WriteAllBytes(savePath, itemBGBytes);
        AssetDatabase.ImportAsset(savePath);
    }

    void OnPostprocessTexture(Texture2D texture)
    {

    }
}