using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Numerics;
using DarkcupGames;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using NSubstitute.Core;

public class CellTutorial : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerClickHandler
{
    public SpriteRenderer spriteRenderer { get; private set; }
    public TextMeshPro valueTxt;
    public ColorSet colorSet;
    private BigInteger value;
    public BigInteger Value
    {
        get { return value; }
        set
        {
            this.value = value;
            valueTxt.text = BigIntegerConverter.ConverNameValue(value);
            colorSet.SetColor();
        }
    }
    public int valueTutorial;
    private bool added;
    private BounceOnClick bounceClick;
    private CellInteractEffect cellInteractEffect;
    private static bool isDragging;
    private static BigInteger initValue;
    private static int countInitValue;
    private static List<CellTutorial> listCell = new List<CellTutorial>();
    private static List<LineRenderer> lines = new List<LineRenderer>();
    private static List<int> multipliers = new List<int>();
    [SerializeField] private LineRenderer linePrefab;
    [SerializeField] private Effect effectPrefab;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        colorSet = GetComponent<ColorSet>();
        bounceClick = GetComponent<BounceOnClick>();
        cellInteractEffect = GetComponent<CellInteractEffect>();
        InitMultipliers();
    }

    private void Start()
    {
        Value = (BigInteger)valueTutorial;
    }

    private void EffectChose()
    {
        bounceClick.Bounce();
        cellInteractEffect.PlaySound();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        added = true;
        listCell.Add(this);
        initValue = value;
        var line = PoolSystem.Instance.GetObject(linePrefab, transform.position);
        lines.Add(line);
        line.SetPosition(0, transform.position);
        line.SetColors(spriteRenderer.color, spriteRenderer.color);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        CellTutorial[] cells = transform.parent.GetComponentsInChildren<CellTutorial>();
        if(listCell.Count < cells.Length)
        {
            foreach (var cell in listCell)
                cell.added = false;
            listCell.Clear();
            foreach (var line in lines)
                line.gameObject.SetActive(false);
            lines.Clear();
            isDragging = false;
            Tutorial.instance.Fail();
            return;
        }
        isDragging = false;
        if (listCell.Count == 1)
        {
            listCell.Clear();
            lines[0].gameObject.SetActive(false);
            lines.Clear();
            added = false;
            return;
        }
        lines.Last().SetPosition(1, listCell.Last().transform.position);
        ExplodeConnectedCell();
    }

    public void OnDrag(PointerEventData eventData)
    {
        lines.Last().SetPosition(1, Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        EffectChose();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isDragging && !added && CanConnect())
        {
            added = true;
            EffectChose();
            listCell.Add(this);
            countInitValue += (int)(value / initValue);
            lines.Last().SetPosition(1, transform.position);
            var line = PoolSystem.Instance.GetObject(linePrefab, transform.position, transform.parent);
            lines.Add(line);
            line.SetPosition(0, transform.position);
            line.SetColors(spriteRenderer.color, spriteRenderer.color);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        EffectChose();
    }

    private bool CanConnect()
    {
        if (Mathf.Abs(transform.GetSiblingIndex() - listCell.Last().transform.GetSiblingIndex()) > 1)
            return false;

        if (value < listCell.Last().value)
            return false;
        else if (value > listCell.Last().value
            && listCell.Count > 1
            && listCell[0].value == listCell[1].value
            && value / 2 == listCell.Last().value)
            return true;
        if (value == listCell.Last().value)
            return true;
        return false;
    }

    private void ExplodeConnectedCell()
    {
        foreach (var cell in listCell)
        {
            cell.gameObject.SetActive(false);
            var fx = PoolSystem.Instance.GetObject(effectPrefab, cell.transform.position);
            fx.Play(listCell, listCell.IndexOf(cell), cell.spriteRenderer.color, listCell.Last().spriteRenderer.color);
        }
        foreach (var line in lines)
            line.gameObject.SetActive(false);
        DOVirtual.DelayedCall(0.5f, () =>
        {
            foreach (var cell in listCell)
                cell.added = false;
            listCell.Last().gameObject.SetActive(true);
            CheckNextPart(listCell.Last());
            listCell.Last().Value = CalculateTotal(initValue, countInitValue);
            listCell.Clear();
            lines.Clear();
            cellInteractEffect.PlaySound();
            countInitValue = 0;
        });
    }

    private static BigInteger CalculateTotal(BigInteger initValue, int cellCount)
    {
        return initValue * (BigInteger)Mathf.Pow(2, IndexCellCount(cellCount) + 1);
    }

    private static int IndexCellCount(int cellCount)
    {
        if (cellCount == 0)
            return -1;
        for (var index = 0; index <= 30; index++)
        {
            if (cellCount == multipliers[index])
                return index;
            if (multipliers[index + 1] > cellCount && multipliers[index] <= cellCount)
                return index;
        }
        return multipliers.Count - 1;
    }

    private static void InitMultipliers()
    {
        for (int i = 0; i <= 30; i++)
        {
            var pow = Mathf.Pow(2, i);
            multipliers.Add((int)pow);
        }
    }

    private void CheckNextPart(CellTutorial cell)
    {
        CellTutorial[] cells = cell.transform.parent.GetComponentsInChildren<CellTutorial>();
        if (cells.Length <= 2)
            Tutorial.instance.NextPart();
    }
}
