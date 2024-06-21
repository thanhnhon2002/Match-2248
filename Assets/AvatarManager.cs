using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarManager : MonoBehaviour
{
    public static AvatarManager Instance;

    public Sprite[] avatars;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
}
