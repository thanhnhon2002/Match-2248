using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
[RequireComponent(typeof(CanvasGroup))]
public class TextShadow : MonoBehaviour
{
    public TextMeshProUGUI txtTarget;
    private TextMeshProUGUI txt;
    private CanvasGroup canvasGroup;
    public Vector3 offset;

    private void Awake()
    {
        txt = GetComponent<TextMeshProUGUI>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        canvasGroup.alpha = 0;
    }

    private void Update()
    {
        transform.position = txtTarget.transform.position + offset;
        canvasGroup.alpha = txtTarget.color.a;
    }

    public void SetTarget(TextMeshProUGUI txtTarget)
    {
        this.txtTarget = txtTarget;
        txt.text = txtTarget.text;
    }

    public void SetColor(Color color)
    {
        txt.color = color;
    }
}