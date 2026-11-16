using UnityEngine;

namespace TYU.Script.Virtual_OS.Virus.Viruses
{
    public class VirusMEMEZ : AbstractVirus
    {
        [SerializeField] private IsNotResponding notePad;
        
        public override void KillHisComputer()
        {
            base.KillHisComputer();
            Instantiate(notePad, windowCvs.transform);
        }
    }
}