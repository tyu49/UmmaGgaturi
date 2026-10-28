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
        private RectTransform _rtTrm;
        private TaskBarManager _manager;
        private Canvas _myCvs;
        private TaskBarApplication _curData;
        private UIDragMovementModule _mvMd;
        public void Initialize(Canvas cvs, TaskBarManager manager)
        {
            _manager = manager;
            _myCvs = cvs;
            _rtTrm = transform as RectTransform;
            _mvMd = new UIDragMovementModule(_rtTrm, _myCvs);
            gameObject.SetActive(false);
        }

        public void StartMove(TaskBarApplication data)
        {
            gameObject.SetActive(true);
            
            _rtTrm.anchoredPosition = GetMousePosInCanvas.GetPos(_myCvs);
            iconTrm.position = data.transform.position;
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
            _mvMd.StopMove();
            gameObject.SetActive(false);
        }

        private void Update()
        {
            _mvMd.UseModule();
        }
    }
}