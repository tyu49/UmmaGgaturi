using System;
using TYU.Script.Module;
using UnityEngine;
using UnityEngine.UI;

namespace TYU.Script.Virtual_OS.TaskBar
{
    public class TaskBarAppMover : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private Transform iconTrm;
        [SerializeField] private float distanceLimit;
        private RectTransform _rtTrm;
        private TaskBarManager _manager;
        private Canvas _myCvs;
        private TaskBarApplication _curData;
        private UIDragMovementModule _mvMd;
        public bool isMoved { get; private set; }
        private Vector2 _beforePos;
        private bool _isMoving;
        public void Initialize(Canvas cvs, TaskBarManager manager)
        {
            _manager = manager;
            _myCvs = cvs;
            _rtTrm = transform as RectTransform;
            _mvMd = new UIDragMovementModule(_rtTrm, _myCvs);
            gameObject.SetActive(false);
        }

        public void StartMove(TaskBarApplication data, Vector3 beforePos)
        {
            if (_isMoving) return;
            _isMoving = true;
            gameObject.SetActive(true);
            isMoved = false;
            _beforePos = _rtTrm.anchoredPosition;
            _rtTrm.anchoredPosition = GetMousePosInCanvas.GetPos(_myCvs);
            iconTrm.position = beforePos;
            var rtTrm = data.transform as RectTransform;
            var plusPos = rtTrm.sizeDelta / 2;
            var iconRt = iconTrm as RectTransform;
            iconRt.anchoredPosition += plusPos;
            
            
            _curData = data;
            icon.sprite = _curData._myData.Icon;
            _mvMd.StartMove();
            
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.TryGetComponent<TaskBarApplication>(out var app)) return;
            if (app == _curData) return;
            _manager.ChangePos(_curData, app);
        }

        public void StopMove()
        {
            _isMoving = false;
            _mvMd.StopMove();
            gameObject.SetActive(false);
        }

        private void Update()
        {
            _mvMd.UseModule();
            if (Mathf.Abs((_rtTrm.anchoredPosition - _beforePos).magnitude) > distanceLimit)
                isMoved = true;
        }
        /*
         * さよなら歌姫最後の曲だね
         * 君の歌が本当に好きだ
         * 今夜も歌姫凄く綺麗だね
         * 君の事が本当に好きだ
         * 
         * さよなら歌姫アンコールはどうする
         * 君の事だから　きっと無いね
         * さよなら歌姫、アルコールはどうする
         * 僕は全然飲めないけど
         */
    }
}