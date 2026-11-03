using System;
using System.Collections.Generic;
using NUnit.Framework.Internal;
using TYU.Script.Virtual_OS.TaskBar;
using UnityEngine;

namespace TYU.Script.Virtual_OS.Application
{
    public class ApplicationManager : MonoBehaviour
    {
        [SerializeField] private Canvas pointCanvas;
        [SerializeField] private Canvas applicationCanvas;
        [SerializeField] private Application appBase;
        [field : SerializeField] public ApplicationMover appMover { get; private set; }
        [SerializeField] private ApplicationSO testSo;
        [SerializeField] private TaskBarManager tBManager;
        private PointMaker _pointMaker;
        private List<AppPoint> _points;

        public event Action<Application> clickAction;
        
        private void Awake()
        {
            _pointMaker = GetComponentInChildren<PointMaker>();
            _points = _pointMaker.MakePositions(pointCanvas);
            appMover.Initialize(applicationCanvas);
            tBManager.Initialize();
        }

        private void TryMakeApplication(ApplicationSO data)
        {
            for (int x = 0; x < 18; ++x)
            {
                bool isEnd = false;
                for (int y = 0; y < 10; ++y)
                {
                    var point = _points[x + y*18];
                    if (point.curApp == null)
                    {
                        
                        var newApp = Instantiate(appBase, applicationCanvas.transform).GetComponent<Application>();
                        newApp.Initialize(data, point, this);
                        isEnd = true;
                        break;
                    }
                }

                if (isEnd) break;
            }
            tBManager.MakeNewApp(data);
        }

        [ContextMenu("test")]
        public void tets() => TryMakeApplication(testSo);

        public void ClickAction(Application caller = null)
        {
            clickAction?.Invoke(caller);
        }
    }
}