using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshPro))]
public class TextShadow : MonoBehaviour
{
    public TextMeshPro txtTarget;
    private TextMeshPro txt;
    public Vector3 offset;
    private Color color;

    private void Awake()
    {
        txt = GetComponent<TextMeshPro>();
    }

    private void Update()
    {
        transform.position = txtTarget.transform.position + offset;
        color.a = txtTarget.color.a;
        txt.color = color;
    }

    public void SetTarget(TextMeshPro txtTarget)
    {
        this.txtTarget = txtTarget;
        txt.text = txtTarget.text;
    }

    public void SetColor(Color color)
    {
        color.a = 0;
        txt.color = color;
        this.color = color;
    }
}