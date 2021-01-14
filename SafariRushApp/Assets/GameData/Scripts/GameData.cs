using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    public PlayerData PlayerData { get; set; }
    public List<StatisticData> statistics;
    public OptionsData Options { get; set; }

    public List<PuzzleData> Puzzles;
    public List<TextDataBase> languages;

    public bool FirstEntry { get; set; }
    private string playerDataPath;
    private string statisticsPath;
    private string optionsPath;
    private string entryPath;

    public GameData()
    {
        FirstEntry = true;
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
        Puzzles = new List<PuzzleData>((Resources.Load("Puzzles") as PuzzlePool).puzzles);
    }

    public string GetText(string key)
    {
        foreach(TextDataBase tdb in languages)
        {
            if(tdb.langauge.Equals(Options.Langauge))
            {
                return tdb.Dictionary[key];
            }
        }
        return "Text not found";
    }
    
    public PuzzleData GetPuzzle(int delta)
    {
        //Gotta change
        return Puzzles[0];
    }

    public void LoadData()
    {
        FirstEntry = DataManager.LoadData<bool>(entryPath);
        if(FirstEntry)
        {
            PlayerData = DataManager.LoadData<PlayerData>(playerDataPath);
            statistics = DataManager.LoadData<List<StatisticData>>(statisticsPath);
            Options = DataManager.LoadData<OptionsData>(optionsPath);
        }
    }

    public void SaveData()
    {
        DataManager.SaveData(FirstEntry, entryPath);
        DataManager.SaveData(PlayerData, playerDataPath);
        DataManager.SaveData(statistics, statisticsPath);
        DataManager.SaveData(Options, optionsPath);
    }
}
