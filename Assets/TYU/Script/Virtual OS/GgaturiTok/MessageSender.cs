using System;
using TMPro;
using UnityEngine;

namespace TYU.Script.Virtual_OS.GgaturiTok
{
    public class MessageSender : MonoBehaviour
    {

        [SerializeField] private GameObject replyOptionPanel;
        private MessageChoiceSO _currentData;
        
        
        
        public void GiveData(MessageChoiceSO option)
        {//선택지들 주기
            _currentData = option;
        }
        public void OpenOptions()
        {// 선택지 선택창 열기
            if (_currentData == null) return;
            replyOptionPanel.SetActive(true);
            
        }
        public void ChooseOption()
        {
            _currentData = null;
        }
        
    }

}