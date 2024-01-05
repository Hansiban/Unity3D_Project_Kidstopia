using OpenApiFormat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading;

public class ChatGPT : MonoBehaviour
{
    private NPCInfo_Data info;
    [SerializeField] private TMP_Text dialogText;
    [SerializeField] private Button audioButton;

    OpenAIRequest api;

    public void DialogButtonClick()
    {
        ChatGPTRequest(SpeechToText()); // STT�� string message
    }

    private void NpcClick()
    {
        NpcDialogStart();
    }

    private string SpeechToText()
    { // ���� �ν� ��ư
        // ���� �ν��� ���� ũ�⺸�� Ŭ ��
        string message = string.Empty;

        
        return message;
    }

    private async void NpcDialogStart()
    { // Npc Ŭ������ �� �� �� ����
        ChatRequest request = new ChatRequest();
        request.message = new List<ChatMessage>
        {
            new ChatMessage() { role = role.system, message = $""},
        };
    }

    private async void ChatGPTRequest(string message)
    {
        ChatRequest request = new ChatRequest();
        request.message = new List<ChatMessage>
        {
            
            new ChatMessage() { role = role.system, message = $"{message}" },
        };

    }
}
