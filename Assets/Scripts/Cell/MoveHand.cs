using UnityEngine;
using DG.Tweening;
using System.Collections;

public class MoveHand : MonoBehaviour
{
    CellTutorial[] cells;
    float speed=0.8f;

    private void Awake()
    {
        cells = transform.transform.GetComponentsInChildren<CellTutorial>(true);
    }
    private void OnEnable()
    {
        Tutorial.instance.hand.transform.DOComplete();
        Tutorial.instance.hand.SetActive(false);
        Invoke(nameof(StartAnimation),0.5f); 
    }
    private void StartAnimation()
    {
        StartCoroutine(Animation());
    }
    IEnumerator Animation()
    {
        Tutorial.instance.hand.transform.DOComplete();
        Tutorial.instance.hand.transform.position = cells[0].transform.position;
        Tutorial.instance.hand.SetActive(true);
        for (int cell=1;cell<cells.Length;cell++)
        {
            Tutorial.instance.hand.transform.DOMove(cells[cell].transform.position, speed).SetEase(Ease.Linear);
            yield return new WaitForSeconds(speed);
        }
        yield return new WaitForSeconds(0.25f);
        Tutorial.instance.hand.SetActive(false);          
        yield return new WaitForSeconds(0.55f);
        yield return StartCoroutine(Animation());
    }
    private void OnDisable()
    {
        Tutorial.instance.hand.transform.DOComplete();
        Tutorial.instance.hand.SetActive(false);
    }
}