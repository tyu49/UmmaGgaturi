using System;
using System.Collections.Generic;
using SUNFISHH.Scripts;
using SUNFISHH.Scripts.Player;
using TYU.Script;
using TYU.Script.Virtual_OS.GgaturiTok;
using UnityEngine;

public class WindowManager : MonoSingleton<WindowManager>
{
    [SerializeField] private Transform windowParent;
    private Dictionary<MakeNewDownLoadPanel, AbstractWindow> _downloadDict = new();
    private static WindowManager instance = null;
    private readonly List<AbstractWindow> _windows;

    private void Awake()
    {
        if (!MakeSingleton()) return;
        EventBus<MakeNewDownLoadPanel>.Subscribe(MakeNewWindow);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        EventBus<MakeNewDownLoadPanel>.Unsubscribe(MakeNewWindow);
    }

    private void MakeNewWindow(MakeNewDownLoadPanel data)
    {
        if (_downloadDict.TryGetValue(data, out var window))
        {
            try
            {
                Destroy(window.gameObject);
            }
            catch (MissingReferenceException)
            {
                return;
            }
        }
        _downloadDict[data] = Instantiate(data.data, windowParent);
        _downloadDict[data].OpenCheck(true);
    }
    
}
