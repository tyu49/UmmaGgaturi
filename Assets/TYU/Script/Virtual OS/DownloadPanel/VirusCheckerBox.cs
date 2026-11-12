using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TYU.Script.Virtual_OS.DownloadPanel
{
    public class VirusCheckerBox : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private bool isVirusWhenActivate;
        [SerializeField] private Image checkImage;
        public bool currentState { get; private set; } //true = 활성화, false = 비활성화
        public event Action OnClicked;

        public bool GetVirusState() => isVirusWhenActivate == currentState;


        public void OnPointerClick(PointerEventData eventData)
        {
            ChangeState(!currentState);
            OnClicked?.Invoke();
        }

        public void ChangeState(bool stateValue)
        {
            currentState = stateValue;
            checkImage.enabled = currentState;
        }
        
        
    }
}