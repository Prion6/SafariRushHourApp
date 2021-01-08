using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager
{
    private static PHPManager PHPManager;
    private static PHPManager _PHPManager {
        get
        {
            if(PHPManager == null)
            {
                GameObject go = Resources.Load("PHPManager") as GameObject;
                PHPManager = GameObject.Instantiate(go).GetComponent<PHPManager>();
            }
            return PHPManager;
        }
    }
    
    public static string Puzzle { get; set; }

    private static List<PuzzlePool> Puzzles;
    private static List<PuzzlePool> _Puzzles
    {
        get
        {
            if(Puzzles == null)
            {
                Puzzles = new List<PuzzlePool>();
                Puzzles.Add(Resources.Load("JuniorPuzzles") as PuzzlePool);
                Puzzles.Add(Resources.Load("BeginnerPuzzles") as PuzzlePool);
                Puzzles.Add(Resources.Load("IntermediatePuzzles") as PuzzlePool);
                Puzzles.Add(Resources.Load("AdvancedPuzzles") as PuzzlePool);
                Puzzles.Add(Resources.Load("ExpertPuzzles") as PuzzlePool);
            }
            return Puzzles;
        }
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

    public static string GetBackUpPuzzle(int difficulty)
    {
        return _Puzzles[difficulty].GetRandom();
    }

    public static void SetBackUpPuzzle(int difficulty)
    {
        Puzzle = GetBackUpPuzzle(difficulty);
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
