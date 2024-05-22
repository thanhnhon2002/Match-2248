using DG.Tweening;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;

public class MoveBenzier : MonoBehaviour
{
    public Transform onePos;
    public Transform twoPos;
    Transform threePos;
    public static float timeDafault = 1.2f;
    float time;
    Vector3 towPosRandom;
    Vector3 firstTarget;
    Vector3 secondTarget;
    Vector3 thirdTarget;

    Quaternion quaternion;
    float timeCount;
    private void OnEnable()
    {
        SetData();
        StartCoroutine(IMove());
    }
    private void SetData()
    {
        timeCount = 0;
        threePos = UIManager.Instance.uiReward;
        time = Random.Range(timeDafault - 0.5f, timeDafault + 0.2f);
        quaternion = Quaternion.Euler(0, 0, Random.Range(-180, 90));
        towPosRandom = (quaternion * (twoPos.position - onePos.position) + onePos.position) * Random.Range(0.6f, 1.2f);
        //transform.DOPath(firstTarget,4,PathType.CatmullRom);
    }

    private IEnumerator IMove()
    {
        while (timeCount <= time)
        {
            timeCount += Time.deltaTime;
            firstTarget = Vector3.Lerp(onePos.position, towPosRandom, timeCount);
            secondTarget = Vector3.Lerp(towPosRandom, threePos.position, timeCount);
            thirdTarget = Vector3.Lerp(firstTarget, secondTarget, timeCount);
            transform.position = thirdTarget;
            yield return null;
        }
        gameObject.SetActive(false);
    }
}
