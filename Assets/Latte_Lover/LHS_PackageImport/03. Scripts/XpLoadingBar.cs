using PrimeTween;
using UnityEngine;

namespace Latte_Lover.LHS_PackageImport._03._Scripts
{
    public class XpLoadingBar : MonoBehaviour
    {
        [SerializeField] private RectTransform blockGroup;

        [SerializeField] private float startX = -60f;
        [SerializeField] private float endX = 220f;

        [SerializeField] private float moveDuration = 1.2f;
        [SerializeField] private float stepInterval = 0.05f;
        [SerializeField] private float restartDelay = 0.2f;

        private Tween _moveTween;
        private Tween _delayTween;

        private void OnEnable()
        {
            PlayLoop();
        }

        private void OnDisable()
        {
            _moveTween.Stop();
            _delayTween.Stop();
        }

        private void PlayLoop()
        {
            if (blockGroup == null) return;

            SetPosition(startX);

            _moveTween = Tween.Custom(this, 0f, 1f, moveDuration, (target, progress)
                    => target.UpdatePosition(progress),Ease.Linear).OnComplete(this, target => target.OnMoveComplete());
        }

        private void UpdatePosition(float progress)
        {
            float elapsed = progress * moveDuration;
            float steppedTime = Mathf.Floor(elapsed / stepInterval) * stepInterval;
            float steppedProgress = Mathf.Clamp01(steppedTime / moveDuration);
            float x = Mathf.Lerp(startX, endX, steppedProgress);

            SetPosition(x);
        }

        private void OnMoveComplete()
        {
            SetPosition(endX);

            _delayTween = Tween.Delay(this, restartDelay, target => target.PlayLoop());
        }

        private void SetPosition(float x)
        {
            Vector2 position = blockGroup.anchoredPosition;
            position.x = x;
            blockGroup.anchoredPosition = position;
        }
    }
}