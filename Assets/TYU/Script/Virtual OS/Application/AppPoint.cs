using System;
using UnityEngine;

namespace TYU.Script.Virtual_OS.Application
{
    public class AppPoint : MonoBehaviour
    {
        private Application _curApp;
        private Canvas _myCanvas;
        private RectTransform _rtTrm;

        private void Initialize(Canvas canvas)
        {
            _curApp = null;
            _myCanvas = canvas;
            _rtTrm = transform as RectTransform;
        }

        public bool Put(Application appData)
        {
            if (_curApp != null) return false;
            appData.trm.anchoredPosition = _rtTrm.anchoredPosition;
            _curApp = appData;
            return true;
        }

        public void RemoveMyApp() => _curApp = null;
    }
}