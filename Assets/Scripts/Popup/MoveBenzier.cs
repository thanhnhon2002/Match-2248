using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;

public class MoveBenzier : MonoBehaviour
{
    public Transform onePos;
    public Transform twoPos;
    Transform threePos;
    public static float timeDafault = 1f;
    float time;
    Vector3 towPosRandom;
    Vector3 firstTarget;
    Vector3 secondTarget;
    Vector3 thirdTarget;

    Quaternion quaternion;
    private void SetData()
    {
        threePos = UIManager.Instance.uiReward;
        time = Random.Range(timeDafault - 0.5f, timeDafault + 0.2f);
        quaternion = Quaternion.Euler(0, 0, Random.Range(-180, 90));
        towPosRandom = (quaternion * (twoPos.position - onePos.position) + onePos.position)*Random.Range(0.6f,1.2f);
        //transform.DOPath(firstTarget,4,PathType.CatmullRom);
    }
    private void OnEnable()
    {
        Move();
    }
    private void Move()
    {
        SetData();
        LeanTween.value(0, 1, time).setOnUpdate((float time) =>
        {
            firstTarget = Vector3.Lerp(onePos.position, towPosRandom, time);
            secondTarget = Vector3.Lerp(towPosRandom, threePos.position, time);
            thirdTarget = Vector3.Lerp(firstTarget, secondTarget, time);
            transform.position = thirdTarget;
        });
    }
}
