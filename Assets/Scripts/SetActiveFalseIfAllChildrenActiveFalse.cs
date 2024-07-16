using System.Collections.Generic;
using UnityEngine;

public class SetActiveFalseIfAllChildrenActiveFalse : MonoBehaviour
{
    public List<Transform> children;

    private void Update()
    {
        for (int i = 0; i < children.Count; i++)
        {
            if (children[i].gameObject.activeInHierarchy) return;
        }
        gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        for (int i = 0; i < children.Count; i++)
        {
             children[i].gameObject.SetActive(true);
        }
    }
}