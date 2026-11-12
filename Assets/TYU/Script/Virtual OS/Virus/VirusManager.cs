using System;
using System.Collections.Generic;
using UnityEngine;

namespace TYU.Script.Virtual_OS.Virus
{
    public class VirusManager : MonoSingleton<VirusManager>
    {
        [SerializeField] private List<AbstractVirus> viruses;

        private Dictionary<VirusEnum, AbstractVirus> _virusDict;

        private void Awake()
        {
            if (!MakeSingleton()) return;
            foreach (var virus in viruses)
                _virusDict.TryAdd(virus.type, virus);
        }

        public AbstractVirus GetVirus(VirusEnum type)
        {
            if (_virusDict.TryGetValue(type, out var virus)) return virus;
            Debug.LogError("이거 없데");
            return null;
        }
    }
}