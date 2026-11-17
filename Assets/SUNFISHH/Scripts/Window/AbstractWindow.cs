using System;
using PrimeTween;
using TMPro;
using TYU.Script.Module;
using TYU.Script.Virtual_OS.TaskBar;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace SUNFISHH.Scripts
{
    public abstract class AbstractWindow : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [Header("Window Settings")] 
        private Canvas _canvas;
        private TaskBarApplication _taskBarApplication;
        private bool _isOpen;
        private bool _isMin;
        
        public bool IsOpen => _isOpen;
        // protected RectTransform WindowRect => _rectTransform;
        
        [Header("Drag Settings")]
        // [SerializeField] private Vector2 dragSize;
        [SerializeField] private RectTransform dragRange;
        private UIDragMovementModule _dragMovementModule;
        
        [Header("Max Settings")]
        private RectTransform _rectTransform;
        private Image _image;
        private bool _isMaxClick;
        
        private Vector2 _srtSize;
        private Vector2 _curTransform;
        private Vector2 _curRectSize;
        private Vector2 _minPos;
        
        protected virtual bool CanDrag => true;
        
        #region primeTween
        [SerializeField] private float duration = 0.05f;
        
        // 최소화
         private TweenSettings<Vector2> minWin;
         private TweenSettings<Vector2> rtnWin;
        
        // 전체화면
        private TweenSettings<Vector2> openMax;
        private TweenSettings<Vector2> openMin;
        private TweenSettings<Vector2> openPos;
        private TweenSettings<Vector2> openDelta;
        
        private TweenSettings<Vector2> closeMax;
        private TweenSettings<Vector2> closeMin;
        private TweenSettings<Vector2> closePos;
        private TweenSettings<Vector2> closeDelta;

        // 닫기
        private TweenSettings<Vector2> openWin;
        private TweenSettings<Vector2> closeWin;
        [SerializeField] private TweenSettings<float> openWinAlpha;
        [SerializeField] private TweenSettings<float> closeWinAlpha;
        
        #endregion

        public virtual void OnWindowUpdate() { }
        public virtual void OnOpen() { }
        public virtual void OnClose() { }

        private Camera _cam;
        

        public void Init(TaskBarApplication taskBar)
        {
            _taskBarApplication = taskBar;
            Setting();
        }
        

        public void Commit(TaskBarApplication taskBar)
        {
            _taskBarApplication = taskBar;
        }

        protected virtual void Setting()
        {
            
        }

        public void OpenCheck(bool value)
        {
            _isOpen = value;
        }
        
        protected virtual void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _image = GetComponent<Image>();
            _canvas = GetComponentInParent<Canvas>();

            _dragMovementModule = new UIDragMovementModule(_rectTransform, _canvas);
            
            _srtSize = _rectTransform.sizeDelta;
            
            _cam =  Camera.main;
        }

        private void Update()
        {
            /*if (CanDrag)
            {
                if (Mouse.current.leftButton.wasPressedThisFrame)
                    CheckCanDrag(true);

                if (Mouse.current.leftButton.wasReleasedThisFrame)
                    CheckCanDrag(false);

                _dragMovementModule.UseModule();
            }*/
            
            OnWindowUpdate();
        }

        private void CheckCanDrag(bool isClick)
        {
            if(!isClick)
            {
                _dragMovementModule.StopMove();
                return;
            }

            /*if (CheckConflictWindow())
            {
                _dragMovementModule.StopMove();
                return;
            }*/
            
            var cam = _canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : _canvas.worldCamera;
            if (RectTransformUtility.RectangleContainsScreenPoint(dragRange, Mouse.current.position.ReadValue(), cam))
            {
                _dragMovementModule.StartMove();
            }
            
        }

        private bool CheckConflictWindow()
        {
            if (_cam == null) return false;
            
            int index = _rectTransform.GetSiblingIndex();
            return true;
        }

        #region 최소화
        
        // 작업표시줄 쪽으로 작아지는 애니메이션 만들기 (살려줘)
        // 주의 : _curRectSize만큼 커지기
        public virtual void MinWindow()
        {
            if(_taskBarApplication == null) return;
            var pos = _taskBarApplication.GetPos(_canvas);

            _isOpen = false;
            gameObject.SetActive(false);
        }

        public virtual void RtnWindow()
        {
            _isOpen = true;
            gameObject.SetActive(true);
        }

        #endregion
        
        #region 전체화면

        public virtual void MaxWindow()
        {
            _isMaxClick = !_isMaxClick;
            
            if (_isMaxClick)
            {
                dragRange.gameObject.SetActive(false);
                _curRectSize = _rectTransform.sizeDelta;
                _curTransform = _rectTransform.anchoredPosition;

                openMax.settings.duration = openMin.settings.duration = openPos.settings.duration = openDelta.settings.duration = duration;
                
                openMax.startValue = _rectTransform.anchorMax;
                openMin.startValue = _rectTransform.anchorMin;
                openPos.startValue = _curTransform;
                openDelta.startValue = _curRectSize;
                
                openMax.endValue = new Vector2(1f, 1f);
                openMin.endValue = new Vector2(0f, 0f);
                openDelta.endValue = Vector2.zero;
                
                Tween.UIAnchorMax(_rectTransform, openMax);
                Tween.UIAnchorMin(_rectTransform, openMin);
                Tween.UIAnchoredPosition(_rectTransform, openPos);
                Tween.UISizeDelta(_rectTransform, openDelta);
            }
            else
            {
                closeMax.settings.duration = closeMin.settings.duration = closePos.settings.duration = closeDelta.settings.duration = duration;
                
                closeMax.startValue = _rectTransform.anchorMax;
                closeMin.startValue = _rectTransform.anchorMin;
                closePos.startValue = _curTransform;
                closeDelta.startValue = _curRectSize;
                
                closeMin.endValue = closeMax.endValue = new Vector2(0.5f, 0.5f);
                closePos.endValue = _curTransform;
                closeDelta.endValue = _curRectSize;
                
                Tween.UIAnchorMax(_rectTransform, closeMax);
                Tween.UIAnchorMin(_rectTransform, closeMin);
                Tween.UIAnchoredPosition(_rectTransform, closePos);
                Tween.UISizeDelta(_rectTransform, closeDelta);
                
                dragRange.gameObject.SetActive(true);
            }
                
        }

        #endregion
        
        #region 창 열고 닫기

        public virtual void OpenWindow()
        {
            if (_isOpen)
            {
                // transform.SetAsLastSibling();
                return;
            }
            
            _isOpen = true;
            gameObject.SetActive(true);
            _rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            _rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            
            openWin.settings.duration = openWinAlpha.settings.duration = duration;
            openWin.startValue = new Vector2(_srtSize.x /2, _srtSize.y /2);
            openWin.endValue = _srtSize;

            // 알파값 조정하는거 만들기
            openWinAlpha.startValue = _image.color.a;
            openWinAlpha.endValue = 0f;
            
            Tween.UISizeDelta(_rectTransform, openWin);
            OnOpen();
        }
        
        public virtual void CloseWindow()
        {
            if (!_isOpen) return;
            
            _isOpen = false;
            Vector2 curSize = _rectTransform.sizeDelta;
            
            closeWin.settings.duration = duration;
            closeWin.startValue = curSize;
            closeWin.endValue = new Vector2(curSize.x /2, curSize.y /2);

            Tween.UISizeDelta(_rectTransform, closeWin).OnComplete(() => gameObject.SetActive(false));
            
            _rectTransform.position = Vector2.zero;
            OnClose();
        }

        #endregion


        /*private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(dragTrs.position, dragSize);
        }*/
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            CheckCanDrag(true);
        }

        public void OnDrag(PointerEventData eventData)
        {
            _dragMovementModule.UseModule();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            CheckCanDrag(false);
        }
    }
}