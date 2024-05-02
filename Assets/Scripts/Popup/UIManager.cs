using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    public GameObject prefabReward;
    public Transform uiReward;
    public DiamondGroup diamondGroup;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else Destroy(gameObject);
    }
    public void SpawnEffectReward(Vector2 spawner)
    {
        var obj = PoolSystem.Instance.GetObject(prefabReward, spawner, PopupManager.Instance.transform);
        obj.GetComponent<SoundPopup>().PlayPopupSound();
        LeanTween.value(0, 1, MoveBenzier.timeDafault+0.3f).setOnComplete(()=>
        {
            diamondGroup.Display();
            obj.SetActive(false);
        });
    }
}