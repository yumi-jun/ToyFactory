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
    [SerializeField] TMP_Dropdown quizDropdown;
    private string gptKey, quizType;
    private string response;
    
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
        Debug.Log("선택된 옵션" + quizDropdown.options[quizDropdown.value].text);

        var chat = openAIApi.Chat.CreateConversation();
        chat.Model = Model.ChatGPTTurbo;
        chat.RequestParameters.Temperature = 0; //값이 높을 수록 답이 창의적으로 나오지만 리스크가 큼

        chat.AppendSystemMessage("나는 대학교 강의노트의 내용을 퀴즈를 풀면서 공부하고 싶어. 너는 내가 제시한 지문을 바탕으로 퀴즈를 생성해주면 돼. 너의 대답에는 질문, 답, 해설 이 3가지가 필수로 포함되어야 해. 유형은 5지선다형 문제, ox 문제가 있어. 꼭 입력된 문제의 개수에 맞게 문제를 생성해야해."); //gpt에서 시스템으로서의 역할과 무엇을 해야하는지 세부적으로 설명

        //유저랑 어시스턴트 대화 예시
        chat.AppendUserInput("제시된 글을 바탕으로 문제를 2개 만들어줘");
        chat.AppendExampleChatbotOutput("Q. 아주대의 마스코트 캐릭터는 누구일까요?\n\n a) 치토\nb) 기룡이\nc) 넙죽이\nd) 한양이\ne) 눈송이\n\nA. a)\n\n 해설: 아주대의 마스코트는 치토입니다." +
            "\n\nQ. 아주대의 개교기념일은 언제인가요?\n\n a) 4월 11일\nb) 4월 12일\nc) 4월 13일\nd) 4월 14일\ne) 4월 15일\n\nA. a)\n\n 해설: 아주대의 개교기념일은 4월 12일입니다.");
        //chat.AppendUserInput("제시된 글을 바탕으로 문제 2개를 만들어줘. 문제는 5지선다형 1개와 OX형 1개로 구성되어야 해.");
        //chat.AppendExampleChatbotOutput("Q. 아주대의 마스코트 캐릭터는 누구일까요?\n\n a) 치토\nb) 기룡이\nc) 넙죽이\nd) 한양이\ne) 눈송이\n\nA. a)\n\n 해설: 아주대의 마스코트는 치토입니다.\n\n" +
        //    "\n\nQ. 아주대 디지털미디어학과는 팔달관을 사용한다.\n\n A. X\n\n 해설: 아주대 디지털미디어학과는 산학원을 사용합니다.");
        //Q. 질문   , a) b) c) d) e) , A. b)

        await chat.GetResponseFromChatbotAsync();

        //실제로 질문하는 부분
        if (quizDropdown.value != 2)
        {
            chat.AppendUserInput(inputLectureNoteStr + " 이 글을 바탕으로 문제를 " + quizNum.text + "개 만들어줘. 문제의 유형은 " + quizDropdown.options[quizDropdown.value].text + "이고 답은 1개여야해. 해설도 꼭 써줘. 문제는 Q로만 표시하고 번호는 쓰지 말아줘.");
        }
        else {
            chat.AppendUserInput(inputLectureNoteStr + " 이 글을 바탕으로 먼저 답이 1개인 5지선다형 문제 " + (int.Parse(quizNum.text))/2 + "개를 만들어주고, 이어서 OX형 문제 " + (int.Parse(quizNum.text)-(int.Parse(quizNum.text))/2) + "개를 만들어줘. 문제 개수 꼭 지키고 해설도 꼭 써줘. 문제는 Q로만 표시하고 번호는 쓰지 말아줘.");
        }

        response = await chat.GetResponseFromChatbotAsync();
        Debug.Log(response);
        
        GameSceneUserDataManager.Instance().SetQuizString(response);
        

        foreach (ChatMessage msg in chat.Messages) {
            Debug.Log($"{msg.Role}: {msg.TextContent}");
        }
    }

    public string GetQuizText()
    {
        return response;
    }
}
