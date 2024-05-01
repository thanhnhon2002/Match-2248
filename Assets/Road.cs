using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    public RectTransform rectTransform;
    [SerializeField] private float speed;
    private Vector2 direction;
    private int offset;
    private void Awake ()
    {
        rectTransform = (RectTransform)transform;
        offset = 1;
    }

    private void Update ()
    {
        speed -= Time.deltaTime;
        if (speed <= 0) speed = 0;
        rectTransform.anchoredPosition += new Vector2 (0, speed * Time.deltaTime * direction.y);
        if (rectTransform.anchoredPosition.y *2 < -rectTransform.rect.height)
        {
            rectTransform.anchoredPosition = new Vector2 (rectTransform.anchoredPosition.x, Screen.height /2f);
            if (offset == 1) offset = 2;
            else offset = 1;
        }
    }

    public void Move (float speed, Vector2 dir)
    {
        direction = dir;
        this.speed = speed;
    }
}
