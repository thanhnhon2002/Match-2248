using DG.Tweening;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingShowAppOpen : MonoBehaviour
{
    [SerializeField] private bool showDebug;

    private float LOADING_TIME;
    public PopupLoading popupLoading;
    public AdmobAppOpen appOpen;
    public Image unmask;

    private void Awake()
    {
        StartLoadingAndShowAppOpen(() =>
        {
            unmask.rectTransform.DOSizeDelta(Vector2.zero, Const.DEFAULT_TWEEN_TIME).OnComplete(() => SceneManager.LoadScene("UI Home"));         
        });
    }

    public async void StartLoadingAndShowAppOpen(System.Action onLoadFinished)
    {
        while (!FirebaseManager.remoteConfig.fetch) await Task.Yield();
        LOADING_TIME = FirebaseManager.remoteConfig.LOADING_TIME;
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