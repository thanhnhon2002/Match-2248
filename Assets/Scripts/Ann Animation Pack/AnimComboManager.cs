using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// this enum contain name of gameobject that have AnimCombo component to find and run the AnimCombo
/// this enum below should contain name of gameobject that have AnimCombo in List<AnimCombo> animComboList before using or it wont work
/// </summary>
public enum AnimComboName
{
    PopupPause,
    NoInternetConnectionPopup,
    PopupMakeSure
}
/// <summary>
/// this class using enum AnimComboName to find and run AnimCombo class
/// </summary>
public class AnimComboManager : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    public List<AnimCombo> animComboList = new List<AnimCombo>();

    /// <summary>
    /// get all children that have AnimCombo to use.
    /// </summary>
    [ContextMenu("Update AnimComboList")]
    public void SetAnimComboList()
    {
        animComboList.Clear();
        animComboList = GetComponentsInChildren<AnimCombo>(true).ToList();
        QuerySameNameObject();
    }

    private void QuerySameNameObject()
    {
        var query = animComboList.GroupBy(animCombo => animCombo.name).Where(group => group.Count() > 1);
        if (query.Any())
        {
            animComboList.Clear();
            foreach (var group in query)
            {
                Debug.LogError($"Duplicate GameObject name: {group.Key}");
            }
            Debug.LogError("Some children gameobject have the same name, please rename it and try again");
        }
    }

    public void OpenCombo(AnimComboName animComboName)
    {
        try
        {
            var convertName = animComboName.ToString();
            var query = animComboList.First(animCombo => convertName.Contains(animCombo.gameObject.name));
            if (query != null) query.OpenAnim();
        }
        catch
        {
            Debug.LogError("Something wrong, make sure animComboList & AnimComboGameObjectName have same gameobject name");
        }

    }

    public void CloseCombo(AnimComboName animComboName)
    {
        try
        {
            var convertName = animComboName.ToString();
            var query = animComboList.First(animCombo => convertName.Contains(animCombo.gameObject.name));
            if (query != null) query.CloseAnim();
        }
        catch
        {
            Debug.LogError("Something wrong, make sure animComboList & AnimComboGameObjectName have same gameobject name");
        }

    }

    public AnimCombo GetAnimCombo(AnimComboName animComboName)
    {
        try
        {
            var convertName = animComboName.ToString();
            var query = animComboList.First(animCombo => convertName.Contains(animCombo.gameObject.name));
            return query;
        }
        catch
        {
            Debug.LogError("Something wrong, make sure animComboList & AnimComboGameObjectName have same gameobject name");
            return null;
        }
    }
}