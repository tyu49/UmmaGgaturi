using SUNFISHH.Scripts;
using UnityEngine;

namespace TYU.Script.Virtual_OS.GgaturiTok
{
    public class MakeDownLoadPanelMessage : TokMessage
    {
        [SerializeField] private AbstractWindow prefab;
        
        
        public override void Resize()
        {
            
        }

        public override void MakeNewEvent()
        {
            base.MakeNewEvent();
            
        }
        public void MakeDownloadPanel() => EventBus<MakeNewDownLoadPanel>.Publish(new  MakeNewDownLoadPanel()
        {
            data = prefab
        });
    }
}