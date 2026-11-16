using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class IsNotResponding : MonoBehaviour, IPointerDownHandler
{
    private ErrorWindow errorWindow;
    private void Awake()
    {
        errorWindow =  GetComponentInChildren<ErrorWindow>();
        errorWindow.CloseErrorWindow();
        var btn = errorWindow.GetEvent();
        btn.onClick.AddListener(()=> Destroy(gameObject));
    }

    public void OnPointerDown(PointerEventData eventData) => errorWindow.ShowErrorWindow();
}
