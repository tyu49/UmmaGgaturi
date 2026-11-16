using System;
using System.Collections.Generic;
using System.Linq;
using TYU.Script.Virtual_OS.GgaturiTok;
using TYU.Script.Virtual_OS.Virus;
using UnityEngine;

public class GgaturiTokSizeController : MonoBehaviour
{
    [SerializeField] private RectTransform _myTrm;
    private List<TokMessage> _messagesList = new();
    private float _defaultHeight;

    private void Awake()
    {
        _defaultHeight = _myTrm.sizeDelta.y;
    }

    
    [ContextMenu("Test")]
    public void OnChangeMessage()
    {
        _messagesList.Clear();
        _messagesList = GetComponentsInChildren<TokMessage>().ToList();
        float height = 0;
        foreach (var message in _messagesList)
        {
            height +=message.GetHeight();
        }

        if (height < _defaultHeight)
        {
            _myTrm.sizeDelta = new Vector2(0, _defaultHeight);
            _myTrm.anchoredPosition = new Vector2(0, 0);
        }
        else
        {
            _myTrm.sizeDelta = new Vector2(0, height);
            _myTrm.anchoredPosition = new Vector2(0, height -  _defaultHeight);
        }
    }
}
