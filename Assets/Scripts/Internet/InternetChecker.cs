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
#if !UNITY_EDITOR
        InitCheckInternetConnection();
#endif
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
        Debug.Log("Please check your internet connection.");
        PopupManager.Instance.ShowPopup(PopupOptions.NoInternet);
    }
    private void OnInternetReconnected()
    {
        Debug.Log("Internet connection restored.");
        HideNoInternetConnectionPopup();
    }
    private void HideNoInternetConnectionPopup()
    {
        Debug.Log("Internet connection is back.");
        PopupManager.Instance.HidePopup(PopupOptions.NoInternet);
    }
}

