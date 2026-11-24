using System;
using System.Collections.Generic;
using System.Linq;
using SUNFISHH.Scripts;
using TYU.Script.Virtual_OS.GgaturiTok;
using TYU.Script.Virtual_OS.Virus;
using UnityEngine;
using UnityEngine.UI;

public class GgaturiTokController : AbstractWindow
{
    [SerializeField] private Transform areaTrm;
    [SerializeField] private MessageSender sender;
    [SerializeField] private MessageSO firstMessage;
    [SerializeField] private MessageSO tsedsfs;
    
    private List<TokMessage> _messagesList = new();
    private float _defaultHeight;
    private RectTransform _areaRtTrm;
    
    private void Awake()
    {
        base.Awake();
        OnStateChanged += ChangeMessageSize;
        _areaRtTrm = areaTrm as RectTransform;
        _defaultHeight = _areaRtTrm.sizeDelta.y;
    }

    private void OnDestroy()
    {
        OnStateChanged -= ChangeMessageSize;
    }

    private void Start()
    {
        PushNewMessage(firstMessage);
    }
    private void ChangeMessageSize()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(_areaRtTrm);
        
        float height = 0;
        foreach (var message in _messagesList)
        {
            message.Resize();
            height +=message.GetHeight();
        }
        _areaRtTrm.sizeDelta = new Vector2(0, Mathf.Max(_defaultHeight, height));
        LayoutRebuilder.ForceRebuildLayoutImmediate(_areaRtTrm);
    }
    public void PushNewMessage(MessageSO data)
    {
        var nwMsg = Instantiate(data.MyMessage, areaTrm);
        nwMsg.Initialize(data, this);
        _messagesList.Add(nwMsg);
        if (data.HasReplyOption)
        {
            sender.GiveData(data.ReplyOption);
        }
        else
        {
            nwMsg.MakeNewEvent();
        }
        ChangeMessageSize();
    }
}
