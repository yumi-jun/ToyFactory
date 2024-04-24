using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonManager : MonoBehaviour
{

    private UserData data;
    private GameSceneUserDataManager _gameSceneUserDataManager;
    // Start is called before the first frame update
    void Start()
    {
        _gameSceneUserDataManager = FindObjectOfType<GameSceneUserDataManager>();
        GetJsonData();
    }

    public void GetJsonData()
    {
        data = _gameSceneUserDataManager.GetUserdata();
        Debug.Log(data.id+" + "+data.username);
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
