using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class DataRankManager : MonoBehaviour
{
    [ContextMenu("Get Data Rank")]
    public static async Task GetRankGlobal()
    {
        try
        {
            var dataSnapshot = await ServerSystem.databaseRef.Child(ServerSystem.RANK_DATA_URL).GetValueAsync();
            if(dataSnapshot == null) Debug.LogWarning("No data found.");
            var json = dataSnapshot.GetRawJsonValue();
            ServerSystem.rank.topTenRank = JsonConvert.DeserializeObject<List<UserDataServer>>(json);
        } catch (Exception ex)
        {
            Debug.LogError($"Error: {ex.Message}");
        }
    }
}
