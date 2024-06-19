using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDisable : MonoBehaviour
{
    [SerializeField] private float time;
    private float currentTime;

    private void OnEnable()
    {
        currentTime = time;
    }

    private void Update()
    {
        currentTime -= Time.deltaTime;
        gameObject.SetActive(currentTime > 0);
    }
}
