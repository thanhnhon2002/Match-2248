using DG.Tweening;
using UnityEngine;

public class Hammer : Power
{
    public static Hammer Instance { get; private set; }
    [SerializeField] private Animator hammer;
    [SerializeField] private Transform hamerImg;
    [SerializeField] private ParticalSystemController smashFx;
    private void Awake ()
    {
        Instance = this;
    }
    public override void UsePower ()
    {
        base.UsePower ();
        GameFlow.Instance.gameState = GameState.Smash;
        hammer.SetBool ("Play", true);
    }

    public void Smash(Cell cell)
    {
        hammer.SetBool ("Play", false);
        var destination = GameFlow.Instance.mainCam.WorldToScreenPoint (cell.transform.position);
        var position = hamerImg.position;
        var sq = DOTween.Sequence ();
        sq.AppendCallback (() => 
        {
            hamerImg.DOMove (destination, 1f).OnComplete (() => hammer.Play ("HammerOne"));
        });
        sq.AppendInterval (1.5f);
        sq.AppendCallback (() => 
        {
            RemoveCell (cell);
            displayGroup.DOFade (0f, 1f).OnComplete(() => 
            {
                GameFlow.Instance.bottomGroup.SetActive (true);
                displayGroup.gameObject.SetActive (false);
                displayGroup.alpha = 1f;
            });
        });

    }


    private void RemoveCell (Cell cell)
    {
        var fx = PoolSystem.Instance.GetObject(smashFx, cell.transform.position); 
        fx.ChangeColor(cell.spriteRenderer.color);
        cell.gameObject.SetActive (false);

        var cellsInSameCol = GridManager.Instance.allCellInCollom[cell.gridPosition.x];
        var spawnPos = GridManager.Instance.GetCellAt (new GridPosition (cell.gridPosition.x, 1)).transform.localPosition;
        spawnPos.y++;

        cellsInSameCol.Remove (cell);
        var newCell = GridManager.Instance.SpawnCellNew (spawnPos);
        cellsInSameCol.Add (newCell);

        for (int i = 0; i <= cellsInSameCol.Count; i++)
        {

            cellsInSameCol.Sort ((a, b) => a.transform.localPosition.y.CompareTo (b.transform.localPosition.y));

            GridManager.Instance.ReassignGridPos (cell.gridPosition.x, cellsInSameCol);
        }
        LeanTween.delayedCall(0.5f,() => GridManager.Instance.Drop ());
    }
}
