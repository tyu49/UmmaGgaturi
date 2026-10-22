using UnityEngine;
using UnityEngine.InputSystem;

namespace TYU.Script
{
    public static class GetMousePosInCanvas
    {
        public static Vector2 GetPos(Canvas canvas)
        {
            if (canvas is null) return Vector2.zero;
            var screenPos = Mouse.current.position.ReadValue();
            var csvRtTrm = canvas.transform as RectTransform;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                csvRtTrm,
                screenPos,
                canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null :
                canvas.worldCamera,
                out var point);
            return point;
        }
    }
}