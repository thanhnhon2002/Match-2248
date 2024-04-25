using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    public GameObject prefabReward;
    public Transform uiReward;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else Destroy(gameObject);
    }
    public void SpawnEffectReward(Transform spawner)
    {
        var obj = PoolSystem.Instance.GetObject(prefabReward, spawner.position,PopupManager.Instance.transform);
        obj.GetComponent<SoundPopup>().PlayPopupSound();
        LeanTween.value(0, 1, MoveBenzier.timeDafault+0.3f).setOnComplete(()=>
        {
            obj.SetActive(false);
        });
    }
}