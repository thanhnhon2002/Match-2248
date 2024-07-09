using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AuthenticationManager : MonoBehaviour
{
    public static AuthenticationManager Instance;
    [SerializeField] private GameObject googleSignInButton;
    [SerializeField] private GameObject facebookSignInButton;
    [SerializeField] private GameObject signOutButton;

    [SerializeField] private GoogleAuthentication googleAuth;
    //[SerializeField] private FacebookAuthentication facebookAuth;

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateSignInUI()
    {
        bool isGoogleLoggedIn = googleAuth.IsLoggedIn();
        Debug.Log("Trang thai login:" + isGoogleLoggedIn);
        //bool isFacebookLoggedIn = facebookAuth.IsLoggedIn();

        if (isGoogleLoggedIn /*|| isFacebookLoggedIn*/)
        {
            Debug.Log("Trang thai login:" + isGoogleLoggedIn);
            googleSignInButton.SetActive(false);
            facebookSignInButton.SetActive(false);
            signOutButton.SetActive(true);
        }
        else
        {
            Debug.Log("Trang thai login:" + isGoogleLoggedIn);
            googleSignInButton.SetActive(true);
            //facebookSignInButton.SetActive(true);
            signOutButton.SetActive(false);
        }
    }

    public void SignOut()
    {
        UpdateSignInUI();
        FirebaseManager.Instance.SignOut();
    }
}
