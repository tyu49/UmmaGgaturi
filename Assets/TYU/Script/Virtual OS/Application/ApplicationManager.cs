using System;
using System.Collections.Generic;
using UnityEngine;

namespace TYU.Script.Virtual_OS.Application
{
    public class ApplicationManager : MonoBehaviour
    {
        [SerializeField] private Canvas pointCanvas;
        [SerializeField] private Canvas applicationCanvas;
        [SerializeField] private GameObject appBase;
        [field : SerializeField] public ApplicationMover appMover { get; private set; }
        private PointMaker _pointMaker;
        private List<AppPoint> _points;

        private void Awake()
        {
            _pointMaker = GetComponentInChildren<PointMaker>();
            _points = _pointMaker.MakePositions(pointCanvas.transform);
        }

        private void TryMakeApplication(ApplicationSO data)
        {
            foreach (var point in _points)
            {
                if (point.curApp == null)
                {
                    var newApp = Instantiate(appBase, applicationCanvas.transform).GetComponent<Application>();
                    newApp.Initialize(data, point, this);
                    break;
                }
            }
        }
    }
}