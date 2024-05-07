using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector2;

public class InfiniteScroll : MonoBehaviour
{
    [SerializeField] private Road roadPre;
    [SerializeField] private Button upButton;
    [SerializeField] private Button downButton;
    [SerializeField] private CanvasGroup group;
    [SerializeField] private Transform content;
    private Road displayRoad;
    public int lastUpdateIndex = 1;

    private void Awake()
    {
        displayRoad = GetComponentInChildren<Road>();
    }

    private void OnEnable()
    {
        group.alpha = 0f;
        group.DOFade(1f, Const.DEFAULT_TWEEN_TIME);
        upButton.interactable = IsNotLowest();
    }

    private void Start()
    {
        StartCoroutine(displayRoad.UpdateDisplay());
        upButton.interactable = IsNotLowest();
    }

    public void Up()
    {
        upButton.interactable = false;
        downButton.interactable = false;
        var newRoad = PoolSystem.Instance.GetObject(roadPre, Vector3.zero);
        newRoad.transform.SetParent(content, false);
        newRoad.transform.localScale = Vector3.one;
        newRoad.rectTransform.anchoredPosition = new Vector2(0, -displayRoad.rectTransform.rect.height);
        newRoad.rectTransform.DOAnchorPosY(0, Const.DEFAULT_TWEEN_TIME);
        displayRoad.rectTransform.DOAnchorPosY(displayRoad.rectTransform.rect.height, Const.DEFAULT_TWEEN_TIME).OnComplete(() =>
        {
            displayRoad.gameObject.SetActive(false);
            displayRoad = newRoad;
            lastUpdateIndex -= newRoad.checkPoints.Length *2;
            StartCoroutine(newRoad.UpdateDisplay());
            upButton.interactable = IsNotLowest();
            downButton.interactable = true;
        });
    }

    public void Down()
    {
        upButton.interactable = false;
        downButton.interactable = false;
        var newRoad = PoolSystem.Instance.GetObject(roadPre, Vector3.zero);
        newRoad.transform.SetParent(content, false);
        newRoad.transform.localScale = Vector3.one;
        newRoad.rectTransform.anchoredPosition = new Vector2(0, displayRoad.rectTransform.rect.height);
        newRoad.rectTransform.DOAnchorPosY(0, Const.DEFAULT_TWEEN_TIME);
        displayRoad.rectTransform.DOAnchorPosY(-displayRoad.rectTransform.rect.height, Const.DEFAULT_TWEEN_TIME).OnComplete(() =>
        {
            displayRoad.gameObject.SetActive(false);
            displayRoad = newRoad;
            StartCoroutine(newRoad.UpdateDisplay());
            upButton.interactable = true;
            downButton.interactable = true;
        });
    }

    private bool IsNotLowest() => displayRoad.checkPoints.All(x => x.dislayValue > 2);

    public void Close()
    {
        group.DOFade(0f, Const.DEFAULT_TWEEN_TIME).OnComplete(() => gameObject.SetActive(false));
    }
}
