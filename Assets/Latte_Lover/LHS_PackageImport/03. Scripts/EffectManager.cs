using System;
using System.Collections;
using System.Collections.Generic;
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


        #region Jansang

//지정 시간만큼 잔상 나옴
        public void PlayAfterImage(SpriteRenderer target, float interval, float duration,float afterImageLifetime, Color color, float alpha)
        {
            if (target == null || interval <= 0f || duration <= 0f || afterImageLifetime <= 0f) return;

            StartCoroutine(AfterImageRoutine(target, interval, duration, color, alpha, afterImageLifetime));
        }

        private IEnumerator AfterImageRoutine(SpriteRenderer target, float interval, float duration, Color color,
            float alpha, float afterImageLifetime)
        {
            float elapsed = 0f;
            WaitForSeconds wait = new WaitForSeconds(interval);

            while (elapsed < duration && target != null)
            {
                CreateAfterImage(target, color, alpha, afterImageLifetime);

                yield return wait;
                elapsed += interval;
            }
        }

        private void CreateAfterImage(SpriteRenderer target, Color color, float alpha, float fadeDuration)
        {
            GameObject ghost = new GameObject("AfterImage");

            ghost.transform.SetPositionAndRotation(target.transform.position, target.transform.rotation);
            ghost.transform.localScale = target.transform.lossyScale;

            SpriteRenderer renderer = ghost.AddComponent<SpriteRenderer>();

            renderer.sprite = target.sprite;
            renderer.flipX = target.flipX;
            renderer.flipY = target.flipY;
            renderer.sortingLayerID = target.sortingLayerID;
            renderer.sortingOrder = target.sortingOrder - 1;

            color.a = Mathf.Clamp01(alpha);
            renderer.color = color;

            StartCoroutine(FadeAfterImage(renderer, fadeDuration));
        }

        private IEnumerator FadeAfterImage(SpriteRenderer renderer, float duration)
        {
            Color color = renderer.color;
            float startAlpha = color.a;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;

                color.a = Mathf.Lerp(startAlpha, 0f, Mathf.Clamp01(elapsed / duration));
                renderer.color = color;

                yield return null;
            }

            Destroy(renderer.gameObject);
        }


//껐다켰다하는거
        private readonly Dictionary<SpriteRenderer, Coroutine> _afterImageCoroutines = new();

        public void StartAfterImage(SpriteRenderer target, float interval, float afterImageLifetime, Color color, float alpha)
        {
            if (target == null || interval <= 0f || afterImageLifetime <= 0f) return;
            if (_afterImageCoroutines.ContainsKey(target)) return;

            Coroutine coroutine =
                StartCoroutine(ToggleAfterImageRoutine(target, interval, color, alpha, afterImageLifetime));

            _afterImageCoroutines.Add(target, coroutine);
        }

        public void StopAfterImage(SpriteRenderer target)
        {
            if (ReferenceEquals(target, null)) return;
            if (!_afterImageCoroutines.TryGetValue(target, out Coroutine coroutine)) return;

            if (coroutine != null)
                StopCoroutine(coroutine);

            _afterImageCoroutines.Remove(target);
        }

        private IEnumerator ToggleAfterImageRoutine(SpriteRenderer target, float interval, Color color, float alpha,
            float afterImageLifetime)
        {
            WaitForSeconds wait = new WaitForSeconds(interval);

            while (target != null)
            {
                CreateAfterImage(target, color, alpha, afterImageLifetime);
                yield return wait;
            }

            _afterImageCoroutines.Remove(target);
        }

        #endregion


        #region Flash

        public enum FlashMode
        {
            InstantFade,
            SmoothFade
        }

        private sealed class FlashState
        {
            public Coroutine Coroutine;
            public Color OriginalColor;
        }

        private readonly Dictionary<SpriteRenderer, FlashState> _flashStates = new();

        public void PlayFlash(SpriteRenderer target, Color flashColor, int flashCount, float flashInterval,
            float flashDuration, FlashMode flashMode)
        {
            if (target == null || flashCount <= 0 || flashInterval < 0f || flashDuration <= 0f) return;

            Color originalColor = target.color;

            if (_flashStates.TryGetValue(target, out FlashState previous))
            {
                StopCoroutine(previous.Coroutine);
                originalColor = previous.OriginalColor;
                target.color = originalColor;
                _flashStates.Remove(target);
            }

            FlashState state = new FlashState
            {
                OriginalColor = originalColor
            };

            _flashStates.Add(target, state);

            state.Coroutine = StartCoroutine(
                FlashRoutine(target, flashColor, flashCount, flashInterval, flashDuration, flashMode, state)
            );
        }

        private IEnumerator FlashRoutine(SpriteRenderer target, Color flashColor, int flashCount, float flashInterval,
            float flashDuration, FlashMode flashMode, FlashState state)
        {
            Color originalColor = state.OriginalColor;
            flashColor.a = originalColor.a;

            for (int i = 0; i < flashCount; i++)
            {
                float elapsed = 0f;

                while (elapsed < flashDuration)
                {
                    if (target == null)
                    {
                        _flashStates.Remove(target);
                        yield break;
                    }

                    float progress = Mathf.Clamp01(elapsed / flashDuration);
                    float intensity;

                    if (flashMode == FlashMode.InstantFade)
                    {
                        intensity = Mathf.SmoothStep(1f, 0f, progress);
                    }
                    else
                    {
                        intensity = progress < 0.5f
                            ? Mathf.SmoothStep(0f, 1f, progress * 2f)
                            : Mathf.SmoothStep(1f, 0f, (progress - 0.5f) * 2f);
                    }

                    MaterialPropertyBlock block = new MaterialPropertyBlock();

                    target.GetPropertyBlock(block);
                    block.SetFloat("_FlashAmount", intensity);
                    block.SetColor("_FlashColor", flashColor);
                    target.SetPropertyBlock(block);

                    elapsed += Time.deltaTime;
                    yield return null;
                }

                if (target == null)
                {
                    _flashStates.Remove(target);
                    yield break;
                }

                target.color = originalColor;

                if (i < flashCount - 1 && flashInterval > 0f)
                {
                    yield return new WaitForSeconds(flashInterval);
                }
            }

            _flashStates.Remove(target);
        }

        #endregion


        private void OnDestroy()
        {
            ResetPixel();
        }
    }
}
