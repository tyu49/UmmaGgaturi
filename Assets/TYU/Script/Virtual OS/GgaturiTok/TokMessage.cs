using System;
using TMPro;
using UnityEngine;

namespace TYU.Script.Virtual_OS.GgaturiTok
{
    public class TokMessage : MonoBehaviour
    {
        [SerializeField] protected RectTransform myTrm;
        [SerializeField] protected TextMeshProUGUI text;
        protected GgaturiTokController Controller;
        protected MessageSO MyData;
        public float GetHeight() => myTrm.sizeDelta.y;
        public virtual void Initialize(MessageSO data, GgaturiTokController controller)
        {
            MyData = data;
            Controller = controller;
            text.SetText(data.Text);
        }

        [ContextMenu("Test")]
        public virtual void Resize()
        {
            Vector2 textSize = text.GetPreferredValues(text.text);
            textSize.y -= text.rectTransform.anchoredPosition.y;
            myTrm.sizeDelta = new Vector2(myTrm.sizeDelta.x, textSize.y);
        }

        public virtual void MakeNewEvent()
        {
            if(!MyData.IsEndMessage)
                Controller.PushNewMessage(MyData.NextMessage);
            else IndividualEventBus<GgaturiTokEvent>.Publish(MyData.MyEvent);
        }
    }
}