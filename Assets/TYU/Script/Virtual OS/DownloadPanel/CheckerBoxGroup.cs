using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TYU.Script.Virtual_OS.DownloadPanel
{
    public class CheckerBoxGroup : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image checkImage;
        
        private bool _currentState; //true = 활성화, false = 비활성화
        private List<VirusCheckerBox> _checkersInGroup;

        public event Action OnClicked;
        
        private void Awake()
        {
            var boxes= GetComponentsInChildren<VirusCheckerBox>();
            _checkersInGroup = new();
            _checkersInGroup = boxes.ToList();
            foreach (var checker in _checkersInGroup)
                checker.OnClicked += CheckState;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if(_currentState) DisableAll();
            else ChangeState(!_currentState);
        }

        private void CheckState()
        {
            foreach (var checker in _checkersInGroup)
            {
                if (!checker.currentState)
                {
                    ChangeState(false);
                    return;
                }
            }
            ChangeState(true);
        }

        private void ChangeState(bool state)
        {
            
            _currentState = state;
            checkImage.enabled = _currentState;
            if(_currentState)
                foreach (var checker in _checkersInGroup)
                    checker.ChangeState(true);
        }

        private void DisableAll()
        {
            if(_currentState)
                foreach (var checker in _checkersInGroup)
                    checker.ChangeState(false);
            _currentState = false;
            checkImage.enabled = false;
        }

        private void OnDestroy()
        {
            foreach (var checker in _checkersInGroup)
                checker.OnClicked -= CheckState;
        }
    }
}