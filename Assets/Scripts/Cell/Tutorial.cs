using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class Tutorial : MonoBehaviour
{
    public static Tutorial instance;
    public List<EffectPart> effects = new List<EffectPart>();
    public int currentPart;
    public Image background;
    public Image background2;
    [SerializeField] GameObject uiHome;
    [SerializeField] GameObject uiEnd;
    public GameObject hand;
    private void Awake()
    {
        instance = this;
        hand.SetActive(false);
    }
    public void StartTutorial()
    {
        currentPart = 0;
        effects[currentPart].Animation(0);
        hand.SetActive(false);
        background.gameObject.SetActive(true);
        background2.gameObject.SetActive(true);
        uiHome.SetActive(false);

    }
    public void NextPart()
    {
        //effects[currentPart].part.gameObject.SetActive(false);
        hand.transform.DOComplete();
        currentPart++;
        if (currentPart == effects.Count)
        {
            ////Start Game
            var userData = GameSystem.userdata;
            userData.firstPlayGame = false;
            GameSystem.SaveUserDataToLocal();         
            StartCoroutine(StartGame());
            return;
        }
        effects[currentPart].Animation(0);
        //hand.SetActive(true);
    }
    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(1f);
        background.gameObject.SetActive(false);
        uiEnd.SetActive(true);
        effects[currentPart-1].part.gameObject.SetActive(false);
        uiEnd.GetComponent<CanvasGroup>().alpha = 0;
        uiEnd.GetComponent<CanvasGroup>().DOFade(1, 1f);
        yield return new WaitForSeconds(1f);
        //background2.gameObject.SetActive(false);
        //uiHome.gameObject.SetActive(true);       
        uiEnd.SetActive(false);
        Home.Instance.MoveToGamePlay();
    }

    public void Fail()
    {
        var currentPart = effects[this.currentPart];
        currentPart.titlle.GetComponent<CanvasGroup>().alpha = 0;
        currentPart.topic.GetComponent<CanvasGroup>().alpha = 0;

        currentPart.titlle.GetComponent<CanvasGroup>().DOFade(1f, 0.5f);
        currentPart.topic.GetComponent<CanvasGroup>().DOFade(1f, 0.5f);

        currentPart.titlle.text = "Oops";
        currentPart.topic.text = "Try again";
    }
}
