using Facebook.Unity;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FacebookAuthentication : MonoBehaviour
{
    [SerializeField] private Image profileImage;
    [SerializeField] private TextMeshProUGUI nameText;
    void Awake()
    {
        if (!FB.IsInitialized)
        {
            // Initialize the Facebook SDK
            FB.Init(InitCallback, OnHideUnity);
        }
        else
        {
            // Already initialized, signal an app activation App Event
            FB.ActivateApp();
        }
    }
    private void InitCallback()
    {
        if (FB.IsInitialized)
        {
            // Signal an app activation App Event
#if !UNITY_EDITOR
            FB.ActivateApp();
#endif
            // Continue with Facebook SDK
            // ...
        }
        else
        {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
    }
    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            // Pause the game - we will need to hide
            Time.timeScale = 0;
        }
        else
        {
            // Resume the game - we're getting focus again
            Time.timeScale = 1;
        }
    }
    public bool IsLoggedIn()
    {
        return FB.IsLoggedIn;
    }
    public void SignIn()
    {
        var perms = new List<string>() { "gaming_profile", "email", };
        FB.LogInWithReadPermissions(perms, AuthCallback);
    }
    public void SignOut()
    {
        FB.LogOut();
    }
    private void AuthCallback(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            var accessToken = AccessToken.CurrentAccessToken.TokenString;
            FirebaseManager.Instance.OnLoginFBCompleted(accessToken);
            FB.API("/me?fields=name", HttpMethod.GET, HandleUserInfo);
            //string pictureUrl = pictureUrlData["url"] as string;
        }
        else
        {
            Debug.Log("User cancelled login");
        }
    }
    private void HandleUserInfo(IResult result)
    {
        if (result == null)
        {
            Debug.LogError("Failed to Load Profile. Response is null");
            return;
        }

        // Log any errors that may happen
        if (result.Error != null)
        {
            Debug.LogError("Error Loading Profile: " + result.Error);
            return;
        }

        // If there was no error, handle the user's name
        if (!string.IsNullOrEmpty(result.RawResult))
        {
            var userInfo = (Dictionary<string, object>)Facebook.MiniJSON.Json.Deserialize(result.RawResult);
            string name = userInfo["name"] as string;
            nameText.text = name;  // Set the name to your UI Text

            Debug.Log("User Name: " + name);
        }
        else
        {
            Debug.LogError("Empty Response on Profile Information");
        }
    }
    private IEnumerator LoadImage(System.Uri imageURL)
    {
        WWW www = new WWW(imageURL.ToString());
        yield return www;
        profileImage.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), Vector2.zero);
    }
    public void ShareLink()
    {
        FB.ShareLink(new System.Uri("https://www.darkcupgames.com/"), "Check out this game", "This game is awesome", new System.Uri("https://www.facebook.com/"));
    }
    public void InviteFriends()
    {
        FB.AppRequest("Come play this awesome game with me!", title: "Invite your friends to join you");
    }
}
