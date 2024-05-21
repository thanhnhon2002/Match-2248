using Firebase.Database;
using Firebase.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace DeepTrackSDK
{
    [System.Serializable]
    public class DeepTrackUser
    {
        public string id;
        public bool firstOpen = false;
        public double startPlayDay;
        public string lastPlayDay;
        public int totalPlayDay;
        public float totalPlayTime;
        public int retentionType;
        public int lastInterShow;
        public int level = -1;
        public int maxLevel;
        public string language;
        public bool internetDisable;
        public string appVersion;
        public string originalAppVersion;
        public int sessionCount;
        public int exceptionCount;
        public string installerName;
        public int totalSkippedIntertistial;
        public int currentLevelPlayed;
        public List<string> actionChecker = new List<string>();
        public Dictionary<string, DeepTrackWinLoseData> winloseDatas = new Dictionary<string, DeepTrackWinLoseData>();
        public DeepTrackUserRevenue revenue = new DeepTrackUserRevenue();

        public void CheckValid()
        {
            if (revenue == null) revenue = new DeepTrackUserRevenue();
            if (winloseDatas == null) winloseDatas = new Dictionary<string, DeepTrackWinLoseData>();
            if (actionChecker == null) actionChecker = new List<string>();
        }
    }

    [System.Serializable]
    public class DeepTrackSession
    {
        public string id;
        public string installer;
        public float totalTime;
        public int skippedInter;
        public float secondPerAds;
        public float secondPerInter;
        public string startTime;
        public float sessionTime;
        public int totalPlayDay;
        public int retentionType;
        public string language;
        public List<string> actions;
    }

    [RequireComponent(typeof(DeepTrack))]
    public class DeepTrackSaveLoadData : MonoBehaviour
    {
        public static DatabaseReference databaseRef;
        private static string startTime;
        private static string appVersion;
        private static DeepTrackUser userData;

        private void Start()
        {
            startTime = DateTime.UtcNow.ToString();
            appVersion = Application.version;
            if (string.IsNullOrEmpty(appVersion)) appVersion = "1.0";
            appVersion = Application.version.Replace(".", "_");
        }

        public static DeepTrackUser LoadUserData()
        {
            if (!IsFileExist(DeepTrackConstants.FILE_NAME))
            {
                CreateNewUserData();
            }
            else
            {
                userData = DeserializeObjectFromFile<DeepTrackUser>(DeepTrackConstants.FILE_NAME);
                if (userData == null || userData.id == null || userData.id == "")
                {
                    CreateNewUserData();
                }
            }
            return userData;
        }

        public static void SaveLocal(DeepTrackUser user)
        {
            userData = user;
            string json = JsonConvert.SerializeObject(userData);
            string path = FileUtilities.GetWritablePath(DeepTrackConstants.FILE_NAME);
            FileUtilities.SaveFile(System.Text.Encoding.UTF8.GetBytes(json), path, true);
        }

        public static void SaveServer(DeepTrackUser user)
        {
            userData = user;
            if (userData == null || userData.id == null || userData.id == "") CreateNewUserData();

            string json = JsonConvert.SerializeObject(userData);
            if (databaseRef == null) return;
            databaseRef.Child("userdata").Child(appVersion).Child(userData.id).SetRawJsonValueAsync(json).ContinueWithOnMainThread(task =>
            {
                if (task.Status != System.Threading.Tasks.TaskStatus.RanToCompletion)
                {
                    Debug.LogError($"save to server failed with status {task.Status}, exception = {task.Exception}");
                }
            });
        }

        static void CreateNewUserData()
        {
            userData = new DeepTrackUser();
            userData.id = GetRandomUserKey();
            userData.startPlayDay = new TimeSpan(DateTime.UtcNow.Ticks).TotalDays;
            SaveLocal(userData);
        }

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

        static T DeserializeObjectFromFile<T>(string fileName, string password = null, bool isAbsolutePath = false)
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
            string GetRandomString(int length)
            {
                string rand = "";
                for (int i = 0; i < length; i++)
                {
                    rand += DeepTrackConstants.ALPHABET[UnityEngine.Random.Range(0, DeepTrackConstants.ALPHABET.Length)];
                }
                return rand;
            }

            string appVersion = Application.version.Replace(".", "_");
            string datetime = DateTime.Now.ToString("yyMMdd");
            string installerName = Application.installerName.Replace(".", "_");
            string key = datetime + "_" + appVersion + "_" + Application.systemLanguage + "_" + GetRandomString(4);
            if (installerName == "com.android.vending")
            {
                key += "_user";
            }
            return key;
        }

        public static void SaveActions(List<string> actions)
        {
            if (actions.Count > DeepTrackConstants.MAX_ACTION_LOG)
            {
                List<string> newActions = new List<string>();
                newActions.Add("...");
                newActions.Add(actions[actions.Count - 2]);
                newActions.Add(actions[actions.Count - 1]);
                actions = newActions;
            }
            DeepTrackSession session = new DeepTrackSession();
            session.id = userData.id;
            session.skippedInter = userData.totalSkippedIntertistial;
            session.language = userData.language;
            session.retentionType = userData.retentionType;
            session.totalPlayDay = userData.totalPlayDay;
            session.totalTime = userData.totalPlayTime;
            session.sessionTime = Time.time;
            session.startTime = startTime;
            session.actions = actions;
            session.installer = Application.installerName;

            //save server
            string json = JsonConvert.SerializeObject(session);
            if (databaseRef == null) return;
            databaseRef.Child("session").Child(appVersion).Child(userData.id).Child(userData.sessionCount.ToString()).SetRawJsonValueAsync(json).ContinueWithOnMainThread(task =>
            {
                Debug.Log("Run finish, result = " + task.Status);
                Debug.Log("Exception = " + task.Exception);
            });
        }
    }
}