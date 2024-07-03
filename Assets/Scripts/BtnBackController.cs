using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnBackController : MonoBehaviour
{
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClickBtn);
    }

    private void OnClickBtn()
    {
        GetComponentInParent<EffectOnClose>().Close();
        MenuOptions.Instance.ShowDefaultOption();
    }
}
