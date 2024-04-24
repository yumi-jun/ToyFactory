using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{
    private static SceneLoader instance = new SceneLoader();

    private SceneLoader()
    {
        
    }

    public static SceneLoader Instance()
    {
        return instance;
    }
    void Awake()
    {
        if (instance == null)
        {
            // 씬 전환 시 파괴되지 않도록 설정
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {
            // 이미 인스턴스가 존재하면 이전 인스턴스를 파괴
            Destroy(gameObject);
        }
    }
    public void LoadNextScene()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void LoadQuizScene()
    {
        SceneManager.LoadScene("QuizScene");
    }
}
