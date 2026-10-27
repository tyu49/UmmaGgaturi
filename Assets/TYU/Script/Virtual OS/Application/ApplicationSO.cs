using System;
using SUNFISHH.Scripts;
using UnityEngine;

namespace TYU.Script.Virtual_OS.Application
{
    [CreateAssetMenu(fileName = "ApplicationData", menuName = "Virtual/Application", order = 0)]
    public class ApplicationSO : ScriptableObject
    {
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field:SerializeField] public string Name { get; private set; }
        [field:SerializeField] public GameObject Window { get; private set; }
        [field: SerializeField] public bool LockInTaskBar { get; private set; }

        private void OnValidate()
        {
            if (Window != null && !Window.TryGetComponent<AbstractWindow>(out var window))
                Window = null;
        }
    }
}