using UnityEngine;

namespace TYU.Script.Virtual_OS.GgaturiTok
{
    public class MessageSender : MonoBehaviour
    {

        [SerializeField] private GameObject replyOptionPanel;
        private ReplyOption _currentData = new();
        private bool _hasData;
        
        
        
        public void GiveData(ReplyOption option)
        {//선택지들 주기
            _currentData = option;
            _hasData = true;
        }
        public void OpenOptions()
        {// 선택지 선택창 열기
            if (!_hasData) return;
            replyOptionPanel.SetActive(true);
            
        }
        public void ChooseOption()
        {
            _hasData = false;
            _currentData = new ReplyOption();
        }
        
    }

}