using System;
using PrimeTween;
using UnityEngine;

namespace SUNFISHH.Scripts
{
    public abstract class AbstractWindow : MonoBehaviour
    {
        private RectTransform _rectTransform;
        private bool _isMaxClick;
        
        private Vector2 _curTransform;
        private Vector2 _curRectSize;

        #region trashAnim
        [SerializeField] private float duration = 0.05f;
        [SerializeField] private TweenSettings<Vector2> openMax;
        [SerializeField] private TweenSettings<Vector2> openMin;
        [SerializeField] private TweenSettings<Vector2> openPos;
        [SerializeField] private TweenSettings<Vector2> closeMax;
        [SerializeField] private TweenSettings<Vector2> closeMin;
        [SerializeField] private TweenSettings<Vector2> closePos;
        #endregion

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }
        
        // 최소화
        public void MinWindow()
        {
            gameObject.SetActive(false);
        }

        // 전체 화면 
        public void MaxWindow()
        {
            _isMaxClick = !_isMaxClick;
            
            if (_isMaxClick)
            {
                _curRectSize = _rectTransform.sizeDelta;
                _curTransform = _rectTransform.anchoredPosition;

                openMax.settings.duration = openMin.settings.duration = openPos.settings.duration = duration;
                
                openMax.startValue = _rectTransform.anchorMax;
                openMin.startValue = _rectTransform.anchorMin;
                openPos.startValue = _curTransform;
                
                openMax.endValue = new Vector2(1f, 1f);
                openMin.endValue = new Vector2(0f, 0f);
                
                Tween.UIAnchorMax(_rectTransform, openMax);
                Tween.UIAnchorMin(_rectTransform, openMin);
                Tween.UIAnchoredPosition(_rectTransform, openPos);
                
                _rectTransform.sizeDelta = Vector2.zero;
            }
            else
            {
                closeMax.settings.duration = closeMin.settings.duration = closePos.settings.duration = duration;
                
                closeMax.startValue = _rectTransform.anchorMax;
                closeMin.startValue = _rectTransform.anchorMin;
                closePos.startValue = _curTransform;
                
                closeMin.endValue = closeMax.endValue = new Vector2(0.5f, 0.5f);
                closePos.endValue = _curTransform;
                
                Tween.UIAnchorMax(_rectTransform, closeMax);
                Tween.UIAnchorMin(_rectTransform, closeMin);
                Tween.UIAnchoredPosition(_rectTransform, closePos);
                
                _rectTransform.sizeDelta = _curRectSize;

            }
                
        }
        
        // 창 닫기
        // 서서히 사라지는 애니메이션 만들기
        public void CloseWindow()
        {
            gameObject.SetActive(false);
            _rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            _rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        }
    }
}