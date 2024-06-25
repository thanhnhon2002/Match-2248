using UnityEngine;
using TMPro;
using System.Collections.Generic;
using DarkcupGames;
using DG.Tweening;

public class EffectCongratulation : MonoBehaviour
{
    public const float EFFECT_SOUND_VOLUMNE = 0.3f;

    [SerializeField] private List<TextMeshPro> txtEffects;
    [SerializeField] private List<Color> colors;
    [SerializeField] private TextShadow txtShadow;
    [SerializeField] private AudioClip congratulationSound;
    [SerializeField] private Transform startPos;
    [SerializeField] private Transform midPos;
    [SerializeField] private Transform endPos;
    public float moveUpTime = 0.5f;
    public float delayTime = 0.5f;

    private readonly List<string> congratulationTexts = new List<string>()
    {
        "Great",
        "Perfect",
        "Awesome",
        "Fantastic",
        "Excellent",
        "Impressive",
        "Bravo",
        "Outstanding",
        "Amazing",
        "Superb",
        //"Combo Master",
        //"Chain Champion",
        //"Unstoppable",
        //"Mega Chain",
        //"Endless Streak",
        //"Infinity Chain",
        //"Relentless",
        //"Epic Sequence",
        //"Continuous Combo",
        //"Ultimate Chain",
        //"Never-ending",
        //"Chain Reaction",
        //"Flawless Streak",
        "Combo King"
    };

    private void OnEnable()
    {
        int rand = Random.Range(0, txtEffects.Count);
        for (int i = 0; i < txtEffects.Count; i++)
        {
            txtEffects[i].gameObject.SetActive(i == rand);
            if (i == rand)
            {
                txtShadow.SetColor(colors[i]);
                ShowEffect(txtEffects[i], congratulationTexts[Random.Range(0, congratulationTexts.Count)].ToUpper());
            }
        }
    }

    public void ShowEffect(TextMeshPro comboTxt, string content)
    {
        comboTxt.text = content;
        AudioSystem.Instance.PlaySound(congratulationSound, EFFECT_SOUND_VOLUMNE);
        txtShadow.SetTarget(comboTxt);

        var color = Color.white;
        color.a = 0f;
        comboTxt.DOKill();
        comboTxt.color = color;
        comboTxt.DOFade(1f, Const.DEFAULT_TWEEN_TIME);
        comboTxt.gameObject.SetActive(true);
        comboTxt.transform.position = startPos.transform.position;
        comboTxt.transform.DOMove(midPos.transform.position, moveUpTime);

        

        LeanTween.delayedCall(moveUpTime + delayTime, () =>
        {
            comboTxt.transform.DOMove(endPos.transform.position, moveUpTime);
            comboTxt.DOFade(0f, moveUpTime).OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
        });
    }
}