using System;
using System.Collections.Generic;
using TYU.Script.Virtual_OS.Application;
using UnityEngine;

namespace TYU.Script.Virtual_OS.DownloadPanel
{
    public abstract class AbstractDownloadChecker : MonoBehaviour
    {
        [SerializeField] protected List<VirusCheckerBox> checkers;
        [SerializeField] protected ApplicationSO downloadApp;
        protected int _errorChecks;
        protected bool _hasVirus;

        public void GetDataFromCheckers()
        {
            _errorChecks = 0;
            foreach (var checker in checkers)
            {
                if (checker.GetVirusState()) _errorChecks++;
            }
            _hasVirus = _errorChecks > 0;
        }
        public virtual void StartDownload()
        {
            ApplicationManager.Instance.TryMakeApplication(downloadApp);
        }
    }
}