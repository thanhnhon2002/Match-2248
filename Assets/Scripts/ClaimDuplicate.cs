using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClaimDuplicate : MonoBehaviour
{
    [SerializeField] private float cost;
    public void OnClick()
    {
        var userData = GameSystem.userdata;
        if (userData.diamond < cost)
        {
            GameFlow.Instance.shop.SetActive(true);
            GameFlow.Instance.shop.GetComponent<EffectOpenSlide>().DoEffect();
            GetComponent<Button>().enabled = true;
            return;
        }
        GameFlow.Instance.diamondGroup.AddDiamond((int)-cost);
        GameFlow.Instance.diamondGroup.Display();
        GameFlow.Instance.popupAnimComboManager.OpenComboAnimQueue();
        GridManager.Instance.DoubleHightCellValue();
        PopupManager.Instance.DeQueue();
        GameSystem.SaveUserDataToLocal();
        DataUserManager.SaveUserData();
    }
}
