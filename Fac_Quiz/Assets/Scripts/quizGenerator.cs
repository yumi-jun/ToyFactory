using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Models;
using UnityEngine.UI;
using TMPro;
using System.IO;


public class quizGenerator : MonoBehaviour
{
    public string inputLectureNoteStr;
    private OpenAIAPI openAIApi;
    [SerializeField] TMP_InputField quizNum;
    private string gptKey;


    private void Start()
    {
        FileInfo fileInfo = new FileInfo("./chatGPT_API_Key.txt");

        if (fileInfo.Exists)
        {
            StreamReader reader = new StreamReader("./chatGPT_API_Key.txt");
            gptKey = reader.ReadToEnd();
            reader.Close();
        }
        else {
            Debug.LogError("파일이 존재하지 않습니다");
        }

        openAIApi = new OpenAIAPI(gptKey);
    }

    public async void makeQuiz() {

        if (string.IsNullOrWhiteSpace(inputLectureNoteStr)) {
            Debug.Log("강의노트를 선택해주세요.");
            return;
        }
        if (string.IsNullOrWhiteSpace(quizNum.text)) {
            Debug.Log("생성될 문제의 개수를 입력해주세요.");
            return;
        }

        Debug.Log("퀴즈 개수: " + quizNum.text);

        var chat = openAIApi.Chat.CreateConversation();
        chat.Model = Model.ChatGPTTurbo;
        chat.RequestParameters.Temperature = 0; //값이 높을 수록 답이 창의적으로 나오지만 리스크가 큼

        chat.AppendSystemMessage("나는 대학교 강의노트의 내용을 퀴즈를 풀면서 공부하고 싶어. 너는 내가 제시한 지문을 바탕으로 퀴즈를 생성해주면 돼. 너의 대답에는 질문, 답, 문제의 해설 이 3가지가 필수로 포함되어야 해. 그리고 내가 만들어달라 한 문제 개수 만큼 필수로 만들어야 해."); //gpt에세 시스템으로서의 역할과 무엇을 해야하는지 세부적으로 설명

        //유저랑 어시스턴트 대화 예시
        //chat.AppendUserInput("");
        //chat.AppendExampleChatbotOutput("");

        //실제로 질문하는 부분
        chat.AppendUserInput( inputLectureNoteStr + " 이 글을 바탕으로 문제 " +quizNum.text+ "개를 만들어줘. 문제의 유형은 5지선다형이고 답은 1개여야해.");
        string response = await chat.GetResponseFromChatbotAsync();
        Debug.Log(response);

        foreach (ChatMessage msg in chat.Messages) {
            Debug.Log($"{msg.Role}: {msg.TextContent}");
        }
    }
}
