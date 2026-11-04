using PrimeTween;
using TYU.Script.Virtual_OS.Application;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace TYU.Script.Virtual_OS.TaskBar
{
    public class TaskBarApplication : MonoBehaviour, IPointerUpHandler, IPointerMoveHandler, IPointerClickHandler, IPointerDownHandler
    {
        [SerializeField] private Image icon;
        [SerializeField] private Image backGround;
        [SerializeField] private TweenSettings<float> setting;
        private TaskBarManager _manager;
        private TaskBarAppMover _mover;
        private RectTransform _rtTrm;
        private Camera _mainCam;
        private Vector3 _beforePos;
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
            if (_myData.LockInTaskBar) return;
            _manager.RemoveApp(_myData);
        }

        public Vector2 GetPos(Canvas cvs)
        {
            Vector2 scPos = _mainCam.WorldToScreenPoint(transform.position);
            var cvsRt = cvs.transform as RectTransform;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(cvsRt, scPos,
                cvs.renderMode == RenderMode.ScreenSpaceCamera ? cvs.worldCamera : null, out var point);
            return point;
        }

        public void ClickApp()
        {
            Debug.Log(1);
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                _mover.StopMove();
            }
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            if(eventData.dragging)
                _mover.StartMove(this, _beforePos);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            ClickApp();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if(eventData.button == PointerEventData.InputButton.Left)
                _beforePos = transform.position;
        }
    }
}