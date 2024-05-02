using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Models;

/*
 * 1. 새 퀴즈 버튼
 * 2. 퀴즈 생성 캔버스
 *  a. 파일 선택 버튼
 *  b. 퀴즈 유형 선택 체크박스 혹은 드롭박스
 *  c. 문제 개수 선택 inputfield
 *  d. 셍성 버튼 => gpt한테 설정 값들이랑 강노 텍스트 전달
 * 3. 결과 텍스트 받기 -> 이거가지고 퀴즈 ui에 넣는건 다른 분들이??
 */


public class quizGenerator : MonoBehaviour
{
    public string inputLectureNoteStr;
    private OpenAIAPI openAIApi;

    private void Start()
    {
        //저번에 path에서 key 가지고있는 텍스트 파일 가져오는 코드 찾아서 다시 써먹기
        openAIApi = new OpenAIAPI("apikey here");
    }

    public async void makeQuiz() {
        var chat = openAIApi.Chat.CreateConversation();
        chat.Model = Model.ChatGPTTurbo;
        chat.RequestParameters.Temperature = 0; //값이 높을 수록 답이 창의적으로 나오지만 리스크가 큼

        chat.AppendSystemMessage("gpt에세 시스템으로서의 역할과 무엇을 해야하는지 세부적으로 설명");

        //유저랑 어시스턴트 대화 예시
        chat.AppendUserInput("");
        chat.AppendExampleChatbotOutput("");

        //실제로 질문하는 부분
        chat.AppendUserInput("");
        string response = await chat.GetResponseFromChatbotAsync();
        Debug.Log(response);

        foreach (ChatMessage msg in chat.Messages) {
            Debug.Log($"{msg.Role}: {msg.TextContent}");
        }
    }
}
