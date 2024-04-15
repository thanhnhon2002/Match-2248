using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RandomSprites : MonoBehaviour
{
    public List<Sprite> sprites;
    private TextMeshPro txtNumber;
    [SerializeField] private bool disable;
    [ContextMenu("Random Sprite")]
    public void Awake ()
    {
        if (disable) return;
        txtNumber = GetComponentInChildren<TextMeshPro>();
        int index = Random.Range (0, sprites.Count);
        GetComponent<SpriteRenderer>().sprite = sprites[index];
        txtNumber.text = ((int)Mathf.Pow (2, index + 1)).ToString ();
    }
}