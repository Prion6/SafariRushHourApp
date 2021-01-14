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
    private string playerDataPath = "Player";
    private string statisticsPath = "Statistics";
    private string optionsPath = "Options";
    private string entryPath = "Entry";
    private string puzzlesPath = "Puzzles";

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
        if(!FirstEntry)
        {
            PlayerData = DataManager.LoadData<PlayerData>(playerDataPath);
            statistics = DataManager.LoadData<List<StatisticData>>(statisticsPath);
            Options = DataManager.LoadData<OptionsData>(optionsPath);
        }
    }

    public void SaveData()
    {
        try
        {
            DataManager.SaveData(FirstEntry, entryPath);
        }
        catch
        {
            Debug.Log("Entry not saved");
        }

        try
        {
            DataManager.SaveData(PlayerData, playerDataPath);
        }
        catch
        {
            Debug.Log("Player not saved");
        }

        try
        {
            DataManager.SaveData(statistics, statisticsPath);
        }
        catch
        {
            Debug.Log("Statistics not saved");
        }

        try
        {
            DataManager.SaveData(Options, optionsPath);
        }
        catch
        {
            Debug.Log("Options not saved");
        }

        try
        {
            DataManager.SaveData(Puzzles, puzzlesPath);
        }
        catch
        {
            Debug.Log("Puzzles not saved");
        }
    }
}
