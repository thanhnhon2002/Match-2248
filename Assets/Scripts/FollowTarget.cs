using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public Transform target;
    [SerializeField] private Vector3 offset;
    //// Start is called before the first frame update
    //void Start()
    //{
    //    //offset = transform.localPosition;
    //    transform.SetParent(null);
    //}

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = target.position + offset;
    }
}
