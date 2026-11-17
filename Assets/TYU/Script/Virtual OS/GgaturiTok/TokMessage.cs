using System;
using TMPro;
using UnityEngine;

namespace TYU.Script.Virtual_OS.GgaturiTok
{
    public class TokMessage : MonoBehaviour
    {
        [SerializeField] private RectTransform myTrm;
        [SerializeField] private TextMeshProUGUI text;
        
        public float GetHeight() => myTrm.sizeDelta.y;
        public virtual void Initialize(MessageSO data, GgaturiTokController controller)
        {
            
        }

        [ContextMenu("Test")]
        public virtual void Resize()
        {
            text.ForceMeshUpdate();
            Vector2 textSize = text.GetRenderedValues();
            textSize.y -= text.rectTransform.anchoredPosition.y;
            myTrm.sizeDelta = new Vector2(myTrm.sizeDelta.x, textSize.y);
        }
    }
}