using UnityEngine;
using TMPro;

public abstract class SetTextPanel : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI[] texts;
    public virtual void SetText() { }
}
