using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUIManager : MonoBehaviour
{
    [SerializeField] GameObject LobbyListPanel, InLobbyPanel, QuizListPanel, QuizGenerationPanel, LobbyCreationPenel;
    //private bool quizSelected = false; //true여야 버튼 활성화되게 설정

    private void Start()
    {
        LobbyListPanel.SetActive(true);
        InLobbyPanel.SetActive(false);
        QuizListPanel.SetActive(false);
        QuizGenerationPanel.SetActive(false);
        LobbyCreationPenel.SetActive(false);
    }

    public void showInQuizListPanel() {
        InLobbyPanel.SetActive(false);
        QuizGenerationPanel.SetActive(false);
        QuizListPanel.SetActive(true);
    }

    public void showQuizGenerationPanel() {
        QuizListPanel.SetActive(false);
        QuizGenerationPanel.SetActive(true);
    }

    public void showInLobbyPanel() {
        QuizListPanel.SetActive(false);
        QuizGenerationPanel.SetActive(false);
        InLobbyPanel.SetActive(true);
    }

    public void showLobbyCreationPanel() {
        LobbyListPanel.SetActive(false);
        LobbyCreationPenel.SetActive(true);
    }

    public void showLobbyListPanel() {
        LobbyCreationPenel.SetActive(false);
        LobbyListPanel.SetActive(true);
    }
}
