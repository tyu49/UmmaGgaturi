using System;
using System.Collections.Generic;
using TYU.Script.Virtual_OS.Application;
using TYU.Script.Virtual_OS.Virus;
using UnityEngine;

namespace TYU.Script.Virtual_OS.DownloadPanel
{
    public abstract class AbstractDownloadChecker : MonoBehaviour
    {
        [SerializeField] protected List<VirusCheckerBox> checkers;
        [SerializeField] protected ApplicationSO downloadApp;
        protected int _errorChecks;
        protected List<VirusEnum> _viruses;
        public event Action OnEssentialDidntCheck;
        public void GetDataFromCheckers()
        {
            _errorChecks = 0;
            _viruses = new List<VirusEnum>();
            foreach (var checker in checkers)
            {
                var virus = checker.GetVirusState();
                if (virus != VirusEnum.None)
                    _viruses.Add(virus);
                if (checker.EssentialCheck()) continue;
                OnEssentialDidntCheck?.Invoke();
            }
        }
        public virtual void StartDownload()
        {
            ApplicationManager.Instance.TryMakeApplication(downloadApp);
        }
    }
}