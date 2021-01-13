using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    public PlayerData PlayerData { get; set; }
    public List<StatisticData> statistics;
    public OptionsData Options { get; set; }
    public bool FirstEntry { get; set; }
    public List<PuzzleData> Puzzles;
    public List<TextDataBase> languages;

    public GameData()
    {
        Load();
    }

    public void Load()
    {
        LoadPuzzlePool();
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
}
