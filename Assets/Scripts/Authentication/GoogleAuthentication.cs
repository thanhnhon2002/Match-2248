using Google;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class GoogleAuthentication : MonoBehaviour
{
    public const string WEB_CLIENT_ID = "208772801322-vc71ernkepvd3293l3b92iamr8849925.apps.googleusercontent.com";
    [SerializeField] private Image profileImage;
    private GoogleSignInConfiguration configuration;
    private string imageURL;
    private void Start()
    {
        configuration = new GoogleSignInConfiguration
        {
            WebClientId = WEB_CLIENT_ID,
            RequestEmail = true,
            RequestIdToken = true
        };
    }
    public void SignIn()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        GoogleSignIn.Configuration.RequestEmail = true;
        GoogleSignIn.Configuration.RequestAuthCode = true;
        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished);
    }
    private void OnAuthenticationFinished(Task<GoogleSignInUser> task)
    {
        if (task.IsFaulted)
        {
            using (IEnumerator<Exception> enumerator = task.Exception.InnerExceptions.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    GoogleSignIn.SignInException error = (GoogleSignIn.SignInException)enumerator.Current;
                    Debug.Log("Got Error: " + error.Status + " " + error.Message);
                }
                else
                {
                    Debug.Log("Got Unexpected Exception: " + task.Exception);
                }
            }
        }
        else if (task.IsCanceled)
        {
            Debug.Log("Canceled");
        }
        else
        {
            Debug.Log("Welcome: " + task.Result.DisplayName + "!");
            Debug.Log("Email: " + task.Result.Email);
            Debug.Log("IdToken: " + task.Result.IdToken);
            Debug.Log("UserId: " + task.Result.UserId);
            Debug.Log("AuthCode: " + task.Result.AuthCode);
            imageURL = $"{task.Result.ImageUrl}";
            StartCoroutine(LoadImage());
        }
    }
    private IEnumerator LoadImage()
    {
        WWW www = new WWW(imageURL);
        yield return www;
        profileImage.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), Vector2.zero);
    }
    public void SignOut()
    {
        if (profileImage != null)
        {
            profileImage.sprite = null;
        }
        Debug.Log("Signing out");
        GoogleSignIn.DefaultInstance.SignOut();
    }
}
