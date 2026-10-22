using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TYU.Script.Virtual_OS.Application
{
    public class Application : MonoBehaviour
    {

        public ApplicationSO myData { get; private set; }
        public RectTransform trm { get; private set; }
        private Image _img;
        private TextMeshProUGUI _text;
        private AppPoint _curPoint;
        private ApplicationManager _manager;
        private float _clickDelay = 0.6f;
        private float _lastClickTime = 0f;

        //trigger event 써서 Up Down으로 누르고 때고 판단 하면 와! 개쌀먹 ㄴㅇㅅ
        //더블 클릭도 걍 ㅣㅅ간 재서 trigger로 클릭 두번 판단해서 시간 안에 했음녀 창 호출하도록 해야지ㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎ
        // 개쌀먹이다ㅏㅏㅏㅏㅏㅏㅏ
        
        private void OnEnable()
        {
            trm = transform as RectTransform;
        }

        public void Initialize(ApplicationSO data, AppPoint point, ApplicationManager manager)
        {
            _manager = manager;
            myData = data;
            _img.sprite = data.icon;
            _text.SetText(data.name);
        }

        public void StartMove() => _manager.appMover.StartMove(this);
        public void EndMove() => _manager.appMover.TryMove();
    }
}