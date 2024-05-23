using UnityEngine;

public class InternetChecker : MonoBehaviour
{
    public static InternetChecker Instance { get; private set; }
    [SerializeField] private float checkInterval = 1f;
    private bool wasConnected;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        InitCheckInternetConnection();
    }
    private void InitCheckInternetConnection()
    {
        wasConnected = Application.internetReachability != NetworkReachability.NotReachable;
        if (!wasConnected) ShowNoInternetConnectionPopup();
        InvokeRepeating(nameof(CheckInternetConnection), checkInterval, checkInterval);
    }
    private void CheckInternetConnection()
    {
        bool isConnected = Application.internetReachability != NetworkReachability.NotReachable;

        if (wasConnected && !isConnected)
        {
            OnInternetDisconnected();
        }
        else if (!wasConnected && isConnected)
        {
            OnInternetReconnected();
        }

        wasConnected = isConnected;
    }

    private void OnInternetDisconnected()
    {
        Debug.Log("Internet connection lost.");
        ShowNoInternetConnectionPopup();
    }
    private void ShowNoInternetConnectionPopup()
    {
        PopupManager.Instance.ShowPopup(PopupOptions.NoInternet);
        Debug.Log("Please check your internet connection.");
    }
    private void OnInternetReconnected()
    {
        Debug.Log("Internet connection restored.");
        HideNoInternetConnectionPopup();
    }
    private void HideNoInternetConnectionPopup()
    {
        PopupManager.Instance.HidePopup(PopupOptions.NoInternet);
        Debug.Log("Internet connection is back.");
    }
}

