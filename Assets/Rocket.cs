using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public Vector2 target;
    private void Update ()
    {
        var dir = target - (Vector2)transform.position;
        transform.up = dir.normalized;
    }
}
