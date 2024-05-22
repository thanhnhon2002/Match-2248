using System.Collections;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    public GameObject prefabReward;
    public Transform uiReward;
    public DiamondGroup diamondGroup;
    float timeCount;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else Destroy(gameObject);
    }
    public void SpawnEffectReward(Transform spawner)
    {
        timeCount = 0;
        StartCoroutine(ISpawnEffectReward(spawner));

    }
    public IEnumerator ISpawnEffectReward(Transform spawner)
    {
        var obj = PoolSystem.Instance.GetObject(prefabReward, spawner.position, PopupManager.Instance.transform);
        obj.GetComponent<SoundPopup>().PlayPopupSound();
        while (timeCount <= MoveBenzier.timeDafault + 0.3f)
        {
            timeCount += Time.deltaTime;
            yield return null;
        }
        diamondGroup.Display();
    }
}