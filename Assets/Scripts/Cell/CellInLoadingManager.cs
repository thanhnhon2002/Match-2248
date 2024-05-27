using DarkcupGames;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CellInLoadingManager : MonoBehaviour
{
    public GameObject[] imageObjs;
    public Slider loadingSlider;
    public GameObject[] prefabEffectUIs;
    public AudioClip[] audios;
    private const float CELL_VOLUME = 0.2f;
    public void OnStartAnimation()
    {
        PlaySound();
        UnDisplayObj();
        DisplayEffectObj();
        TurnOffSlider();
    }

    private void TurnOffSlider()
    {
        loadingSlider.gameObject.transform.DORotate(new Vector3(0, 0, 180), 0).OnComplete(() =>
        {
            loadingSlider.DOValue(0, 0f).OnComplete(() =>
            {
                loadingSlider.gameObject.SetActive(false);
            });

        });
    }

    private void PlaySound()
    {
        AudioSystem.Instance.PlaySound(audios.RandomElement(), CELL_VOLUME);
    }

    private void DisplayEffectObj()
    {
        foreach (GameObject obj in prefabEffectUIs)
        {
            obj.GetComponent<EffectUIAnimManager>().MoveToTarget();
        }
    }

    private void UnDisplayObj()
    {
        foreach (GameObject obj in imageObjs)
        {
            obj.SetActive(false);
        }
    }
}
