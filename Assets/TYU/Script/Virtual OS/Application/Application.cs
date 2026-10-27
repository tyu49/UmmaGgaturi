using System;
using SUNFISHH.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TYU.Script.Virtual_OS.Application
{
    public class Application : MonoBehaviour
    {
        public ApplicationSO myData { get; private set; }
        public RectTransform trm { get; private set; }
        [SerializeField] private Image img;
        [SerializeField] private Image backGround;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private Color selectColor;
        [SerializeField] private Color clickColor;
        private AppPoint _curPoint;
        private ApplicationManager _manager;
        private float _clickDelay = 0.6f;
        private float _lastClickTime = 0f;
        private bool _isClicked;

        private AbstractWindow _myWindow;
        //대충 여기다가 나중에 추상화 클래스로 창을 프로퍼티로 넣으셈
        //trigger event 써서 Up Down으로 누르고 때고 판단 하면 와! 개쌀먹 ㄴㅇㅅ
        //더블 클릭도 걍 ㅣㅅ간 재서 trigger로 클릭 두번 판단해서 시간 안에 했음녀 창 호출하도록 해야지ㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎ
        // 개쌀먹이다ㅏㅏㅏㅏㅏㅏㅏ
        public void Initialize(ApplicationSO data, AppPoint point, ApplicationManager manager)
        {
            trm = transform as RectTransform;
            _manager = manager;
            myData = data;
            img.sprite = data.Icon;
            text.SetText(data.Name);
            gameObject.name = data.Name;
            point.Put(this);
            _curPoint = point;
            // _myWindow = data.Window.GetComponent<AbstractWindow>();
            _manager.clickAction += ClickServer;
        }

        public void MouseEnter() => backGround.color = selectColor;
        public void MouseExit() => backGround.color = _isClicked ? clickColor : new Color(1, 1, 1, 0);
        
        public void StartMove() => _manager.appMover.StartMove(this);
        public void EndMove() => _manager.appMover.TryMove();

        public void ClickAction()
        {
            _manager.ClickAction(this);
            if (myData == null) return;
            // if(_lastClickTime + _clickDelay < Time.time)
            //이때 _myWindow에서 창 호출하는거 하면 됌
            _lastClickTime = Time.time;
        }

        private void ClickServer(Application data)
        {
            _isClicked = data == this;
            if (!_isClicked)
                backGround.color = new Color(1, 1, 1, 0);
        }

        public void TryChangePos(AppPoint point)
        {
            if (!point.Put(this)) return;
            _curPoint.RemoveMyApp();
            _curPoint = point;
        }

        private void OnDestroy()
        {
            if (_manager == null) return;
            _manager.clickAction -= ClickServer;
        }
    }
}