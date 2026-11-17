using System;
using UnityEngine;

namespace TYU.Script.Virtual_OS.GgaturiTok
{
    [Serializable]
    public struct ReplyOption
    {
        [field:SerializeField] public string Text { get; private set; }
        [field:SerializeField] public TokMessage Message { get; private set; }
    }
}