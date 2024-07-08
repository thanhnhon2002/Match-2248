using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        if (SceneManager.GetActiveScene().name == "GameplayUI")
        {
            GetComponentInParent<EffectCloseSlide>().Close(false);
        }
        else
        {
            GetComponentInParent<EffectSlideManager>().SetSlideTheOpenOne(MenuOptions.Instance.dicMenuOptions[OptionMenu.Home]);
            MenuOptions.Instance.ShowDefaultOption();

        }
    }
}
