using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using System.Collections;

public class Tutorial : MonoBehaviour
{
    public static Tutorial instance;
    public List<EffectPart> effects = new List<EffectPart>();
    public int currentPart;
    public Image background;
    public Image background2;
    [SerializeField] GameObject uiHome;
    private void Awake()
    {
        instance = this;
    }
    public void StartTutorial()
    {
        currentPart = 0;
        effects[currentPart].Animation(0);
        background.gameObject.SetActive(true);
        background2.gameObject.SetActive(true);
        uiHome.gameObject.SetActive(false);

    }
    public void NextPart()
    {
        //effects[currentPart].part.gameObject.SetActive(false);
        currentPart++;
        if (currentPart == effects.Count)
        {
            ////Start Game
            var userData = GameSystem.userdata;
            userData.firstPlayGame = true;
            GameSystem.SaveUserDataToLocal();         
            StartCoroutine(StartGame());
            return;
        }
        effects[currentPart].Animation(0);
    }
    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(1);
        //background.gameObject.SetActive(false);
        //background2.gameObject.SetActive(false);
        //uiHome.gameObject.SetActive(true);
        //effects[currentPart-1].part.gameObject.SetActive(false);
        Home.Instance.MoveToGamePlay();
    }
}
