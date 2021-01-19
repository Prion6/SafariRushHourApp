using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager
{
    private static PHPManager _PHPManager;
    private static PHPManager PHPManager {
        get
        {
            if(_PHPManager == null)
            {
                GameObject go = Resources.Load("PHPManager") as GameObject;
                _PHPManager = GameObject.Instantiate(go).GetComponent<PHPManager>();
            }
            return _PHPManager;
        }
    }

    private static GameData _GameData;
    private static GameData GameData
    {
        get
        {
            if(_GameData == null)
            {
                _GameData = new GameData();
            }
            return _GameData;
        }
    }
    
    public static PuzzleData Puzzle { get; set; }

    public static int LowerAge
    {
        get { return GameData.LowerAge; }
    }

    public static int PlayerID
    {
        get { return GameData.PlayerData.ID; }
    }

    public static int PlayerRanking
    {
        get { return GameData.PlayerData.Ranking; }
    }
    
    public static void LoadScene(int sceneID)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneID);
    }

    public static void LoadScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    public static void LoadPuzzleScene(int delta)
    {
        if(GameData.PlayerData.Ranking + delta < 0)
        {
            Debug.LogError("Difficulty level not supported");
            return;
        }
        PHPManager.SetPuzzleScene(GameData.PlayerData.ID, delta);
    }
    
    public static void SetBackUpPuzzle(int delta)
    {
        Puzzle = GameData.GetPuzzle(GameData.PlayerData.Ranking + delta);
    }

    public static string GetText(string key)
    {
        return GameData.GetText(key);
    }

    public static bool IsRegistered
    {
        get { return GameData.Registered; }
        set { GameData.Registered = value; }
    }

    public static void RegisterNewPlayer(PlayerData player)
    {
        GameData.PlayerData = player;
        PHPManager.RegisterUser(player);
    }

    public static void RegisterGame(StatisticData data)
    {
        PHPManager.RegisterGame(data);
    }

    public static void StoreGameData(StatisticData data)
    {
        GameData.AddGameData(data);
    }

    public static void FreeGameData(StatisticData data)
    {
        GameData.FreeGameData(data);
    }

    public static void UpdateData()
    {
        if(PlayerID == -1)
        {
            PHPManager.RegisterUser(GameData.PlayerData);
        }
        for(int i = 0; i < GameData.statistics.Count; i++)
        {
            RegisterGame(GameData.statistics[i]);
        }
    }

    public static void Quit()
    {
        OnQuit();
        Application.Quit();
    }

    public static void OnQuit()
    {
        GameData.SaveData();
    }

}
