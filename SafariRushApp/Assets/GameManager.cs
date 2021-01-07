﻿using System.Collections;
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
        if(difficulty < 0 || difficulty >= System.Enum.GetValues(typeof(DIFFICULTY)).Length)
        {
            Debug.LogError("Difficulty level not supported");
            return;
        }
        Debug.Log("Load " + (DIFFICULTY)difficulty + " map");
        _PHPManager.SetPuzzleScene((DIFFICULTY)difficulty);
    }
        
}

public enum DIFFICULTY
{
    BEGGINER,
    JUNIOR,
    INTERMEDIATE,
    ADVANCED,
    EXPERT
}