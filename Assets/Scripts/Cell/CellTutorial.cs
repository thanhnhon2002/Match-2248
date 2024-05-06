using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Numerics;
using DarkcupGames;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;

public class CellTutorial : MonoBehaviour,IPointerDownHandler ,IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerClickHandler
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
    private static List<int> multiliers = new List<int>();
    [SerializeField] private LineRenderer linePrefab;
    [SerializeField] private Effect effectPrefab;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        colorSet = GetComponent<ColorSet>();
        bounceClick = GetComponent<BounceOnClick>();
        cellInteractEffect = GetComponent<CellInteractEffect>();
        InitMultilier();
    }
    void Start()
    {
        Value = (BigInteger)valueTutorial;
    }
    void EffectChose()
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
        var line = PoolSystem.Instance.GetObject(linePrefab, this.transform.position);
        lines.Add(line);
        line.SetPosition(0, transform.position);
        line.SetColors(spriteRenderer.color, spriteRenderer.color);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        if(listCell.Count == 1)
        {
            listCell.Clear();
            lines[0].gameObject.SetActive(false);
            lines.Clear();
            added = false;
            return;
        }
        ExploseConectedCell();
        
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
        if(isDragging && !added&&CanConect())
        {
            added = true;
            EffectChose();
            listCell.Add(this);
            countInitValue += (int) (value / initValue);
            lines.Last().SetPosition(1, transform.position);
            var line = PoolSystem.Instance.GetObject(linePrefab, this.transform.position);
            lines.Add(line);
            line.SetPosition(0, transform.position);
            line.SetColors(spriteRenderer.color, spriteRenderer.color);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        EffectChose();
    }
    private bool CanConect()
    {
        if (value < listCell.Last().value) return false;
        else if(value> listCell.Last().value
            &&listCell.Count>1
            && listCell[0].value == listCell[1].value
            &&value/2==listCell.Last().value) return true;
        if (value == listCell.Last().value) return true;
        return false;
    }
    private void ExploseConectedCell()
    {
        foreach(var cell in listCell)
        {
            var fx = PoolSystem.Instance.GetObject(effectPrefab, cell.transform.position);
            fx.Play(listCell,listCell.IndexOf(cell), cell.spriteRenderer.color, listCell.Last().spriteRenderer.color);    
        }
        DOVirtual.DelayedCall(0.5f, () =>
        {
            foreach (var cell in listCell) cell.gameObject.SetActive(false);
            foreach (var cell in listCell) cell.added = false;
            listCell.Last().gameObject.SetActive(true);
            listCell.Last().Value = CalculateTotal(initValue,countInitValue);
            listCell.Clear();
            foreach (var line in lines) line.gameObject.SetActive(false);
            lines.Clear();
            cellInteractEffect.PlaySound();
            countInitValue = 0;
        });
    }
    BigInteger CalculateTotal(BigInteger initValue, int cellCount)
    {
        return initValue * (BigInteger)Mathf.Pow(2, IndexCellCount(cellCount) + 1);
   
    }
    private int IndexCellCount(int cellCount)
    {
        if (cellCount == 0) return -1;
        for (var index = 0; index <= 30; index++)
        {
            if (cellCount == multiliers[index]) return index;
            if (multiliers[index + 1] > cellCount && multiliers[index] <= cellCount)
                return index;
        }
        return multiliers.Count - 1;
    }
    private void InitMultilier()
    {
        for (int i = 0; i <= 30; i++)
        {
            var pow = Mathf.Pow(2, i);
            multiliers.Add((int)pow);
        }
    }
}
