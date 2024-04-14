using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

public class AddresableProperty : ObjectProperties
{
    public const int PRE_LOAD_ASSET_AMOUNT = 5;
    public static AddresableProperty Instance { get; private set; }
    [SerializeField] private AssetReference[] addresableRefs;
    public UnityEvent<float> OnDoneLoadingEach;
    [SerializeField] private int currentAssetLoaded;

    protected override void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
        ready = false;
        SceneManager.activeSceneChanged += ClearEvent;
    }

    public override async void Init()
    {
        await LoadAllRef();
    }

    public async void PreLoad(int preloadAmount = PRE_LOAD_ASSET_AMOUNT)
    {
        await PreLoadAsset(preloadAmount);
    }


    private async Task PreLoadAsset(int preloadAmount)
    {
        var currentLoaded = currentAssetLoaded;
        var loadCount = 0;
        var amount = currentAssetLoaded + preloadAmount;
        if (amount > addresableRefs.Length) amount = addresableRefs.Length;
        if (currentLoaded == amount)
        {
            Debug.Log("All assets has been loaded");
            return;
        }
        for (int i = currentLoaded; i < amount; i++)
        {
            var handle = addresableRefs[i].LoadAssetAsync<GameObject>();
            await handle.Task;
            if (handle.Status == AsyncOperationStatus.Failed) Debug.LogError($"Fail to load reference of {addresableRefs[i].Asset.name}");
            else
            {
                if (collection.ContainsKey(i))
                {
                    Debug.LogError($"Duplicate key of object name {addresableRefs[i].Asset.name}");
                    return;
                }
                collection.Add(i, handle.Result);
                currentAssetLoaded++;
                loadCount++;
                OnDoneLoadingEach?.Invoke((float)loadCount / preloadAmount);
            }
        }
        ready = true;
        Debug.Log($"Preload Completed {loadCount} assets. Total completed {currentAssetLoaded} assets");
        onLoadComplete?.Invoke();
    }

    private async Task LoadAllRef()
    {
        for (int i = 0; i < addresableRefs.Length; i++)
        {
            var handle = addresableRefs[i].LoadAssetAsync<GameObject>();
            await handle.Task;
            if (handle.Status == AsyncOperationStatus.Failed) Debug.LogError($"Fail to load reference of {addresableRefs[i].Asset.name}");
            else
            {
                if (collection.ContainsKey(i))
                {
                    Debug.LogError($"Duplicate key of object name {addresableRefs[i].Asset.name}");
                    return;
                }
                if (collection.ContainsValue((GameObject)addresableRefs[i].Asset))
                {
                    Debug.LogError($"Duplicate value of object name {addresableRefs[i].Asset.name}");
                    return;
                }
                collection.Add(i, handle.Result);
            }
        }
        ready = true;
        onLoadComplete?.Invoke();
    }

    private void ClearEvent(UnityEngine.SceneManagement.Scene thisScene, UnityEngine.SceneManagement.Scene nextScene)
    {
        onLoadComplete.RemoveAllListeners();
    }
}