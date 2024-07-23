using DarkcupGames;
using Newtonsoft.Json;
using System;
using System.IO;
using UnityEngine;

public class ServerSaveLoadLocal : MonoBehaviour
{
    public const string FILE_NAME = "user_server";
    public const string ALPHABET = "abcdefghijklmnopqrstuvwxyz0123456789";

    public static UserDataServer LoadUserDataFromLocal()
    {
        UserDataServer userData = new UserDataServer();
        if (!IsFileExist(FILE_NAME))
        {
            userData = CreateNewUserData();
        }
        else
        {
            userData = DeserializeObjectFromFile<UserDataServer>(FILE_NAME);
            if (userData == null || userData.id == null || userData.id == "")
            {
                userData = CreateNewUserData();
            }
        }
        return userData;
    }

    public static void SaveToLocal(UserDataServer user)
    {
        string json = JsonConvert.SerializeObject(user);
        string path = FileUtilities.GetWritablePath(FILE_NAME);
        FileUtilities.SaveFile(System.Text.Encoding.UTF8.GetBytes(json), path, true);
    }

    public static UserDataServer CreateNewUserData()
    {
        UserDataServer user = new UserDataServer();
        user.id = GetRandomUserKey();
        user.CopyFromLocalData(); //CopyUserDataTo(user);
        SaveToLocal(user);
        return user;
    }

    public static UserDataServer CreateNewData()
    {
        UserDataServer user = new UserDataServer();
        user.id = GetRandomUserKey();
        user.gameData = new GameData();
        GameSystem.userdata.firstPlayGame = true;
        return user;
    }

    //public static UserDataServer CopyUserDataTo(UserDataServer user)
    //{
    //    if (GameSystem.userdata == null)
    //    {
    //        Debug.LogError("user data currently is null");
    //        return user;
    //    }
    //    if (GameSystem.userdata.gameData == null)
    //    {
    //        Debug.LogError("user game data null");
    //        return user;
    //    }
    //    user.maxIndex = GameSystem.userdata.gameData.maxIndex;
    //    user.indexPlayer = GameSystem.userdata.gameData.indexPlayer;
    //    user.nickName = GameSystem.userdata.nickName;
    //    user.avatarIndex = GameSystem.userdata.avatarIndex;
    //    return user;
    //}

    static bool IsFileExist(string filePath, bool isAbsolutePath = false)
    {
        if (filePath == null || filePath.Length == 0) return false;

        string absolutePath = filePath;
        if (!isAbsolutePath)
        {
            absolutePath = GetWritablePath(filePath);
        }
        return (System.IO.File.Exists(absolutePath));
    }

    static string GetWritablePath(string filename, string folder = "")
    {
        string path = "";

#if UNITY_EDITOR
        path = System.IO.Directory.GetCurrentDirectory() + "\\DownloadedData";
#elif UNITY_ANDROID
		path = Application.persistentDataPath ;
#elif UNITY_IPHONE
		path = Application.persistentDataPath ;
#elif UNITY_WP8 || NETFX_CORE || UNITY_WSA
		path = Application.persistentDataPath ;
#endif
        if (folder != "")
        {
            path += Path.DirectorySeparatorChar + folder;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
        path += Path.DirectorySeparatorChar + filename;
        return path;
    }

    public static T DeserializeObjectFromFile<T>(string fileName, string password = null, bool isAbsolutePath = false)
    {
        T data = default(T);
        byte[] localSaved = LoadFile(fileName, isAbsolutePath);
        if (localSaved == null)
        {
            Debug.Log(fileName + " not exist, returning null");
        }
        else
        {
            string json = System.Text.Encoding.UTF8.GetString(localSaved, 0, localSaved.Length);
            if (!string.IsNullOrEmpty(password))
            {
                string decrypt = EncryptionHelper.Decrypt(Convert.FromBase64String(json), password);
                if (string.IsNullOrEmpty(decrypt))
                {
                    Debug.LogWarning("Can't decrypt file " + fileName);
                    return data;
                }
                else
                {
                    json = decrypt;
                }
            }
            data = JsonConvert.DeserializeObject<T>(json);
            return data;
        }
        return data;
    }

    static byte[] LoadFile(string filePath, bool isAbsolutePath = false)
    {
        if (filePath == null || filePath.Length == 0)
        {
            return null;
        }

        string absolutePath = filePath;
        if (!isAbsolutePath) { absolutePath = GetWritablePath(filePath); }

        if (System.IO.File.Exists(absolutePath))
        {
            return System.IO.File.ReadAllBytes(absolutePath);
        }
        else
        {
            return null;
        }
    }

    static string GetRandomUserKey()
    {
        const int LENGHT = 8;

        string GetRandomString(int length)
        {
            string rand = "";
            for (int i = 0; i < length; i++)
            {
                rand += ALPHABET[UnityEngine.Random.Range(0, ALPHABET.Length)];
            }
            return rand;
        }
        //string appVersion = Application.version.Replace(".", "_");
        //string datetime = DateTime.Now.ToString("yyMMdd");
        //string installerName = Application.installerName.Replace(".", "_");
        //string key = datetime + "_" + appVersion + "_" + Application.systemLanguage + "_" + GetRandomString(4);
        //if (installerName == "com.android.vending")
        //{
        //    key += "_user";
        //}
        return GetRandomString(LENGHT);
    }
}