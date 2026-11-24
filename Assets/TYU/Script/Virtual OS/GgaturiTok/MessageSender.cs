using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace TYU.Script.Virtual_OS.GgaturiTok
{
    public class MessageSender : MonoBehaviour
    {
        [SerializeField] private CanvasGroup optionSelectGroup;
        [SerializeField] private List<MessageSendingOption> options;
        private MessageChoiceSO _currentData;

        private void Start()
        {
            EventBus<MessageChoosesData>.Subscribe(ChooseOption);
            optionSelectGroup.alpha = 0;
            optionSelectGroup.blocksRaycasts = false;
            optionSelectGroup.interactable = false;
        }

        private void OnDestroy()
        {
            EventBus<MessageChoosesData>.Unsubscribe(ChooseOption);
        }

        public void GiveData(MessageChoiceSO option)
        {//선택지들 주기
            _currentData = option;
            int index = 0;
            foreach (var item in options)
                item.gameObject.SetActive(false);
            foreach (var optionData in _currentData.ReplyOption)
            {
                options[index].gameObject.SetActive(true);
                options[index++].Initialize(optionData);
            }
        }
        public void OpenOptions()
        {// 선택지 선택창 열기
            if (_currentData == null) return;
            optionSelectGroup.alpha = 1;
            optionSelectGroup.blocksRaycasts = true;
            optionSelectGroup.interactable = true;
        }
        private void ChooseOption(MessageChoosesData data)
        {
            _currentData = null;
            optionSelectGroup.alpha = 0;
            optionSelectGroup.blocksRaycasts = false;
            optionSelectGroup.interactable = false;
        }

        public void ResizeOptions()
        {
            foreach (var option in options)
                option.Resize();
        }
    }

    public struct MessageChoosesData
    {
        public MessageSO data;
    }

}