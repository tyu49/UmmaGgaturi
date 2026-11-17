using System;
using UnityEngine;

namespace TYU.Script.Virtual_OS.GgaturiTok
{
    [CreateAssetMenu(fileName ="Choice", menuName = "GgaturiTok/message", order = 0)]
    public class MessageSO : ScriptableObject
    {
        [field: SerializeField] public string Text { get; private set; }
        [field: SerializeField] public TokMessage Message { get; private set; }
        // [field: SerializeField] public 
    }
}