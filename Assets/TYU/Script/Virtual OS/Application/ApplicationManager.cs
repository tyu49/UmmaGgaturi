using System;
using UnityEngine;

namespace TYU.Script.Virtual_OS.Application
{
    public class ApplicationManager : MonoBehaviour
    {
        [SerializeField] private Canvas pointCanvas;
        [SerializeField] private Canvas applicationCanvas;
        [field : SerializeField] public ApplicationMover appMover { get; private set; }
        private PointMaker _pointMaker;

        private void Awake()
        {
            _pointMaker = GetComponentInChildren<PointMaker>();
            _pointMaker.MakePositions(pointCanvas.transform);
        }
    }
}