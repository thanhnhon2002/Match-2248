using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using DarkcupGames;

public class Combo : MonoBehaviour
{
    private const string COMBO_TEXT = "Combo x";
    [SerializeField] private TextMeshProUGUI comboTxt;
    private float lifeTime;
    [SerializeField] private float moveSpeed;
    [SerializeField] private AudioClip comboSound;

    private void Update ()
    {
        var pos = transform.position;
        pos.y -= moveSpeed * Time.deltaTime;
        transform.position = pos;
        lifeTime -= Time.deltaTime;
        gameObject.SetActive(lifeTime > 0);
    }

    public void ShowCombo (int amount, Vector3 position, out float effectTime, float lifeTime = 2f)
    {
        AudioSystem.Instance.PlaySound (comboSound);
        effectTime = lifeTime;
        var color = Color.white;
        color.a = 0f;
        comboTxt.color = color;
        comboTxt.DOFade (1f, Const.DEFAULT_TWEEN_TIME);
        var rectTransform = (RectTransform)transform;
        rectTransform.position = position;
        this.lifeTime = lifeTime;
        gameObject.SetActive(true);
        comboTxt.text = $"{COMBO_TEXT}{amount}";
    }
}
