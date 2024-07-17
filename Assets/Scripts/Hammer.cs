using DarkcupGames;
using DG.Tweening;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Hammer : Power<Hammer>
{
    [SerializeField] private Animator hammer;
    [SerializeField] private RectTransform hamerImg;
    [SerializeField] private ParticalSystemController smashFx;
    [SerializeField] private AudioClip cellSmashSound;
    [SerializeField] private CellHighlight highlightPre;
    [SerializeField] private HammerTutorial tutorialPrefab;
    private HammerTutorial tutorial;
    private bool chose;
    private Cell cell;
    public override void UsePower ()
    {
        if (GameFlow.Instance.gameState != GameState.Playing) return;
        if (GameSystem.userdata.diamond < cost)
        {
            GameFlow.Instance.shop.SetActive(true);
            GameFlow.Instance.shop.GetComponent<EffectOpenSlide>().DoEffect();
            return;
        }
        cell = null;
        base.UsePower ();
        if (!info.isTutorialFinish)
        {
            tutorial = Instantiate(tutorialPrefab, Vector3.zero, UnityEngine.Quaternion.identity);
            tutorial.DoTutorial();
            backButton.gameObject.SetActive(false);
        }
        GameFlow.Instance.gameState = GameState.Smash;
        hammer.SetBool ("Play", true);
        chose = false;
    }

    public override void UsePowerIgnoreCost()
    {
        base.UsePowerIgnoreCost ();
        cell = null;
        base.UsePower();
        GameFlow.Instance.gameState = GameState.Smash;
        hammer.SetBool("Play", true);
        chose = false;
    }

    public void Smash(Cell cell)
    {
        if (this.cell != null) return;
        if (tutorial != null) tutorial.gameObject.SetActive(false);
        this.cell = cell;
        var highligt = PoolSystem.Instance.GetObject(highlightPre, cell.transform.position);
        highligt.cell = cell;
        if (!ignoreCost) GameFlow.Instance.diamondGroup.AddDiamond((int)-cost);
        GameFlow.Instance.diamondGroup.Display();
        info.isTutorialFinish = true;
        GameSystem.SaveUserDataToLocal();
        DataUserManager.SaveUserData();
        if (chose) return;
        chose = true; 
        backButton.gameObject.SetActive (false); 
        hammer.SetBool ("Play", false);
        var destination = GameFlow.Instance.mainCam.WorldToScreenPoint (cell.transform.position);
        var sq = DOTween.Sequence ();
        sq.AppendCallback (() => 
        {
            hamerImg.DOMove (destination, 1f).OnComplete (() => hammer.Play ("HammerOne"));
        });
        sq.AppendInterval (1.5f);
        sq.AppendCallback (() => 
        {
            RemoveCell (cell);
            highligt.gameObject.SetActive(false);
            displayGroup.DOFade (0f, 1f).OnComplete(() => 
            {
                onUseCompleted?.Invoke();
                hamerImg.anchoredPosition = Vector3.zero;
                GameFlow.Instance.bottomGroup.gameObject.SetActive (true);
                GameFlow.Instance.topGroup.gameObject.SetActive (true);
                displayGroup.gameObject.SetActive (false);
                displayGroup.alpha = 1f;

            });
        });

    }


    private void RemoveCell (Cell cell)
    {
        var collomIndex = cell.gridPosition.x;
        var cellsInSameCol = GridManager.Instance.allCellInCollom[collomIndex];
        var fx = PoolSystem.Instance.GetObject(smashFx, cell.transform.position); 
        fx.ChangeColor(cell.spriteRenderer.color);
        cell.gameObject.SetActive (false);
        AudioSystem.Instance.PlaySound (cellSmashSound);
        var spawnPos = GridManager.Instance.GetCellAt (new GridPosition (collomIndex, 1)).transform.position;
        spawnPos.y++;

        cellsInSameCol.Remove (cell);
        var newCell = GridManager.Instance.SpawnCellNew (spawnPos);
        cellsInSameCol.Add (newCell);

        cellsInSameCol.Sort ((a, b) => a.transform.localPosition.y.CompareTo (b.transform.localPosition.y));

        GridManager.Instance.ReassignGridPos (collomIndex, cellsInSameCol);
        LeanTween.delayedCall (0.5f, () => GridManager.Instance.Drop ());
    }
}
