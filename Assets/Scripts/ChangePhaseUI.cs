using UnityEngine;
using UnityEngine.UI;

public class ChangePhaseUI : MonoBehaviour
{
    public Sprite[] sprites;
    int index;
    public void ChangePhase()
    {
        if (index + 1 == sprites.Length) index = 0;
        else index += 1;
        GetComponent<Image>().sprite = sprites[index];
    }
}
