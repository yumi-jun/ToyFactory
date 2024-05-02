using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Models;

/*
 * 1. �� ���� ��ư
 * 2. ���� ���� ĵ����
 *  a. ���� ���� ��ư
 *  b. ���� ���� ���� üũ�ڽ� Ȥ�� ��ӹڽ�
 *  c. ���� ���� ���� inputfield
 *  d. �ļ� ��ư => gpt���� ���� �����̶� ���� �ؽ�Ʈ ����
 * 3. ��� �ؽ�Ʈ �ޱ� -> �̰Ű����� ���� ui�� �ִ°� �ٸ� �е���??
 */


public class quizGenerator : MonoBehaviour
{
    public string inputLectureNoteStr;
    private OpenAIAPI openAIApi;

    private void Start()
    {
        //������ path���� key �������ִ� �ؽ�Ʈ ���� �������� �ڵ� ã�Ƽ� �ٽ� ��Ա�
        openAIApi = new OpenAIAPI("apikey here");
    }

    public async void makeQuiz() {
        var chat = openAIApi.Chat.CreateConversation();
        chat.Model = Model.ChatGPTTurbo;
        chat.RequestParameters.Temperature = 0; //���� ���� ���� ���� â�������� �������� ����ũ�� ŭ

        chat.AppendSystemMessage("gpt���� �ý������μ��� ���Ұ� ������ �ؾ��ϴ��� ���������� ����");

        //������ ��ý���Ʈ ��ȭ ����
        chat.AppendUserInput("");
        chat.AppendExampleChatbotOutput("");

        //������ �����ϴ� �κ�
        chat.AppendUserInput("");
        string response = await chat.GetResponseFromChatbotAsync();
        Debug.Log(response);

        foreach (ChatMessage msg in chat.Messages) {
            Debug.Log($"{msg.Role}: {msg.TextContent}");
        }
    }
}
