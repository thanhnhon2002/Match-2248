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
    [SerializeField] private TextMeshProUGUI userIDText;
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
            // AccessToken class will have session details
            var aToken = Facebook.Unity.AccessToken.CurrentAccessToken.TokenString;

        }
        else
        {
            Debug.Log("User cancelled login");
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
