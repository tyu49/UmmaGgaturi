using System;
using UnityEngine;

namespace TYU.Script.Virtual_OS.GgaturiTok
{
    [CreateAssetMenu(fileName ="Choice", menuName = "GgaturiTok/message", order = 0)]
    public class MessageSO : ScriptableObject
    {
        [field: SerializeField] public string Text { get; private set; }
        [field: SerializeField] public TokMessage MyMessage { get; private set; }
        [field: SerializeField] public MessageSO NextMessage { get; private set; }
        [field: SerializeField] public bool HasReplyOption { get; private set; }
        [field: SerializeField] public MessageChoiceSO ReplyOption { get; private set; }

        private void OnValidate()
        {
            if (!HasReplyOption && ReplyOption != null)
                ReplyOption = null;
        }
    }
}