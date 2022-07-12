using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings<T> : MonoBehaviour where T : Settings<T>
{
    public static T Instance;

    private void Start()
    {
        Instance = (T)this;
    }
}
