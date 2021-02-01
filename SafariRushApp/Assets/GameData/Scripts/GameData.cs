using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;



public class GameData
{
    public PlayerData PlayerData;
    public List<StatisticData> statistics;
    public OptionsData Options;

    public List<PuzzleData> Puzzles;
    public List<TextDataBase> languages;

    public bool Registered { get; set; }
    private string playerDataPath = "Player";
    private string statisticsPath = "Statistics";
    private string optionsPath = "Options";
    private string entryPath = "Entry";
    private string puzzlesPath = "Puzzles";

    public int LowerAge = 5;
    public int selectionOffset = 200;

    

    public GameData()
    {
        Registered = false;
        statistics = new List<StatisticData>();
        Load();
    }

    public void Load()
    {
        LoadPuzzlePool();
        LoadLanguages();
        LoadData();
    }

    public void LoadLanguages()
    {
        languages = new List<TextDataBase>();
        Object[] objects = Resources.FindObjectsOfTypeAll(typeof(TextDataBase));
        foreach(Object o in objects)
        {
            languages.Add(o as TextDataBase);
        }
    }

    public void LoadPuzzlePool()
    {
        Puzzles = DataManager.LoadData<List<PuzzleData>>(puzzlesPath);
        if(Puzzles == null)
        {
            Puzzles = new List<PuzzleData>((Resources.Load("Puzzles") as PuzzlePool).puzzles);
        }
    }

    public string GetText(string key)
    {
        foreach(TextDataBase tdb in languages)
        {
            if(tdb.langauge.Equals(Options.Language))
            {
                return tdb.Dictionary[key];
            }
        }
        return "Text not found";
    }
    
    public PuzzleData GetPuzzle(int ranking)
    {
        //var ops = Puzzles.Where( p => p.Ranking >= ranking - selectionOffset || p.Ranking <= ranking - selectionOffset);
        var ops = Puzzles.OrderBy(p => Mathf.Abs(p.Ranking - ranking));
        var arr = ops.ToArray();
        //System.Array.Reverse(arr);
        int limit = 7;
        if (arr.Length < limit)
        {
            limit = arr.Length;
        }
        int i = Random.Range(0, limit);
        return arr[i];
    }

    public void FreeGameData(StatisticData data)
    {
        if(statistics.Contains(data))
        {
            statistics.Remove(data);
        }
    }

    public void AddGameData(StatisticData data)
    {
        if (!statistics.Contains(data))
        {
            statistics.Add(data);
        }
    }

    public void SetVolume(int i)
    {
        Options.Volume = i;
    }

    public void SetPuzzleRanking(int ranking, int id)
    {
        Puzzles.Where((p) => p.ID == id).ToList().ForEach((x) => x.Ranking = ranking);
    }

    public void SetSpeed(int i)
    {
        Options.Speed = i;
    }

    public void SetLanguage(Language l)
    {
        Options.Language = l;
    }

    public void LoadData()
    {
        Registered = DataManager.LoadData<bool>(entryPath);
        if(Registered)
        {
            PlayerData = DataManager.LoadData<PlayerData>(playerDataPath);  
            statistics = DataManager.LoadData<List<StatisticData>>(statisticsPath);
            Options = DataManager.LoadData<OptionsData>(optionsPath);
        }
        else
        {
            Options = new OptionsData(30,5,Language.ENGLISH);
        }
    }

    public void SaveData()
    {
        DataManager.SaveData(Registered, entryPath);
        DataManager.SaveData(PlayerData, playerDataPath);
        DataManager.SaveData(statistics, statisticsPath);
        DataManager.SaveData(Options, optionsPath);
        DataManager.SaveData(Puzzles, puzzlesPath);
    }
}
