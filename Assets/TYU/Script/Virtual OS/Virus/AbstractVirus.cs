using UnityEngine;

namespace TYU.Script.Virtual_OS.Virus
{
    public class AbstractVirus : MonoBehaviour
    {
         public VirusEnum type { get; protected set; }

         public virtual void KillHisComputer()
         {
             
         }
         
    }
}