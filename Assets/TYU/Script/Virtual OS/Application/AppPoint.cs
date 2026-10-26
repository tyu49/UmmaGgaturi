using System;
using UnityEngine;

namespace TYU.Script.Virtual_OS.Application
{
    public class AppPoint : MonoBehaviour
    {
        public Application curApp { get; private set; }
        private Canvas _myCanvas;
        private RectTransform _rtTrm;

        public void Initialize(Canvas canvas)
        {
            curApp = null;
            _myCanvas = canvas;
            _rtTrm = transform as RectTransform;
        }

        public bool Put(Application appData)
        {
            if (curApp != null) return false;
            appData.trm.anchoredPosition = _rtTrm.anchoredPosition;
            curApp = appData;
            return true;
        }

        public void RemoveMyApp() => curApp = null;
    }
}