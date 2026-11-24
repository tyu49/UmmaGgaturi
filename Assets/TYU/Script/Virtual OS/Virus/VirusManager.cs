using System;
using System.Collections.Generic;
using UnityEngine;

namespace TYU.Script.Virtual_OS.Virus
{
    public class VirusManager : MonoSingleton<VirusManager>
    {
        [SerializeField] private List<AbstractVirus> viruses;
        
        private Dictionary<VirusEnum, int> _virusDangerCount = new();
        
        private void Awake()
        {
            if (!MakeSingleton()) return;
            foreach (VirusEnum type in Enum.GetValues(typeof(VirusEnum)))
            {
                _virusDangerCount.TryAdd(type, 0);
            }
        }

        public void IncreaseVirusDanger(VirusEnum type)
        {
            if (!_virusDangerCount.TryGetValue(type, out var virus)) return;
            Debug.LogError("이거 없데");
            virus++;
        }
        public int GetVirusDanger(VirusEnum type)
        {
            return _virusDangerCount.GetValueOrDefault(type, 0);
        }
    }
}