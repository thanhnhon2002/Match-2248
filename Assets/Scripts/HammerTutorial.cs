using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class HammerTutorial : PowerTutorial
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private float changeInterval;
    private float lastChangeTime;
    private int index;
    private bool run;

    private void OnEnable()
    {
        var color = Color.white;
        spriteRenderer.color = color;
        spriteRenderer.DOFade(1f, 0.5f).OnComplete(DoTutorial);
    }

    private void Update()
    {
        if (!run) return;
        if(Time.time - lastChangeTime > changeInterval)
        {
            index = index == 0 ? 1 : 0;
            spriteRenderer.sprite = sprites[index];
            lastChangeTime = Time.time;
        }
    }

    public override void DoTutorial()
    {
        var allCells = GridManager.Instance.GetComponentsInChildren<Cell>().ToList();
        allCells.Sort((a, b) => a.Value.CompareTo(b.Value));
        transform.DOMove(allCells[0].transform.position, 2f).OnComplete(() => run = true);     
    }
}
