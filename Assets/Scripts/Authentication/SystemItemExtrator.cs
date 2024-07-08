using UnityEngine;

public class SystemItemExtrator: MonoBehaviour
{
    private void Awake()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            transform.GetChild(i).SetParent(null);
        }
    }
}