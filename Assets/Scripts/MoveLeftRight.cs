using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Security.Claims;

public class MoveLeftRight : MonoBehaviour
{
    public GameObject left;
    public GameObject right;
    public List<TextMeshProUGUI> txtMultiples;
    public TextMeshProUGUI textButton;

    public float speed = 500f;
    public int multiple;
    private int direction;

    Vector3 centerPos;
    [SerializeField]float speedMove;
    Vector3 posMove;
    TextMeshProUGUI nearest;
    private void Awake()
    {
        centerPos = (left.transform.position+ right.transform.position)/2;
    }
    private void OnEnable()
    {
        transform.position = left.transform.position;
        direction = 1;
        nearest = txtMultiples[0];
    }
    //public void ShowAnim() {
    //    transform.position = left.transform.position;
    //    LeanTween.move(gameObject, right.transform.position, 1f).setLoopPingPong();
    //}

    private void Update()
    {
        posMove = transform.position;
        if (posMove.x <= left.transform.position.x) direction = 1;
        if (posMove.x >= right.transform.position.x) direction = -1; 
        speedMove = Mathf.Clamp(this.speed *(Vector3.Distance(left.transform.position, centerPos)/ (Vector3.Distance(transform.position,centerPos)+0.0005f)),0,1300f);
        transform.position += new Vector3(speedMove * direction * Time.deltaTime, 0); 
     

        txtMultiples.ForEach(x =>
        {
            if (Vector2.Distance(transform.position, x.transform.position) < Vector2.Distance(transform.position, nearest.transform.position))
            {
                nearest = x;
                multiple = int.Parse(nearest.text.Replace("x", ""));
                textButton.text = "";
                textButton.text = "Claim x"+ (multiple).ToString();
                var cal = 5 * multiple;
            }
        });  
    }
}
