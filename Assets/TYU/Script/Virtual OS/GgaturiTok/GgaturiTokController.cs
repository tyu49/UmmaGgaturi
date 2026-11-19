using System;
using System.Collections.Generic;
using System.Linq;
using SUNFISHH.Scripts;
using TYU.Script.Virtual_OS.GgaturiTok;
using TYU.Script.Virtual_OS.Virus;
using UnityEngine;

public class GgaturiTokController : AbstractWindow
{
    [SerializeField] private Transform areaTrm;
    [SerializeField] private MessageSender sender;
    private List<TokMessage> _messagesList = new();
    private float _defaultHeight;
    private RectTransform _areaRtTrm;
    public event Action OnSizeChanged;
    private void Awake()
    {
        base.Awake();
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
            message.Resize();
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
    public void PushNewMessage(MessageSO data)
    {
        var nwMsg = Instantiate(data.MyMessage, areaTrm);
        nwMsg.Initialize(data, this);
        if (data.HasReplyOption)
        {
            sender.GiveData(data.ReplyOption);
            return;
        }
        nwMsg.MakeNewEvent();
    }
    /*
     * 미래의 나를 위한 지금 생각하는 방향성
     * 기본 메세지는 저기서 다시 PushNewMessage를 보내고
     * 아닐경우 MakeNewEvent에서 싱글톤을 이용해서 특수 이벤트나 상황을 불러오는 식으로 갈듯?
     * 선택지에선 sender에게 GiveData로 데이터를 보내고 선택을 하면 선택하는 애가
     * 자기 SO를 다시 sender에게 보내주고 sender는 다시 그걸 Controller에게 보내고, 다시 거기서 PushNewMessage로 보내주고 그걸 반복하면 됌
     * 이론상? 일단은 될 듯 ㅇㅇ
     */
}
