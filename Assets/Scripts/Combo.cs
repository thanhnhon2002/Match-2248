using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using DarkcupGames;

public class Combo : MonoBehaviour
{
    private const int MIN_COMBO_CONGRATULATION = 6;

    private const string COMBO_TEXT = "Combo x";
    [SerializeField] private TextMeshProUGUI comboTxt;
    private float lifeTime;
    [SerializeField] private float moveSpeed;
    [SerializeField] private AudioClip comboSound;
    [SerializeField] private EffectCongratulation effectCongratulation;

    private bool alive = true;

    private void OnEnable()
    {
        alive = true;
    }

    private void Update()
    {
        var pos = transform.position;
        pos.y += moveSpeed * Time.deltaTime;
        transform.position = pos;
        lifeTime -= Time.deltaTime;

        if (lifeTime < 0 && alive == true)
        {
            alive = false;
            comboTxt.DOFade(0f, Const.DEFAULT_TWEEN_TIME).OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
        }
    }

    public void ShowCombo(int amount, Vector3 position, out float effectTime, float lifeTime = 1f)
    {
        EasyEffect.Appear(gameObject, 1f, 1f, speed: 0.15f);
        AudioSystem.Instance.PlaySound(comboSound);
        effectTime = lifeTime;
        var color = Color.white;
        color.a = 0f;
        comboTxt.color = color;
        comboTxt.DOFade(1f, Const.DEFAULT_TWEEN_TIME);
        var rectTransform = (RectTransform)transform;
        rectTransform.position = position;
        this.lifeTime = lifeTime;
        gameObject.SetActive(true);
        comboTxt.text = $"{COMBO_TEXT}{amount}";

        if (amount >= MIN_COMBO_CONGRATULATION)
        {
            LeanTween.delayedCall(1f, () =>
            {
                effectCongratulation.gameObject.SetActive(true);
            });
        }
    }
}
