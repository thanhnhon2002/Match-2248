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
            FB.Init(InitCallback, OnHideUnity);
        }
        else
        {
            FB.ActivateApp();
        }
    }
    private void Start()
    {
        if (FB.IsInitialized && IsLoggedIn())
        {
            FetchUserProfile();
        }
    }
    private void TrySilentLogin()
    {
        if (FB.IsLoggedIn)
        {
            FetchUserProfile();
        }
        else
        {
            FB.LogInWithReadPermissions(new List<string>() { "public_profile", "email" }, AuthCallback);
        }
    }
    public void FetchUserProfile()
    {
        FB.API("/me?fields=name,picture.type(large)", HttpMethod.GET, HandleUserInfo);
        transform.parent.GetComponent<AuthenticationManager>().UpdateSignInUI();
    }
    private void InitCallback()
    {
        if (FB.IsInitialized)
        {
#if !UNITY_EDITOR
            FB.ActivateApp();
#endif
            if (FB.IsLoggedIn)
            {
                FetchUserProfile();
            }
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
            Time.timeScale = 0;
        }
        else
        {
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
        nameText.text = "User";
        profileImage.sprite = null;
        FB.LogOut();
    }
    private void AuthCallback(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            var accessToken = AccessToken.CurrentAccessToken.TokenString;
            FirebaseManager.Instance.OnLoginFBCompleted(accessToken);
            FetchUserProfile();
        }
        else
        {
            Debug.Log("User cancelled login");
        }
    }
    private void HandleUserInfo(IResult result)
    {
        if (result.Error != null)
        {
            Debug.LogError("Error Loading Profile: " + result.Error);
            return;
        }

        if (result.ResultDictionary == null)
        {
            Debug.LogError("Result dictionary is null");
            return;
        }

        if (result.ResultDictionary.TryGetValue("name", out object name))
        {
            nameText.text = name.ToString();
            Debug.Log("User Name: " + name.ToString());
        }
        else
        {
            Debug.LogError("Name key is missing in the result dictionary");
        }

        if (result.ResultDictionary.TryGetValue("picture", out object pictureObj))
        {
            var pictureData = pictureObj as Dictionary<string, object>;
            if (pictureData != null && pictureData.TryGetValue("data", out object data))
            {
                var dataDict = data as Dictionary<string, object>;
                if (dataDict != null && dataDict.TryGetValue("url", out object url))
                {
                    StartCoroutine(LoadImage(url.ToString()));
                    Debug.Log("Picture URL: " + url.ToString());
                }
                else
                {
                    Debug.LogError("URL key is missing in the data dictionary");
                }
            }
            else
            {
                Debug.LogError("Data key is missing in the picture dictionary");
            }
        }
        else
        {
            Debug.LogError("Picture key is missing in the result dictionary");
        }
    }

    private IEnumerator LoadImage(string imageURL)
    {
        WWW www = new WWW($"{imageURL}");
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
