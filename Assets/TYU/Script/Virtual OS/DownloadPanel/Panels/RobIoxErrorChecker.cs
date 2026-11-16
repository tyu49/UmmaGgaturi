using TYU.Script.Virtual_OS.Virus;
using UnityEngine;

namespace TYU.Script.Virtual_OS.DownloadPanel.Panels
{
    public class RobIoxErrorChecker : AbstractErrorChecker
    {
        public override void StartDownload()
        {
            GetDataFromCheckers();
            foreach (var virus in _viruses)
            {
                var _virus = VirusManager.Instance.GetVirus(virus);
                _virus.KillHisComputer();
            }
            base.StartDownload();
            Destroy(gameObject);
        }
    }
}