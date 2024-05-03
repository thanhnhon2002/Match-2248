using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonGroup : MonoBehaviour
{
    private Button[] buttons;

    private void Awake ()
    {
        buttons = GetComponentsInChildren<Button>();
    }

    public void SetInteract(bool interactable)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = interactable;
        }
    }
}
