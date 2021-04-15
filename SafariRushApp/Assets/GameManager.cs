using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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

    public static UnityEvent OnLanguageChange = new UnityEvent();
    public static UnityEvent OnVolumeChange = new UnityEvent();

    public static void SetLanguages(List<TextDataBase> l)
    {
        GameData.languages = l;
    }
    
    public static PuzzleData Puzzle { get; set; }

    //public static OptionsData Options { get { return GameData.Options; } set {GameData.Options = value;} }

    public static int SelectionOffset { get { return GameData.selectionOffset; } set { GameData.selectionOffset = value; } }
    
    public static float Volume {
        get { return GameData.Options.Volume; }
        set {
            GameData.Options.Volume = value;
            OnVolumeChange.Invoke();
        } }

    public static int Speed { get { return GameData.Options.Speed; } set { GameData.Options.Speed = value; } }

    public static Language Language { get {
            return GameData.Options.Language; }
        set {
            GameData.Options.Language = value;
            OnLanguageChange.Invoke(); } }

    public static void SetLanguage(Language l)
    {
        GameData.SetLanguage(l);
        OnLanguageChange?.Invoke();
    }

    public static PlayerData PlayerData { get { return GameData.PlayerData; } }

    public static int LowerAge
    {
        get { return GameData.LowerAge; }
    }

    public static int PlayerID
    {
        get { return GameData.PlayerData.ID; }
        set { GameData.PlayerData.ID = value; }
    }

    public static int PlayerRanking
    {
        get { return GameData.PlayerData.Ranking; }
        set { GameData.PlayerData.Ranking = value; }
    }

    public static void SetPuzzleRanking(int ranking, int id)
    {
        GameData.SetPuzzleRanking(ranking, id);
    }
    
    public static void LoadScene(int sceneID)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneID);
    }

    public static void LoadScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    public static void LoadPuzzleScene(int ranking)
    {
        PHPManager.GetPuzzle(GameData.PlayerData.ID, ranking);
    }
    
    public static void SetBackUpPuzzle(int ranking)
    {
        Puzzle = GameData.GetPuzzle(ranking);
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

    public static void SetPlayerData(int age, int educationalLevel)
    {
        GameData.PlayerData.Age = age;
        GameData.PlayerData.EducationalLevel = educationalLevel;
    }

    public static void RegisterNewPlayer(PlayerData player)
    {
        GameData.PlayerData = player;
        PHPManager.RegisterUser(player);
        GameData.SaveData();
    }

    public static void RegisterGame(StatisticData data)
    {
        PHPManager.RegisterGame(data);
    }

    public static void GetPlayerInfo(System.Action<string> fetchPlayerInfo)
    {
        PHPManager.GetPlayerInfo(fetchPlayerInfo);
    }

    public static void GetLeaderBoard(System.Action<string> fetchLeaderBoard)
    {
        PHPManager.GetLeaderBoard(fetchLeaderBoard);
    }

    public static void StoreGameData(StatisticData data)
    {
        GameData.AddGameData(data);
    }

    public static void FreeGameData(StatisticData data)
    {
        GameData.FreeGameData(data);
    }

    public static void GetHint(string puzzle, System.Action<bool> isPaused, System.Action<string> getHint)
    {
        PHPManager.GetHint(puzzle, isPaused, getHint);
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
