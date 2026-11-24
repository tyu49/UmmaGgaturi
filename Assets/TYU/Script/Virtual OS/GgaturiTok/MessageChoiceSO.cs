using System.Collections.Generic;
using UnityEngine;

namespace TYU.Script.Virtual_OS.GgaturiTok
{
    [CreateAssetMenu(fileName ="Choice", menuName = "GgaturiTok/Choice", order = 0)]
    public class MessageChoiceSO : ScriptableObject
    {
        [field:SerializeField] public List<MessageSO> ReplyOption{get; private set; }
    }
}