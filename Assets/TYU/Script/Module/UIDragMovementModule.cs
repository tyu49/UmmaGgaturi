using UnityEngine;

namespace TYU.Script.Module
{
    public class UIDragMovementModule
    {
        private Vector2 _beforePos;
        private RectTransform _targetTrm;
        private Canvas _cvs;
        private bool _isOn;

        public UIDragMovementModule(RectTransform trm, Canvas cvs)
        {
            _targetTrm = trm;
            _cvs = cvs;
        }
        
        public void UseModule()
        {
            if (_isOn)
            {
                Vector2 curPos = GetMousePosInCanvas.GetPos(_cvs);
                _targetTrm.anchoredPosition -= _beforePos - curPos;
                _beforePos = curPos;
            }
        }

        public void StartMove()
        {
            _beforePos = GetMousePosInCanvas.GetPos(_cvs);
            _isOn = true;
        }

        public void StopMove() => _isOn = false;
    }
}