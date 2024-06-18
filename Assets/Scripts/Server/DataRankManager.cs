using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataRankManager : MonoBehaviour
{
    [ContextMenu("Get Data Rank")]
    public async void GetRankGlobal()
    {
        try
        {
            var dataSnapshot = await ServerSystem.databaseRef.Child(ServerSystem.RANK_DATA_URL).GetValueAsync();
            if (dataSnapshot != null)
            {
                var json = dataSnapshot.GetRawJsonValue();
                ServerSystem.rank = JsonConvert.DeserializeObject<Rank>(json);
            }
            else
            {
                Debug.LogWarning("No data found.");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error: {ex.Message}");
        }
    }
}
