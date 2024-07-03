using TMPro;
using UnityEngine;

public class InputFieldSearchFriend : MonoBehaviour
{
    private TMP_InputField inputField;

    private void Awake()
    {
        inputField = GetComponentInChildren<TMP_InputField>();
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
