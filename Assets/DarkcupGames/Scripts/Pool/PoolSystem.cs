using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class PoolSystem : MonoBehaviour
{
    public static PoolSystem Instance { get; private set; }
    private Dictionary<string, List<GameObject>> allPool = new Dictionary<string, List<GameObject>> ();

    private void Awake ()
    {
        Instance = this;
    }

    private string SliptKey (string path)
    {
        var key = path;
        if (path.Contains ("/"))
        {
            var stringArr = path.Split ('/');
            key = stringArr[stringArr.Length - 1];
        }
        key = key.Replace ("(Clone)", " ");
        key = key.TrimStart ();
        key = key.TrimEnd ();
        return key;
    }

    #region Non-Genreic
    public GameObject LoadObjectWithPath (string path, Vector3 position, Transform parent = null)
    {

        if (!allPool.ContainsKey (path))
        {
            var newObject = Instantiate (Resources.Load<GameObject> (path), position, Quaternion.identity, parent);
            var newPool = new List<GameObject> ();
            newPool.Add (newObject);
            allPool.Add (path, newPool);
            return newObject;
        }

        var pool = allPool[path];
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                pool[i].SetActive (true);
                pool[i].transform.SetParent (parent);
                pool[i].transform.position = position;
                return pool[i];
            }
        }

        var addObject = Instantiate (Resources.Load<GameObject> (path), position, Quaternion.identity, parent);
        pool.Add (addObject);
        return addObject;
    }

    /// <summary>
    /// Not Recomended
    /// </summary>
    /// <param name="path"></param>
    /// <param name="position"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    public async Task<GameObject> LoadAddressableWithPath (string path, Vector3 position, Transform parent = null)
    {
        var handle = Addressables.LoadAssetAsync<GameObject> (path);
        GameObject returnObj = null;
        handle.Completed += (AsyncOperationHandle<GameObject> obj) =>
        {
            if (!allPool.ContainsKey (path))
            {
                var newPool = new List<GameObject> ();
                returnObj = Instantiate (obj.Result, position, Quaternion.identity, parent);
                newPool.Add (returnObj);
                allPool.Add (path, newPool);
                return;
            }
            var pool = allPool[path];
            for (int i = 0; i < pool.Count; i++)
            {
                if (!pool[i].activeInHierarchy)
                {
                    pool[i].SetActive (true);
                    pool[i].transform.SetParent (parent);
                    pool[i].transform.position = position;
                    returnObj = pool[i];
                    return;
                }
            }

            var addObject = Instantiate (obj.Result, position, Quaternion.identity, parent);
            pool.Add (addObject);
            returnObj = addObject;
        };
        await handle.Task;
        if (handle.Status == AsyncOperationStatus.Failed)
        {
            Debug.LogError ($"Fail to load game object at {path}");
            return null;
        }
        return returnObj;
    }


    /// <summary>
    /// Not Recomended
    /// </summary>
    /// <param name="path"></param>
    /// <param name="position"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    public IEnumerator<GameObject> IELoadAddressableWithPath (string path, Vector3 position, Transform parent = null)
    {
        var handle = Addressables.LoadAssetAsync<GameObject> (path);
        GameObject returnObj = null;
        handle.Completed += (AsyncOperationHandle<GameObject> obj) =>
        {
            if (!allPool.ContainsKey (path))
            {
                var newPool = new List<GameObject> ();
                returnObj = Instantiate (obj.Result, position, Quaternion.identity, parent);
                newPool.Add (returnObj);
                allPool.Add (path, newPool);
                return;
            }
            var pool = allPool[path];
            for (int i = 0; i < pool.Count; i++)
            {
                if (!pool[i].activeInHierarchy)
                {
                    pool[i].SetActive (true);
                    pool[i].transform.SetParent (parent);
                    pool[i].transform.position = position;
                    returnObj = pool[i];
                    return;
                }
            }

            var addObject = Instantiate (obj.Result, position, Quaternion.identity, parent);
            pool.Add (addObject);
            returnObj = addObject;
        };
        yield return handle.WaitForCompletion ();
        if (handle.Status != AsyncOperationStatus.Succeeded)
        {
            Debug.LogError ($"Fail to load game object at {path}");
            yield return null;
            yield break;
        }
        yield return returnObj;
    }

    public GameObject LoadObject (string objectPath, Vector3 position, Transform parent = null)
    {
        var key = SliptKey (objectPath);

        if (!allPool.ContainsKey (key))
        {
            var newObject = Instantiate (Resources.Load<GameObject> (objectPath), position, Quaternion.identity, parent);
            var newPool = new List<GameObject> ();
            newPool.Add (newObject);
            allPool.Add (key, newPool);
            return newObject;
        }

        var pool = allPool[key];
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                pool[i].SetActive (true);
                pool[i].transform.SetParent (parent);
                pool[i].transform.position = position;
                return pool[i];
            }
        }

        var addObject = Instantiate (Resources.Load<GameObject> (objectPath), position, Quaternion.identity, parent);
        pool.Add (addObject);
        return addObject;
    }

    public GameObject GetObject (ObjectProperties data, int key, Vector3 position, Transform parent = null)
    {
        var keyName = SliptKey (data.collection[key].name);
        if (!data.collection.ContainsKey (key))
        {
            Debug.LogError ($"Can't find {keyName} in {data}");
            return null;
        }
        var obj = data.collection[key];
        if (!data.collection.ContainsValue (obj))
        {
            Debug.LogError ($"Can't find GameObject {keyName} in {data}");
            return null;
        }
        if (!allPool.ContainsKey (keyName))
        {
            var newObject = Instantiate (obj, position, Quaternion.identity, parent);
            var newPool = new List<GameObject> ();
            newPool.Add (newObject);
            allPool.Add (keyName, newPool);
            return newObject;
        }

        var pool = allPool[keyName];
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                pool[i].SetActive (true);
                pool[i].transform.SetParent (parent);
                pool[i].transform.position = position;
                return pool[i];
            }
        }

        var addObject = Instantiate (obj, position, Quaternion.identity, parent);
        pool.Add (addObject);
        return addObject;
    }

    public async Task<GameObject> GetObjectAddressable (AssetReference reference, Vector3 position, Transform parent = null)
    {
        if (reference.IsValid ()) return GetObject ((GameObject)reference.Asset, position, parent);

        var handle = reference.LoadAssetAsync<GameObject> ();
        GameObject returnObj = null;
        handle.Completed += (AsyncOperationHandle<GameObject> obj) =>
        {
            if (!allPool.ContainsKey (reference.Asset.name))
            {
                var newPool = new List<GameObject> ();
                returnObj = Instantiate (obj.Result, position, Quaternion.identity, parent);
                newPool.Add (returnObj);
                allPool.Add (reference.Asset.name, newPool);
                return;
            }
            var pool = allPool[reference.Asset.name];
            for (int i = 0; i < pool.Count; i++)
            {
                if (!pool[i].activeInHierarchy)
                {
                    pool[i].SetActive (true);
                    pool[i].transform.SetParent (parent);
                    pool[i].transform.position = position;
                    returnObj = pool[i];
                    return;
                }
            }

            var addObject = Instantiate (obj.Result, position, Quaternion.identity, parent);
            pool.Add (addObject);
            returnObj = addObject;
        };
        await handle.Task;
        if (handle.Status == AsyncOperationStatus.Failed)
        {
            Debug.LogError ($"Fail to load game object at of the reference {reference.Asset.name}");
            return null;
        }
        return returnObj;
    }

    public IEnumerator<GameObject> IEGetAddressableObject (AssetReference reference, Vector3 position, Transform parent = null)
    {
        if (reference.IsValid ()) yield return GetObject ((GameObject)reference.Asset, position, parent);
        var handle = reference.LoadAssetAsync<GameObject> ();
        GameObject returnObj = null;
        handle.Completed += (AsyncOperationHandle<GameObject> obj) =>
        {
            if (!allPool.ContainsKey (reference.Asset.name))
            {
                var newPool = new List<GameObject> ();
                returnObj = Instantiate (obj.Result, position, Quaternion.identity, parent);
                newPool.Add (returnObj);
                allPool.Add (reference.Asset.name, newPool);
                return;
            }
            var pool = allPool[reference.Asset.name];
            for (int i = 0; i < pool.Count; i++)
            {
                if (!pool[i].activeInHierarchy)
                {
                    pool[i].SetActive (true);
                    pool[i].transform.SetParent (parent);
                    pool[i].transform.position = position;
                    returnObj = pool[i];
                    return;
                }
            }

            var addObject = Instantiate (obj.Result, position, Quaternion.identity, parent);
            pool.Add (addObject);
            returnObj = addObject;
        };
        yield return handle.WaitForCompletion ();
        if (handle.Status != AsyncOperationStatus.Succeeded)
        {
            Debug.LogError ($"Fail to load game object at {reference.Asset.name}");
            yield return null;
            yield break;
        }
        yield return returnObj;
    }

    public GameObject GetAddressableObject (int indexInAddressable, Vector3 position, Transform parent = null)
    {
        if (AddresableProperty.Instance == null)
        {
            Debug.LogError ("AddresableProperty null");
            return null;
        }
        if (!AddresableProperty.Instance.ready)
        {
            Debug.LogError ("AddresableProperty is not ready");
            return null;
        }
        return GetObject (AddresableProperty.Instance.collection[indexInAddressable], position, parent);
    }

    public GameObject GetObject (GameObject obj, Vector3 position, Transform parent = null)
    {
        var key = SliptKey (obj.name);
        if (!allPool.ContainsKey (key))
        {
            var newObject = Instantiate (obj, position, Quaternion.identity, parent);
            var newPool = new List<GameObject> ();
            newPool.Add (newObject);
            allPool.Add (key, newPool);
            return newObject;
        }

        var pool = allPool[key];
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                pool[i].SetActive (true);
                pool[i].transform.SetParent (parent);
                pool[i].transform.position = position;
                return pool[i];
            }
        }

        var addObject = Instantiate (obj, position, Quaternion.identity, parent);
        pool.Add (addObject);
        return addObject;
    }

    public T GetObjectFromPool<T>(T prefab, Transform parent) where T : Component
    {
        string key = prefab.name;

        if (!allPool.ContainsKey(key))
        {
            var newObject = Instantiate(prefab, parent);
            newObject.transform.position = parent.position;
            var newPool = new List<GameObject>();
            newPool.Add(newObject.gameObject);
            allPool.Add(key, newPool);
            return newObject;
        }
        var pool = allPool[key];
        foreach (var obj in pool)
        {
            if (!obj.gameObject.activeInHierarchy)
            {
                obj.transform.position = parent.position;
                obj.gameObject.SetActive(true);
                return obj.GetComponent<T>();
            }
        }
        var newObj = Instantiate(prefab, parent);
        newObj.transform.position = parent.position;
        pool.Add(newObj.gameObject);
        return newObj;
    }

    public GameObject GetObject (GameObject obj, Vector3 position, Vector3 rotation, Transform parent = null)
    {
        var key = SliptKey (obj.name);
        if (!allPool.ContainsKey (key))
        {
            var newObject = Instantiate (obj, position, Quaternion.identity, parent);
            var newPool = new List<GameObject> ();
            newPool.Add (newObject);
            allPool.Add (key, newPool);
            return newObject;
        }

        var pool = allPool[key];
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                pool[i].SetActive (true);
                pool[i].transform.SetParent (parent);
                pool[i].transform.position = position;
                pool[i].transform.rotation = Quaternion.Euler (rotation);
                return pool[i];
            }
        }

        var addObject = Instantiate (obj, position, Quaternion.identity, parent);
        pool.Add (addObject);
        return addObject;
    }
    #endregion

    #region Generic
    /// <summary>
    /// Not Recomended
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <param name="position"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    public async Task<T> LoadAddressableWithPath<T> (string path, Vector3 position, Transform parent = null) where T : Component
    {
        var handle = Addressables.LoadAssetAsync<GameObject> (path);
        T returnType = default (T);
        handle.Completed += (AsyncOperationHandle<GameObject> obj) =>
        {
            if (!allPool.ContainsKey (path))
            {
                var newPool = new List<GameObject> ();
                returnType = Instantiate (obj.Result, position, Quaternion.identity, parent).GetComponent<T> ();
                newPool.Add (returnType.gameObject);
                allPool.Add (path, newPool);
                return;
            }
            var pool = allPool[path];
            for (int i = 0; i < pool.Count; i++)
            {
                if (!pool[i].activeInHierarchy)
                {
                    pool[i].SetActive (true);
                    pool[i].transform.SetParent (parent);
                    pool[i].transform.position = position;
                    pool[i].TryGetComponent<T> (out returnType);
                    return;
                }
            }

            var addObject = Instantiate (obj.Result, position, Quaternion.identity, parent);
            pool.Add (addObject.gameObject);
            returnType = addObject.GetComponent<T> ();
        };
        await handle.Task;
        if (handle.Status == AsyncOperationStatus.Failed)
        {
            Debug.LogError ($"Fail to load game object at {path}");
            return null;
        }
        return returnType;
    }
    /// <summary>
    /// Not Recomended
    /// </summary>
    /// <param name="path"></param>
    /// <param name="position"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    public IEnumerator<T> IELoadAddressableWithPath<T> (string path, Vector3 position, Transform parent = null) where T : Component
    {
        var handle = Addressables.LoadAssetAsync<GameObject> (path);
        T returnType = default (T);
        handle.Completed += (AsyncOperationHandle<GameObject> obj) =>
        {
            if (!allPool.ContainsKey (path))
            {
                var newPool = new List<GameObject> ();
                returnType = Instantiate (obj.Result, position, Quaternion.identity, parent).GetComponent<T> ();
                newPool.Add (returnType.gameObject);
                allPool.Add (path, newPool);
                return;
            }
            var pool = allPool[path];
            for (int i = 0; i < pool.Count; i++)
            {
                if (!pool[i].activeInHierarchy)
                {
                    pool[i].SetActive (true);
                    pool[i].transform.SetParent (parent);
                    pool[i].transform.position = position;
                    pool[i].TryGetComponent<T> (out returnType);
                    return;
                }
            }

            var addObject = Instantiate (obj.Result, position, Quaternion.identity, parent);
            pool.Add (addObject.gameObject);
            returnType = addObject.GetComponent<T> ();
        };
        yield return handle.WaitForCompletion ().GetComponent<T> ();
        if (handle.Status != AsyncOperationStatus.Succeeded)
        {
            Debug.LogError ($"Fail to load game object at {path}");
            yield return null;
            yield break;
        }
        yield return returnType;
    }

    public T LoadObjectWithPath<T> (string path, Vector3 position, Transform parent = null) where T : Component
    {
        if (!allPool.ContainsKey (path))
        {
            var newObject = Instantiate (Resources.Load<GameObject> (path), position, Quaternion.identity, parent);
            var newPool = new List<GameObject> ();
            newPool.Add (newObject);
            allPool.Add (path, newPool);
            if (newObject.TryGetComponent<T> (out var returnType)) return returnType;
            else
            {
                var message = new string[] { $"Can't find component {newObject.name} in GameObjetc", path };
                Debug.LogError (message);
                return default (T);
            }
        }

        var pool = allPool[path];
        GameObject disableObject = null;
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                disableObject = pool[i];
                break;
            }
        }

        if (disableObject != null)
        {
            disableObject.transform.position = position;
            disableObject.SetActive (true);
            disableObject.transform.SetParent (parent);
            if (disableObject.TryGetComponent<T> (out var type)) return type;
            else
            {
                var message = new string[] { $"Can't find component {disableObject.name} in GameObjet", path };
                Debug.LogError (message);
                return default (T);
            }
        }

        var addObject = Instantiate (Resources.Load<GameObject> (path), position, Quaternion.identity, parent);
        pool.Add (addObject);
        if (addObject.TryGetComponent<T> (out var newType)) return newType;
        else
        {
            var message = new string[] { "Can't find component in GameObjetc", path };
            Debug.LogError (message);
            return default (T);
        }
    }

    public T LoadObject<T> (string objectPath, Vector3 position, Transform parent = null) where T : Component
    {
        var key = SliptKey (objectPath);

        if (!allPool.ContainsKey (key))
        {
            var newObject = Instantiate (Resources.Load<GameObject> (objectPath), position, Quaternion.identity, parent);
            var newPool = new List<GameObject> ();
            newPool.Add (newObject);
            allPool.Add (key, newPool);
            if (newObject.TryGetComponent<T> (out var returnType)) return returnType;
            else
            {
                var message = new string[] { $"Can't find component {newObject.name} in GameObjetc", objectPath };
                Debug.LogError (message);
                return default (T);
            }
        }

        var pool = allPool[key];
        GameObject disableObject = null;
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                disableObject = pool[i];
                break;
            }
        }

        if (disableObject != null)
        {
            disableObject.transform.position = position;
            disableObject.SetActive (true);
            disableObject.transform.SetParent (parent);
            if (disableObject.TryGetComponent<T> (out var type)) return type;
            else
            {
                var message = new string[] { $"Can't find component {disableObject.name} in GameObjetc", objectPath };
                Debug.LogError (message);
                return default (T);
            }
        }

        var addObject = Instantiate (Resources.Load<GameObject> (objectPath), position, Quaternion.identity, parent);
        pool.Add (addObject);
        if (addObject.TryGetComponent<T> (out var newType)) return newType;
        else
        {
            var message = new string[] { "Can't find component in GameObjetc", objectPath };
            Debug.LogError (message);
            return default (T);
        }
    }

    public async Task<T> GetAddressableObject<T> (AssetReference reference, Vector3 position, Transform parent = null) where T : Component
    {
        T returnType = default (T);
        if (reference.IsValid ())
        {
            var obj = GetObject ((GameObject)reference.Asset, position, parent);
            returnType = obj.GetComponent<T> ();
        } else
        {
            var handle = reference.LoadAssetAsync<GameObject> ();
            handle.Completed += (AsyncOperationHandle<GameObject> obj) =>
            {
                if (!allPool.ContainsKey (reference.Asset.name))
                {
                    var newPool = new List<GameObject> ();
                    returnType = Instantiate (obj.Result, position, Quaternion.identity, parent).GetComponent<T> ();
                    newPool.Add (returnType.gameObject);
                    allPool.Add (reference.Asset.name, newPool);
                    return;
                }
                var pool = allPool[reference.Asset.name];
                for (int i = 0; i < pool.Count; i++)
                {
                    if (!pool[i].activeInHierarchy)
                    {
                        pool[i].SetActive (true);
                        pool[i].transform.SetParent (parent);
                        pool[i].transform.position = position;
                        pool[i].TryGetComponent<T> (out returnType);
                        return;
                    }
                }

                var addObject = Instantiate (obj.Result, position, Quaternion.identity, parent);
                pool.Add (returnType.gameObject);
                addObject.TryGetComponent<T> (out returnType);
            };
            await handle.Task;
            if (handle.Status == AsyncOperationStatus.Failed)
            {
                Debug.LogError ($"Fail to load game object at of the reference {reference.Asset.name}");
                return null;
            }
        }
        return returnType;
    }

    public IEnumerator<T> IEGetAddressableObject<T> (AssetReference reference, Vector3 position, Transform parent = null) where T : Component
    {
        T returnType = default (T);
        if (reference.IsValid ())
        {
            var obj = GetObject ((GameObject)reference.Asset, position, parent);
            returnType = obj.GetComponent<T> ();
        } else
        {
            var handle = reference.LoadAssetAsync<GameObject> ();
            handle.Completed += (AsyncOperationHandle<GameObject> obj) =>
            {
                if (!allPool.ContainsKey (reference.Asset.name))
                {
                    var newPool = new List<GameObject> ();
                    returnType = Instantiate (obj.Result, position, Quaternion.identity, parent).GetComponent<T> ();
                    newPool.Add (returnType.gameObject);
                    allPool.Add (reference.Asset.name, newPool);
                    return;
                }
                var pool = allPool[reference.Asset.name];
                for (int i = 0; i < pool.Count; i++)
                {
                    if (!pool[i].activeInHierarchy)
                    {
                        pool[i].SetActive (true);
                        pool[i].transform.SetParent (parent);
                        pool[i].transform.position = position;
                        pool[i].TryGetComponent<T> (out returnType);
                        return;
                    }
                }

                var addObject = Instantiate (obj.Result, position, Quaternion.identity, parent);
                pool.Add (addObject.gameObject);
                returnType = addObject.GetComponent<T> ();
            };
            yield return handle.WaitForCompletion ().GetComponent<T> ();
            if (handle.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.LogError ($"Fail to load game object at {reference.Asset.name}");
                yield return null;
                yield break;
            }
        }
        yield return returnType;
    }
    public T GetAddressableObject<T> (int indexInAddressable, Vector3 position, Transform parent = null) where T : Component
    {
        if (AddresableProperty.Instance == null)
        {
            Debug.LogError ("AddresableProperty null");
            return null;
        }
        if (!AddresableProperty.Instance.ready)
        {
            Debug.LogError ("AddresableProperty is not ready");
            return null;
        }
        return GetObject<T> (AddresableProperty.Instance.collection[indexInAddressable], position, parent);
    }

    public T GetObject<T> (ObjectProperties data, int key, Vector3 position, Transform parent = null) where T : Component
    {
        var keyName = SliptKey (data.collection[key].name);
        if (!data.collection.ContainsKey (key))
        {
            Debug.LogError ($"Can't find {keyName} in {data}");
            return default (T);
        }

        var obj = data.collection[key];
        if (!data.collection.ContainsValue (obj))
        {
            Debug.LogError ($"Can't find GameObject {keyName} in {data}");
            return default (T);
        }
        if (!allPool.ContainsKey (keyName))
        {
            var newObject = Instantiate (obj, position, Quaternion.identity, parent);
            var newPool = new List<GameObject> ();
            newPool.Add (newObject);
            allPool.Add (keyName, newPool);
            if (newObject.TryGetComponent<T> (out var returnType)) return returnType;
            else
            {
                var message = new string[] { "Can't find component in GameObjetc", keyName };
                Debug.LogError (message);
                return default (T);
            }

        }

        var pool = allPool[keyName];
        GameObject disableObject = null;
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                disableObject = pool[i];
                break;
            }
        }

        if (disableObject != null)
        {
            disableObject.transform.position = position;
            disableObject.SetActive (true);
            disableObject.transform.SetParent (parent);
            if (disableObject.TryGetComponent<T> (out var type)) return type;
            else
            {
                var message = new string[] { "Can't find component in GameObjetc", keyName };
                Debug.LogError (message);
                return default (T);
            }
        }

        var addObject = Instantiate (obj, position, Quaternion.identity, parent);
        pool.Add (addObject);
        if (addObject.TryGetComponent<T> (out var newType)) return newType;

        else
        {
            var message = new string[] { "Can't find component in GameObjetc", keyName };
            Debug.LogError (message);
            return default (T);
        }
    }

    public T GetObject<T> (T obj, Vector3 position, Transform parent = null) where T : Component
    {
        var key = SliptKey (obj.name);
        if (!allPool.ContainsKey (key))
        {
            var newObject = Instantiate (obj, position, Quaternion.identity);
            newObject.transform.SetParent(parent);
            var newPool = new List<GameObject> ();
            newPool.Add (newObject.gameObject);
            allPool.Add (key, newPool);
            if (newObject.TryGetComponent<T> (out var returnType)) return returnType;
            else
            {
                var message = new string[] { "Can't find component in GameObjetc", key };
                Debug.LogError (message);
                return default (T);
            }
        }

        var pool = allPool[key];
        GameObject disableObject = null;
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                disableObject = pool[i];
                break;
            }
        }

        if (disableObject != null)
        {
            disableObject.transform.position = position;
            disableObject.SetActive (true);
            disableObject.transform.SetParent (parent);
            if (disableObject.TryGetComponent<T> (out var type)) return type;
            else
            {
                var message = new string[] { "Can't find component in GameObjetc", key };
                Debug.LogError (message);
                return default (T);
            }
        }

        var addObject = Instantiate (obj, position, Quaternion.identity, parent);
        pool.Add (addObject.gameObject);
        if (addObject.TryGetComponent<T> (out var newType)) return newType;
        else
        {
            var message = new string[] { "Can't find component in GameObjetc", key };
            Debug.LogError (message);
            return default (T);
        }
    }

    public T GetObject<T> (GameObject obj, Vector3 position, Transform parent = null) where T : Component
    {
        var key = SliptKey (obj.name);
        if (!allPool.ContainsKey (key))
        {
            var newObject = Instantiate (obj, position, Quaternion.identity, parent);
            var newPool = new List<GameObject> ();
            newPool.Add (newObject.gameObject);
            allPool.Add (key, newPool);
            if (newObject.TryGetComponent<T> (out var returnType)) return returnType;
            else
            {
                var message = new string[] { "Can't find component in GameObjetc", key };
                Debug.LogError (message);
                return default (T);
            }
        }

        var pool = allPool[key];
        GameObject disableObject = null;
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                disableObject = pool[i];
                break;
            }
        }

        if (disableObject != null)
        {
            disableObject.transform.position = position;
            disableObject.SetActive (true);
            disableObject.transform.SetParent (parent);
            if (disableObject.TryGetComponent<T> (out var type)) return type;
            else
            {
                var message = new string[] { "Can't find component in GameObjetc", key };
                Debug.LogError (message);
                return default (T);
            }
        }

        var addObject = Instantiate (obj, position, Quaternion.identity, parent);
        pool.Add (addObject.gameObject);
        if (addObject.TryGetComponent<T> (out var newType)) return newType;
        else
        {
            var message = new string[] { "Can't find component in GameObjetc", key };
            Debug.LogError (message);
            return default (T);
        }
    }
    #endregion
}

