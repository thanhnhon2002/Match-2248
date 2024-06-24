using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    [SerializeField] private ParticleSystem subParticleSystem;
    [SerializeField] private new ParticleSystem particleSystem;
    private ParticleSystem.MainModule mainModule;
    public void SetColor(Color color)
    {
        mainModule = particleSystem.main;
        mainModule.startColor = color;

        mainModule = subParticleSystem.main;
        mainModule.startColor = color;

    }
}
