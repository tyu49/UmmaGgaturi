using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class WindowLayerReplace : MonoBehaviour, IPointerDownHandler
{
    public void Replace() => transform.SetAsLastSibling();
    public void OnPointerDown(PointerEventData eventData) => transform.SetAsLastSibling();
}
