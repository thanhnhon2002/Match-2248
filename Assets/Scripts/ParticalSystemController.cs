using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ParticleSystemJobs;
using System.Linq;
using UnityEngine.Events;

[RequireComponent(typeof(ParticleSystem))]
public class ParticalSystemController : MonoBehaviour
{
    [SerializeField] private ParticalSystemController[] childPartical;
    private ParticleSystem particle;
    private ParticleSystemRenderer particleRender;
    private Vector3 particalPivot;
    public Vector3 baseScale;
    public UnityEvent callbackAction;
    public ParticleSystem Particle => particle;
    public ParticalSystemController[] Children => childPartical;

    private bool hasInit;
    private void Awake()
    {
        Init();
    }
    public void Init()
    {
        if (hasInit) return;
        hasInit = true;
        baseScale = transform.localScale;
        if (particle == null) particle = GetComponent<ParticleSystem>();
        if (particleRender == null)
        {
            particleRender = GetComponent<ParticleSystemRenderer>();
            particalPivot = particleRender.pivot;
        }
        if (transform.childCount == 0) return;
        var listControllers = GetComponentsInChildren<ParticalSystemController>().ToList();
        listControllers.Remove(this);
        childPartical = listControllers.ToArray();
    }

    public void NormalFlip(float facingValue)
    {
        Init();
        var facing = particleRender.flip;
        if (facingValue < 0)
            facing.x = 0;
        else
            facing.x = 1;
        particleRender.flip = facing;
    }

    public void ScaleFlip(float facingValue)
    {
        Init();
        var scale = transform.localScale;
        if (facingValue < 0)
            scale.x = -1 * baseScale.x;
        else
            scale.x = 1 * baseScale.x;
        transform.localScale = scale;
    }

    public void FlipWithPivot(float facingValue)
    {
        Init();
        var facing = particleRender.flip;
        if (facingValue < 0)
        {
            facing.x = 0;
            particleRender.pivot = particalPivot;
        }
        else
        {
            facing.x = 1;
            particleRender.pivot = new Vector3(-particalPivot.x, particalPivot.y, particalPivot.z);
        }
        particleRender.flip = facing;
    }

    public void SetSortingOrder(LayerMask targetLayer)
    {
        Init();
        particleRender.sortingLayerID = SortingLayer.NameToID(LayerMask.LayerToName(targetLayer));
    }
    public void SetSortingOrder(int order)
    {
        Init();
        particleRender.sortingOrder = order + 1;
    }

    public void ChangeColor(Color color)
    {
        Init();
        var mainModlue = particle.main;
        mainModlue.startColor = color;
    }
    public void SetSortingOrderWithChildren(int order)
    {
        Init();
        particleRender.sortingOrder = order + 100;
        if (childPartical.Length == 0) return;
        for (int i = 0; i < childPartical.Length; i++)
        {
            childPartical[i].SetSortingOrder(order);
        }
    }

    public void Rotate(float zValue)
    {
        var main = particle.main;
        main.startRotation = zValue;
    }

    private void OnParticleSystemStopped()
    {
        callbackAction?.Invoke();
    }
}
