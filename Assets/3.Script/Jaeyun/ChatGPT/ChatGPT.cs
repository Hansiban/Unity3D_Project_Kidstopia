using OpenApiFormat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChatGPT : MonoBehaviour
{
    private NPCInfo_Data info;
    [SerializeField] private TMP_Text dialogText;
    [SerializeField] private Button audioButton;

    private OpenAIRequest api;

    public async void TestResponseButton()
    {
        api = new OpenAIRequest();
        api.openAi_key = "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";
        api.Init();

        ChatRequest chatRequest = new ChatRequest();
        chatRequest.messages.Add(new ChatMessage() { role = $"{role.system}", content = $"�ѽ�, �߽�, �Ͻ��� ���� �� ������ ���ؾ� �Ѵ�. 15�� �̳���" }); // npc prompt
        chatRequest.messages.Add(new ChatMessage() { role = $"{role.user}", content = $"���� �� ��������?" }); // audio
        List<ChatChoice> data = ((await (api.ClientResponseChat(chatRequest))).choices);
        Debug.Log(data);
        foreach (ChatChoice choice in data)
        {
            Debug.Log(choice.message.content);
        }
    }

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
        request.messages = new List<ChatMessage>
        {
            new ChatMessage() { role = role.system.ToString(), content = $""},
        };
    }

    private async void ChatGPTRequest(string message)
    {
        ChatRequest request = new ChatRequest();
        request.messages = new List<ChatMessage>
        {
            new ChatMessage() { role = role.user.ToString(), content = $"{message}" },
        };

    }
}
