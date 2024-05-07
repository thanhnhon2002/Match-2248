using UnityEngine;
using System.Collections.Generic;
using System;

public class Tutorial : MonoBehaviour
{
    public List<EffectPart> effects = new List<EffectPart>();
    int currentPart;
    private void Start()
    {
        StartTutorial();
    }
    public void StartTutorial()
    {
        currentPart = 0;
        effects[currentPart].Animation(0);
    }
    public void NextPart()
    {
        effects[currentPart].part.gameObject.SetActive(false);
        currentPart++;
        if (currentPart == effects.Count) return;
        effects[currentPart].Animation(0);
    }
}
