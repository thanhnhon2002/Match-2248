using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Power : MonoBehaviour
{
    [SerializeField] protected CanvasGroup displayGroup;

    public virtual void UsePower()
    {
        GameFlow.Instance.bottomGroup.SetActive (false);
        displayGroup.gameObject.SetActive (true);
    }
}
