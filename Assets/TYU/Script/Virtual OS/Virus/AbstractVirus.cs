using System;
using UnityEngine;

namespace TYU.Script.Virtual_OS.Virus
{
    public class AbstractVirus : MonoBehaviour
    {
        [SerializeField] protected Canvas virusCvs;
        [SerializeField] protected Canvas windowCvs;
         [field : SerializeField]public VirusEnum type { get; protected set; }
         private VirusEnum _beforeType;
         public virtual void KillHisComputer()
         {
         }

         private void OnValidate()
         {
             if (_beforeType == type) return;
             if(type != VirusEnum.None)
                type -= _beforeType;
             _beforeType = type;
         }
    }
}