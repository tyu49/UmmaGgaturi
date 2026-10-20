using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TYU.Script.Virtual_OS.Application
{
    public class Application : MonoBehaviour
    {

        private Image _img;
        private TextMeshProUGUI _text;
        private AppPoint _curPoint;
        private ApplicationSO _data;
        private RectTransform _trm;

        private void OnEnable()
        {
            _trm = GetComponent<RectTransform>();
        }

        public void Initialize(ApplicationSO data, AppPoint point)
        {
            _img.sprite = data.icon;
            _text.SetText(data.name);
            

        }
    }
}