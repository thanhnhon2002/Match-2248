using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InputFieldSearchFriend : MonoBehaviour
{
    private InputField inputField;

    private void Awake()
    {
        inputField = GetComponent<InputField>();
        if (inputField != null)
        {
            inputField.onValueChanged.AddListener(OnInputFieldValueChanged);
        }
    }
    private async void OnInputFieldValueChanged(string newValue)
    {
        await FriendManager.Instance.SearchUser(newValue);
    }
}
