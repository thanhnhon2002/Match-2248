using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopupChangeName : MonoBehaviour
{
    [SerializeField] private TMP_InputField name;
    private void OnEnable()
    {
        var userData = GameSystem.userdata;
        
    }
}
