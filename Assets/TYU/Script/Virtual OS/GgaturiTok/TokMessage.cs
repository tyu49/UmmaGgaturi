using System;
using UnityEngine;

namespace TYU.Script.Virtual_OS.GgaturiTok
{
    public class TokMessage : MonoBehaviour
    {
        private RectTransform _myTrm;

        private void Awake()
        {
            _myTrm = gameObject.GetComponent<RectTransform>();
        }
        public float GetHeight() => _myTrm.sizeDelta.y;

        public virtual void GetData(ReplyOption data)
        {
            
        }
    }
}