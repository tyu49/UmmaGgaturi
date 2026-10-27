using System;
using UnityEngine;

namespace SUNFISHH.Scripts
{
    public abstract class AbstractWindow : MonoBehaviour
    {
        
        private RectTransform _rectTransform;
        private bool _isMaxClick;
        
        private Vector2 _curTransform;
        private Vector2 _curRectSize;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }
        
        // 최소화
        public void MinWindow()
        {
            gameObject.SetActive(false);
        }

        // 전체 화면 (개쌀먹 ㅋㅋㅋㅋ)
        public void MaxWindow()
        {
            _isMaxClick = !_isMaxClick;
            
            if (_isMaxClick)
            {
                _curRectSize = _rectTransform.sizeDelta;
                _curTransform = _rectTransform.anchoredPosition;
                
                _rectTransform.anchorMin = Vector2.zero;
                _rectTransform.anchorMax = Vector2.one;
                _rectTransform.sizeDelta = Vector2.zero;
                _rectTransform.anchoredPosition = Vector2.zero;
            }
            else
            {
                _rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                _rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
                _rectTransform.sizeDelta = _curRectSize;
                _rectTransform.anchoredPosition = _curTransform;

            }
                
        }
        
        // 창 닫기
        public void CloseWindow()
        {
            Destroy(gameObject);
        }
    }
}