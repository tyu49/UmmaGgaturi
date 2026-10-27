using System;
using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace TYU.Script.Virtual_OS.Application
{
    public class ApplicationMover : MonoBehaviour
    {
        [SerializeField] private Vector2 size;
        [SerializeField] private ContactFilter2D target;
        [SerializeField] private bool debug;
        [SerializeField]private Image img;
        [SerializeField]private TextMeshProUGUI text;
        private Application _data;
        private RectTransform _rtTrm;
        private Canvas _myCvs;
        private bool _isOn;
        private Collider2D[] _resultArray = new Collider2D[8];
        private Vector2 _beforePos;
        //OnMouseClick 에서 Canceled가 됐을 떄 bool값을 false로 해서 위치 변경 취소 -> 개머저리 그냥 쌩고생 하는 거임 이거 완전
        
        //mouse의 위치 변화를 Update로 기록 해서 이동한 만큼 똑같이 Mover도 움직이면 되나? 아마도 되지 않을까

        public void Initialize(Canvas parentCanvas)
        {
            _myCvs = parentCanvas;
            _rtTrm= transform as RectTransform;
            
            gameObject.SetActive(false);
        }
        
        private void ChangeVisual()
        {
            img.sprite = _data.myData.Icon;
            text.SetText(_data.myData.Name);
        }

        public void StartMove(Application data)
        {
            _beforePos = GetMousePosInCanvas.GetPos(_myCvs);
            _rtTrm.anchoredPosition = data.trm.anchoredPosition;
            _data = data;
            ChangeVisual();
            gameObject.SetActive(true);
            _isOn = true;
        }

        public void TryMove()
        {
            
            _isOn = false;
            AppPoint targetPoint = TryCatchPoint();
            if (targetPoint != null)
                _data.TryChangePos(targetPoint);
            _data = null;
            gameObject.SetActive(false);
        }

        private void Update()
        {
            if (_isOn)
            {
                Vector2 curPos = GetMousePosInCanvas.GetPos(_myCvs);
                _rtTrm.anchoredPosition -= _beforePos - curPos;
                _beforePos = curPos;
            }
        }

        private AppPoint TryCatchPoint()
        {
            int cnt = Physics2D.OverlapBox(_rtTrm.position, size, 0f, target, _resultArray);
            if (cnt < 1) return null;
            AppPoint targetPoint = null;
            float minDistance = float.MaxValue;
            for (int i = 0; i < cnt; ++i)
            {
                Collider2D curTarget = _resultArray[i];
                if (curTarget.TryGetComponent<AppPoint>(out var point))
                {
                    var distance = (_rtTrm.transform.position - curTarget.transform.position).magnitude;
                    if (distance > minDistance) continue;
                    minDistance = distance;
                    targetPoint = point;
                }
            }

            return targetPoint;
        }

        private void OnDrawGizmos()
        {
            if (!debug) return;
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, size);
        }
    }
}