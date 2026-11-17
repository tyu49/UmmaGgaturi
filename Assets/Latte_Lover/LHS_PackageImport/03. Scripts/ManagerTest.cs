using UnityEngine;
using UnityEngine.UI;

namespace Latte_Lover.LHS_PackageImport._03._Scripts
{
    public class ManagerTest : MonoBehaviour
    {
        [SerializeField] private float duration;
        [SerializeField] private int _1;
        [SerializeField] private int _2;
        [SerializeField] private int _3;
        
        public void OnPixelBtn()
        {
            EffectManager.instance.SetPixel(_1);
        }

        public void OnPixelBtn2()
        {
            EffectManager.instance.PixelLerp(_2,duration);
        }

        public void OnPixelBtn3()
        {
            EffectManager.instance.PixelLerp(_3, duration);
        }

        public void OnPixelBtn4()
        {
            EffectManager.instance.ResetPixel();
        }
        
    }
}