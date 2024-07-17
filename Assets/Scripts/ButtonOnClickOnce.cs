using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonOnClickOnce : MonoBehaviour
{
    Button button;
    private void Awake()
    {
        button =GetComponent<Button>();
    }
    private void Start()
    {
        
        button.onClick.AddListener(() => button.enabled = false);
    }
    public void OnEnable()
    {
        button.enabled = true;
    }
}
