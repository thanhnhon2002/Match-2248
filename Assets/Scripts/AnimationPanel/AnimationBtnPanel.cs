using UnityEngine;
using UnityEngine.UI;

public class AnimationBtnPanel : MonoBehaviour
{
    public GameObject boder;
    Button button;
    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClickBtn);
    }
    public void OnClickBtn()
    {
        boder.transform.position = transform.position;
    }
}