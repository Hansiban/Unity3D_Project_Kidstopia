using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class Chat : NetworkBehaviour
{
    [SerializeField] private Text chatText;
    [SerializeField] private InputField inputfield;
    [SerializeField] private GameObject canvas;

    private static event Action<string> onMessage;

    //client�� server�� connect �Ǿ��� �� �ݹ��Լ�
    public override void OnStartAuthority()
    {
        if(isLocalPlayer)
        {
            canvas.SetActive(true);
        }
        onMessage += newMessage;
    }
    private void newMessage(string mess)
    {
        chatText.text += mess;
    }

    //Ŭ���̾�Ʈ�� Server�� ������ �� 
    [ClientCallback]
    private void OnDestroy()
    {
        if (!isLocalPlayer) return;
        onMessage -= newMessage;
    }
    //RPC�� �ᱹ ClientRpc ��ɾ� < Command(server)��ɾ� < Client ��ɾ�?
    
    [Client]
    public void Send()
    {
        if (!Input.GetKeyDown(KeyCode.Return)) return;
        if (string.IsNullOrWhiteSpace(inputfield.text)) return;
        cmdSendMessage(inputfield.text);
        inputfield.text = string.Empty;
    }
    
    
    [Command]
    private void cmdSendMessage(string message)
    {
        RPCHandleMessage
            ($"[{SQLManager.instance.info.User_NickName}] : {message}");
        //($"[{connectionToClient.connectionId}] : {message}");
    }

    [ClientRpc]
    private void RPCHandleMessage(string message)
    {
        onMessage?.Invoke($"\n{message}");
    }
}
