using UnityEngine;

namespace TYU.Script.Virtual_OS.Virus.Viruses.WeakPointRemoveAndInstall
{
    public class VirusWeakPointRemoveAndInstall : AbstractVirus
    {
        [SerializeField] private TimerError installing;
        [SerializeField] private BlockInteractError installComplete;
        [SerializeField] private IsNotResponding notePad;

        public override void KillHisComputer()
        {
            base.KillHisComputer();
            Instantiate(notePad, windowCvs.transform);
        }
        
        
    }
}