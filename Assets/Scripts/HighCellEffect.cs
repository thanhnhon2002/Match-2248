using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighCellEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem particle;
    [SerializeField] private GameObject crown;

    public void ShowEffect()
    {
        crown.SetActive(true);
        particle.Play();
    }

    public void StopEffect()
    {
        crown.SetActive(false);
        particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear) ;
    }
}
