using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EffectSlideManager : MonoBehaviour
{
    // sap xep theo thu tu de animation slide hoat dong dung
    public List<GameObject> slidePanelList = new List<GameObject>();
    public GameObject theOpenOne;
    public GameObject theCloseOne;

    public void SetSlideTheOpenOne(GameObject theOpenOne)
    {
        theCloseOne = this.theOpenOne;
        this.theOpenOne = theOpenOne;

        OnSlideChange();
    }

    private void OnSlideChange()
    {
        int iTheOpenOne = 0;
        int iTheCloseOne = 0;
        for (int i = 0; i < slidePanelList.Count; i++)
        {
            if (slidePanelList[i] == theOpenOne)
            {
                iTheOpenOne = i;
            }
            if (slidePanelList[i] == theCloseOne)
            {
                iTheCloseOne = i;
            }
        }
        if (iTheOpenOne > iTheCloseOne)
        {
            theOpenOne.GetComponent<EffectOpenSlide>().DoEffect(false);
            theCloseOne.GetComponent<EffectCloseSlide>().Close(false);
        }
        if (iTheOpenOne < iTheCloseOne)
        {   
            theOpenOne.GetComponent<EffectOpenSlide>().DoEffect(true);
            theCloseOne.GetComponent<EffectCloseSlide>().Close(true);
        }
    }
}
