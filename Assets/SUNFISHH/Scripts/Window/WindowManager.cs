using System;
using System.Collections.Generic;
using SUNFISHH.Scripts;
using SUNFISHH.Scripts.Player;
using TYU.Script;
using UnityEngine;

public class WindowManager : MonoSingleton<WindowManager>
{
    [SerializeField] private Transform windowParent;
    
    private static WindowManager instance = null;
    private readonly List<AbstractWindow> _windows;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    
}
