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
        Debug.Log("OnStartAuthority");
        if(isLocalPlayer)
        {
            canvas.SetActive(true);
        }
        onMessage += newMessage;
    }
    private void newMessage(string mess)
    {
        Debug.Log("newMessage");
        Debug.Log($"�߰��� �޼��� {mess}");
        chatText.text += mess;
    }

    //Ŭ���̾�Ʈ�� Server�� ������ �� 
    [ClientCallback]
    private void OnDestroy()
    {
        Debug.Log("OnDestroy");
        if (!isLocalPlayer) return;
        onMessage -= newMessage;
    }
    //RPC�� �ᱹ ClientRpc ��ɾ� < Command(server)��ɾ� < Client ��ɾ�?
    
    [Client]
    public void Send()
    {
        Debug.Log("Send");
        //if (!Input.GetKeyDown(KeyCode.Return)) return;
        if (string.IsNullOrWhiteSpace(inputfield.text)) return;
        cmdSendMessage(SQLManager.instance.info.User_NickName,inputfield.text);
        inputfield.text = string.Empty;
    }
    
    [Command(requiresAuthority = false)]
    private void cmdSendMessage(string nickname, string message)
    {
        Debug.Log($"cmdSendMessage : {message}");
        RPCHandleMessage(nickname,message);
    }

    [ClientRpc]
    private void RPCHandleMessage(string nickname, string message)
    {
        Debug.Log("RPCHandleMessage");
        onMessage?.Invoke($"[{nickname}] : {message}\n");
    }
}
