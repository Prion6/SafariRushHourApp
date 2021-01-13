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

    public static void SetPuzzle(string s)
    {

    }

    public static void LoadScene(int sceneID)
    {
        SceneManager.LoadScene(sceneID);
    }

    public static void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public static void LoadPuzzleScene(int difficulty)
    {
        if(difficulty < 0 || difficulty >= System.Enum.GetValues(typeof(Difficulty)).Length)
        {
            Debug.LogError("Difficulty level not supported");
            return;
        }
        _PHPManager.SetPuzzleScene((Difficulty)difficulty);
    }
    
    public static void SetBackUpPuzzle(int delta)
    {
        Puzzle = GameData.GetPuzzle(delta);
    }

    public static string GetText(string key)
    {
        return GameData.GetText(key);
    }

    public static void RegisterNewPlayer(PlayerData player)
    {
        GameData.PlayerData = player;
    }

    public static void Quit()
    {
        Application.Quit();
    }
}

public enum Difficulty
{
    JUNIOR,
    BEGGINER,
    INTERMEDIATE,
    ADVANCED,
    EXPERT,
    UNDEFINED
}
