using System.Collections.Generic;
using TYU.Script.Virtual_OS.Application;
using UnityEngine;

namespace TYU.Script.Virtual_OS.TaskBar
{
    public class TaskBarManager : MonoBehaviour
    {
        [SerializeField] private GameObject appBase;
        private List<TaskBarApplication> _tBApps = new();
        private Canvas _myCvs;

        public void Initialize(Canvas cvs)
        {
            _myCvs = cvs;
        }
        
        public void MakeNewApp(ApplicationSO data)
        {
            
        }

        public void RemoveApp(TaskBarApplication data)
        {
            if (!_tBApps.Contains(data)) return;
            
        }

    }
}