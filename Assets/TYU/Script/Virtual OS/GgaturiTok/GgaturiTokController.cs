using System;
using System.Collections.Generic;
using System.Linq;
using TYU.Script.Virtual_OS.GgaturiTok;
using TYU.Script.Virtual_OS.Virus;
using UnityEngine;

public class GgaturiTokController : MonoBehaviour
{
    [SerializeField] private Transform areaTrm;
    [SerializeField] private MessageSender sender;
    private List<TokMessage> _messagesList = new();
    private float _defaultHeight;
    private RectTransform _areaRtTrm;
    private void Awake()
    {
        _areaRtTrm = areaTrm as RectTransform;
        _defaultHeight = _areaRtTrm.sizeDelta.y;
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
            _areaRtTrm.sizeDelta = new Vector2(0, _defaultHeight);
        }
        else
        {
            _areaRtTrm.sizeDelta = new Vector2(0, height);
        }
    }
    public void PushNewMessage(TokMessage msg, ReplyOption data)
    {
        Instantiate(msg, areaTrm);
        msg.Initialize(data);
    }

    private void GiveNewOptionData(MessageChoiceSO replyOption)
    {
        
    }
}
