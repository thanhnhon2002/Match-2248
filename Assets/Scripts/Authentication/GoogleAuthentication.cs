using DarkcupGames;
using Firebase.Extensions;
using Google;
using Newtonsoft.Json;
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
    public static string Name { get; private set; }
    private void Awake()
    {
        if(GoogleSignIn.Configuration == null)
        {
            configuration = new GoogleSignInConfiguration
            {
                WebClientId = WEB_CLIENT_ID,
                RequestEmail = true,
                RequestIdToken = true
            };
            Debug.Log("configurationnnnnnnnnnnn:" + configuration);
            //CheckSignInStatus();
            Debug.Log("Google Sign In Configuration Created");
        }
        GetDataLogin();
        if (currentUser != null)
        {
            Debug.Log("User already signed in");
            GoogleSignIn.Configuration = configuration;
            OnAuthenticationFinished(Task.FromResult(currentUser));
        }
        else
        {
            Debug.Log("Null roiiiiiiiiii");
        }
    }
    private void CheckSignInStatus()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.DefaultInstance.SignInSilently()
            .ContinueWithOnMainThread(OnAuthenticationFinished);
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
        GoogleSignIn.DefaultInstance.SignIn().ContinueWithOnMainThread(OnAuthenticationFinished);
    }

    public void Switch()
    {
        GoogleSignIn.DefaultInstance.SignOut();
        GoogleSignIn.Configuration = null;
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        GoogleSignIn.Configuration.RequestEmail = true;
        GoogleSignIn.Configuration.RequestAuthCode = true;
        Debug.Log("Calling SignIn");
        GoogleSignIn.DefaultInstance.SignIn().ContinueWithOnMainThread(SwitchFinished);
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
            ConvertIdManager.UpdateIdConvert(task.Result.UserId, () =>
            {
                currentUser = task.Result;
                SaveDataToFile(currentUser);
                Debug.Log("Welcome: " + task.Result.DisplayName + "!");
                Debug.Log("Email: " + task.Result.Email);
                Debug.Log("IdToken: " + task.Result.IdToken);
                Debug.Log("UserId: " + task.Result.UserId);
                Debug.Log("AuthCode: " + task.Result.AuthCode);
                nameText.text = task.Result.DisplayName;
                Name = task.Result.DisplayName;
                imageURL = $"{task.Result.ImageUrl}";
                StartCoroutine(LoadImage());
                transform.parent.GetComponent<AuthenticationManager>().UpdateSignInUI();
                string idToken = task.Result.IdToken;
                string accessToken = task.Result.AuthCode;
                FirebaseManager.Instance.OnLoginGoogleCompleted(idToken, accessToken);

                Debug.Log("Google Name" + Name);
                GameSystem.userdata.nickName = Name;
                ServerSystem.user.typeLogin = UserDataServer.TypeLogin.Google;
                ServerSystem.user.idGoogle = task.Result.UserId;
                ServerSystem.user.avatarPath = imageURL;
                DataUserManager.SaveUserData();
                Debug.Log("GOi tu Login");
            });
        }
    }

    private void SwitchFinished(Task<GoogleSignInUser> task)
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
            ConvertIdManager.UpdateIdConvert(task.Result.UserId, () =>
            {
                Debug.Log("Ban dau goi action: " + ServerSystem.user.id);
                currentUser = task.Result;
                SaveDataToFile(currentUser);
                Debug.Log("Welcome: " + task.Result.DisplayName + "!");
                Debug.Log("Email: " + task.Result.Email);
                Debug.Log("IdToken: " + task.Result.IdToken);
                Debug.Log("UserId: " + task.Result.UserId);
                Debug.Log("AuthCode: " + task.Result.AuthCode);
                Name = task.Result.DisplayName;
                imageURL = $"{task.Result.ImageUrl}";
                transform.parent.GetComponent<AuthenticationManager>().UpdateSignInUI();
                string idToken = task.Result.IdToken;
                string accessToken = task.Result.AuthCode;
                FirebaseManager.Instance.OnLoginGoogleCompleted(idToken, accessToken);

                Debug.Log("Google Name" + Name);
                GameSystem.userdata.nickName = Name;
                ServerSystem.user.idGoogle = task.Result.UserId;
                ServerSystem.user.avatarPath = imageURL;
                ServerSystem.user.typeLogin = UserDataServer.TypeLogin.Google;
                Debug.Log("GOi truoc khi Chyen Tk: " + ServerSystem.user.id);
                DataUserManager.SaveUserData();
                Debug.Log("GOi tu Chyen Tk: " + ServerSystem.user.id);
            });
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
        PopupNotification.Instance.ShowPopupYesNo("Are you sure you want to unlink?", () =>
        {
            GoogleSignIn.DefaultInstance.SignOut();
            AuthenticationManager.Instance.SignOut();

            if (profileImage != null)
            {
                profileImage.sprite = null;
            }
            Debug.Log("Signing out");
            if (currentUser != null)
            {
                currentUser = null;
                ServerSystem.user.typeLogin = UserDataServer.TypeLogin.Guest;
                ServerSystem.user.avatarPath = string.Empty;
                ConvertIdManager.RemoveConvertId(ServerSystem.user.idGoogle);
                DataUserManager.SaveUserData();
                Debug.Log("GOi tu Dang xuat");
                Debug.Log("Da unlink:" +ServerSystem.user.typeLogin);
            }
            SaveDataToFile(null);
            AuthenticationManager.Instance.UpdateSignInUI();
        });     
    }

    public void GetDataLogin()
    {
        UserGoogle userGoogle = ServerSaveLoadLocal.DeserializeObjectFromFile<UserGoogle>("dataLogin");
        if (userGoogle.email == null)
        {
            currentUser = null;
        }
        else
        {
            currentUser = new GoogleSignInUser();
            currentUser.AuthCode = userGoogle.authCode;
            currentUser.DisplayName = userGoogle.displayName;
            currentUser.Email = userGoogle.email;
            currentUser.FamilyName = userGoogle.familyName;
            currentUser.GivenName = userGoogle.givenName;
            currentUser.IdToken = userGoogle.idToken;
            currentUser.ImageUrl = userGoogle.imageUrl;
            currentUser.UserId = userGoogle.userId;
            Debug.Log("Gan roiiiiiiiiiiiiiiiiiiiiiiii");
        }
    }

    public void SaveDataToFile(GoogleSignInUser signInUser)
    {
        UserGoogle userGoogle = new UserGoogle();
        if (signInUser != null)
        {
            userGoogle.authCode = signInUser.AuthCode;
            userGoogle.displayName = signInUser.DisplayName;
            userGoogle.email = signInUser.Email;
            userGoogle.familyName = signInUser.FamilyName;
            userGoogle.givenName = signInUser.GivenName;
            userGoogle.idToken = signInUser.IdToken;
            userGoogle.imageUrl = signInUser.ImageUrl;
            userGoogle.userId = signInUser.UserId;
        }
        string json = JsonConvert.SerializeObject(userGoogle);
        string path = FileUtilities.GetWritablePath("dataLogin");
        FileUtilities.SaveFile(System.Text.Encoding.UTF8.GetBytes(json), path, true);
    }

}
