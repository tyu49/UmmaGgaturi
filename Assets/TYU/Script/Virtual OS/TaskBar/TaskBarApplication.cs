using PrimeTween;
using TYU.Script.Virtual_OS.Application;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace TYU.Script.Virtual_OS.TaskBar
{
    public class TaskBarApplication : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private Image backGround;
        [SerializeField] private TweenSettings<float> setting;
        private TaskBarManager _manager;
        private TaskBarAppMover _mover;
        private RectTransform _rtTrm;
        private Camera _mainCam;
        public ApplicationSO _myData { get; private set; }
        public void Initialize(ApplicationSO data, TaskBarManager manager, Vector2 pos)
        {
            _myData = data;
            _manager = manager;
            _rtTrm = transform as RectTransform;
            _mainCam = Camera.main;
            _mover = _manager.AppMover;
            icon.sprite = data.Icon;
            _rtTrm.anchoredPosition = pos;  
        }

        public void MoveTo(float xPos)
        {
            setting.startFromCurrent = true;
            setting.endValue = xPos;
            Tween.UIAnchoredPositionX(_rtTrm, setting);
        }

        public void RemoveApp()
        {
            Destroy(gameObject);
        }

        public Vector2 GetPos(Canvas cvs)
        {
            Vector2 scPos = _mainCam.WorldToScreenPoint(transform.position);
            var cvsRt = cvs.transform as RectTransform;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(cvsRt, scPos,
                cvs.renderMode == RenderMode.ScreenSpaceCamera ? cvs.worldCamera : null, out var point);
            return point;
        }

        // public void StartMove() => _mover.StartMove(this);
        // public void EndMove() => _mover.StopMove();

        public void ClickApp()
        {
            if (_mover.isMoved) return;
            Debug.Log(1);
        }
        private void Update()
        {
            if(Mouse.current.leftButton.wasPressedThisFrame)
                CheckCanDrag(true);
            if(Mouse.current.leftButton.wasReleasedThisFrame)
                CheckCanDrag(false);
        }

        private void CheckCanDrag(bool isClick)
        {
            if(!isClick)
            {
                _mover.StopMove();
                return;
            }
            
            if(RectTransformUtility.RectangleContainsScreenPoint(_rtTrm, Mouse.current.position.ReadValue()))
                _mover.StartMove(this);
        }
    }
}