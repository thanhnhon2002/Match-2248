using Google;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoogleAuthentication : MonoBehaviour
{
    public const string WEB_CLIENT_ID = "208772801322-vc71ernkepvd3293l3b92iamr8849925.apps.googleusercontent.com";
    [SerializeField] private Image profileImage;
    [SerializeField] private TextMeshProUGUI nameText;
    private static GoogleSignInUser currentUser;
    private static GoogleSignInConfiguration configuration;
    private string imageURL;
    private void Awake()
    {
        if (currentUser != null)
        {
            Debug.Log("User already signed in");
            OnAuthenticationFinished(Task.FromResult(currentUser));
        }
        if (GoogleSignIn.Configuration == null)
        {
            configuration = new GoogleSignInConfiguration
            {
                WebClientId = WEB_CLIENT_ID,
                RequestEmail = true,
                RequestIdToken = true
            };
            CheckSignInStatus();
            Debug.Log("Google Sign In Configuration Created");
        }
    }
    private void CheckSignInStatus()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.DefaultInstance.SignInSilently()
            .ContinueWith(OnAuthenticationFinished);
    }
    public bool IsLoggedIn()
    {
        return currentUser != null;
    }
    public void SignIn()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        GoogleSignIn.Configuration.RequestEmail = true;
        GoogleSignIn.Configuration.RequestAuthCode = true;
        Debug.Log("Calling SignIn");
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
            currentUser = task.Result;
            Debug.Log("Welcome: " + task.Result.DisplayName + "!");
            Debug.Log("Email: " + task.Result.Email);
            Debug.Log("IdToken: " + task.Result.IdToken);
            Debug.Log("UserId: " + task.Result.UserId);
            Debug.Log("AuthCode: " + task.Result.AuthCode);
            nameText.text = task.Result.DisplayName;
            imageURL = $"{task.Result.ImageUrl}";
            StartCoroutine(LoadImage());
            transform.parent.GetComponent<AuthenticationManager>().UpdateSignInUI();
            string idToken = task.Result.IdToken;
            string accessToken = task.Result.AuthCode;
            FirebaseManager.Instance.OnLoginGoogleCompleted(idToken, accessToken);
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
            nameText.text = "User";
        }
        Debug.Log("Signing out");
        if (currentUser != null)
        {
            GoogleSignIn.DefaultInstance.SignOut();
            currentUser = null;
        }
    }
}
