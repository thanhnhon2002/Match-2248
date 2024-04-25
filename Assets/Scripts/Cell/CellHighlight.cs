using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellHighlight : MonoBehaviour
{
    public Cell cell;

    private void Update ()
    {
        gameObject.SetActive (cell != null);
    }
}
