using System.Collections.Generic;
using UnityEngine;

namespace TYU.Script.Virtual_OS.GgaturiTok
{
    [CreateAssetMenu(fileName = "FILENAME", menuName = "MENUNAME", order = 0)]
    public class MessageChoice : ScriptableObject
    {
        [field:SerializeField] public List<ReplyOption> ReplyOption{get; private set; }
    }
}