using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OptionMenu
{
    Home,
    PlayerInformation,
    Shop,
}
public class MenuOptions : MonoBehaviour
{
    public static MenuOptions Instance;
    Dictionary<OptionMenu,GameObject> dicMenuOptions = new Dictionary<OptionMenu,GameObject>();
    [SerializeField] OptionAnimation defaultOption;
    private void Awake()
    {
        var options = GetComponentsInChildren<Option>(true);
        foreach (var option in options) dicMenuOptions.Add(option.option, option.gameObject);
        Instance = this;
    }
    private void Start()
    {
        OptionAnimation.optionAnimation = defaultOption;
        ShowOption(OptionAnimation.optionAnimation.option);
    }
    public void ShowOption(OptionMenu option)
    {
        dicMenuOptions[option].SetActive(true);
    }
    public void HideOption(OptionMenu option)
    {
        dicMenuOptions[option].SetActive(false);
    }
    public void HideAllOption(OptionMenu optionException)
    {
        foreach (var option in dicMenuOptions.Keys)
            if (option != optionException)
            {
                if (dicMenuOptions[option].activeInHierarchy) dicMenuOptions[option].SetActive(false);
            }
            else if (!dicMenuOptions[option].activeInHierarchy) dicMenuOptions[option].SetActive(true);
    }
    
}
