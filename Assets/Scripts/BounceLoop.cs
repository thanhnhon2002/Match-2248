using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BounceLoop : MonoBehaviour
{
    [SerializeField] private float speed = 4F;
    [SerializeField] private float smallSize;
    [SerializeField] private float largeSize;

    private void OnEnable ()
    {
        speed = Mathf.Abs(speed);
    }

    private void Update ()
    {
        var scale = transform.localScale;
        if(scale.x >= largeSize) speed = -Mathf.Abs(speed);
        if(scale.x <= smallSize) speed = Mathf.Abs (speed);
        scale += Vector3.one * speed * Time.deltaTime;
        transform.localScale = scale;
    }
}
