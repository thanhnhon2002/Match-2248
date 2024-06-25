using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Runtime.CompilerServices;

public class Avatar : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Sprite[] sprites;
    private static readonly YieldAwaitable yield = Task.Yield();
    public static Sprite avatar { get; private set; }

    private async void OnEnable()
    {
        var user = ServerSystem.user;
        if (user.typeLogin != UserDataServer.TypeLogin.Guest && !string.IsNullOrEmpty(user.avatarPath))
            avatar = await LoadAvatar(user.avatarPath);
        else avatar = sprites[user.avatarIndex];
        image.sprite = avatar;
    }

    public static async Task<Sprite> LoadAvatar(string url)
    {
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(url))
        {
            var asyncOp = www.SendWebRequest();
            while (asyncOp.isDone == false)
                await yield;

            if (www.result != UnityWebRequest.Result.Success) return null;
            var texture = DownloadHandlerTexture.GetContent(www);
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        }
    }
}
