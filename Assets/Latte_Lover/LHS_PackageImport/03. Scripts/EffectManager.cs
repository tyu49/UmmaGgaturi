using System;
using UnityEngine;
using PrimeTween;
namespace Latte_Lover.LHS_PackageImport._03._Scripts
{
    public class EffectManager : MonoBehaviour
    {
        public static EffectManager instance { get; private set; }
        
        private void Awake()
        {
            if (instance == null) instance = this;
            else Destroy(gameObject);
            DontDestroyOnLoad(gameObject);

            ResetPixel();
        }


        #region Pixelize
        
        [SerializeField] private Material PixelizeMat;
        private Tween _pixelTween;
        private int CurrentPixelCount;
        
        private static readonly int _PixelCount = Shader.PropertyToID("_PixelCount"); //만지지마!!!!!!!!!!!!!!!!

        public void SetPixel(int value)
        {
            if(_pixelTween.isAlive)
                _pixelTween.Stop();
            CurrentPixelCount = value;
            PixelizeMat.SetInt(_PixelCount,CurrentPixelCount);
        }

        public void ResetPixel()
        {
            if(_pixelTween.isAlive)
                _pixelTween.Stop();
            CurrentPixelCount = 3840;
            PixelizeMat.SetInt(_PixelCount, CurrentPixelCount);
        }
        
        public void PixelLerp(int EndPixel,float duration)
        {
            CurrentPixelCount = EndPixel;
            PixelFade(CurrentPixelCount,duration);
        }
        private void PixelFade(int target, float duration)
        {
            CurrentPixelCount = target;
            _pixelTween.Stop();
            _pixelTween = Tween.Custom(
                startValue: PixelizeMat.GetFloat(_PixelCount),
                endValue: target,
                duration: duration,
                onValueChange: value => PixelizeMat.SetFloat(_PixelCount, value),
                ease: Ease.OutSine );
        }
        

        #endregion


        private void OnDestroy()
        {
            ResetPixel();
        }
    }
}
