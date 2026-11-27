using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TYU.Script.Virtual_OS.GgaturiTok
{
    public class MessageSendingOption : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;
        private RectTransform _myTrm;
        private MessageSO _myData;

        private void Awake()
        {
            _myTrm = transform as RectTransform;
        }

        public void Initialize(MessageSO data)
        {
            _myData = data;
            text.text = _myData.Text;
            Resize();
        }

        public void Resize()
        {
            text.ForceMeshUpdate();
            Vector2 textSize = text.GetRenderedValues();
            _myTrm.sizeDelta = new Vector2(_myTrm.sizeDelta.x, textSize.y);
        }
    
        public void Choose()
        {
            EventBus<MessageChoosesData>.Publish(new MessageChoosesData()
            {
                data = _myData
            }); 
        }
    }
}