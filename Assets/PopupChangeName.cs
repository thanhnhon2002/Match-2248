using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class PopupChangeName : MonoBehaviour
{
    [SerializeField] private TMP_InputField name;
    private AvatarButton[] avatarButtons;
    private int chosenAvatarIndex;
    private void Awake()
    {
        avatarButtons = GetComponentsInChildren<AvatarButton>();
    }

    private void OnEnable()
    {
        transform.localScale = Vector3.one;
        var userData = GameSystem.userdata;
        name.text = userData.nickName;
        ChooseAvatar(userData.avatarIndex);
    }

    public void ChooseAvatar(int index)
    {
        chosenAvatarIndex = index;
        for (int i = 0; i < avatarButtons.Length; i++)
        {
            avatarButtons[i].transform.localScale = Vector3.one;
            avatarButtons[i].transform.SetSiblingIndex(avatarButtons[i].index);
        }
        avatarButtons[index].transform.SetAsLastSibling();
        avatarButtons[index].transform.DOScale(1.3f, Const.DEFAULT_TWEEN_TIME);
    }

    public void Comfirm()
    {
        var userData = GameSystem.userdata;
        userData.avatarIndex = chosenAvatarIndex;
        userData.nickName = name.text;
        GameSystem.SaveUserDataToLocal();
    }

    public void Close()
    {
        transform.DOScale(0f, Const.DEFAULT_TWEEN_TIME).OnComplete(() => gameObject.SetActive(false));
    }
}
