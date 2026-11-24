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
                VirusManager.Instance.IncreaseVirusDanger(virus);
            }
            IndividualEventBus<VirusEnum>.Publish(VirusEnum.MEMEZ);
            base.StartDownload();
            Destroy(gameObject);
        }
    }
}