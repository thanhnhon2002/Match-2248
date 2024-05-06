using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingShowAppOpen : MonoBehaviour
{
    [SerializeField] private bool showDebug;

    public float LOADING_TIME = 7f;
    public PopupLoading popupLoading;
    public AdmobAppOpen appOpen;
    public Canvas canvasLoading;

    private void Awake()
    {
        StartLoadingAndShowAppOpen(() =>
        {
            SceneManager.LoadScene("UI Home");
        });
    }

    public void StartLoadingAndShowAppOpen(System.Action onLoadFinished)
    {
        popupLoading.gameObject.SetActive(true);
        popupLoading.ShowLoading(LOADING_TIME, () =>{
            if (showDebug) Debug.Log("try showing app open");
            if (appOpen.IsAdsAvailable())
            {
                AdmobAppOpen.placement = "loading";
                if (showDebug) Debug.Log("app open available, showing ads");
                appOpen.ShowAds(() =>
                {
                    if (showDebug) Debug.Log("this is app open closed");
                    //popupLoading.Close();
                    onLoadFinished?.Invoke();
                });
            }
            else
            {
                if (showDebug) Debug.Log("app open available failed");
                //popupLoading.Close();
                onLoadFinished?.Invoke();
            }
        });
    }
}