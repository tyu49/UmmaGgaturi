using TMPro;
using UnityEngine;

namespace TYU.Script.Virtual_OS.GgaturiTok
{
    public class MessageSendingOption : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;
        
        private MessageSO _myData;
        
        public void Initialize(MessageSO data)
        {
            _myData = data;
            text.text = _myData.Text;
        }
    }
}