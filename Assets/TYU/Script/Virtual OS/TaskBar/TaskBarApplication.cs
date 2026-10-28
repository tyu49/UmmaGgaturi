using PrimeTween;
using TYU.Script.Virtual_OS.Application;
using UnityEngine;
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
        public ApplicationSO _myData { get; private set; }
        public void Initialize(ApplicationSO data, TaskBarManager manager, Vector2 pos)
        {
            _myData = data;
            _manager = manager;
            _rtTrm = transform as RectTransform;
            
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

        public void StartMove() => _mover.StartMove(this);
        public void EndMove() => _mover.StopMove();
    }
}