using DarkcupGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class IntertisialButton : MonoBehaviour
{
    [SerializeField] private UnityEvent OnInterDone;
    [SerializeField] private string placement;

    public void OnClick()
    {
        AdManagerMax.Instance.ShowIntertistial(placement, () => OnInterDone?.Invoke());
    }
}
