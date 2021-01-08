﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PHPManager : MonoBehaviour
{
    PHPQuerySet querier;

    public double getPuzzleTimeOut;
    private bool gettingPuzzle;
    private Difficulty puzzleDifficulty;
    private double timeOut;

    // Start is called before the first frame update
    void Awake()
    {
        querier = Resources.Load("PHP Query Set") as PHPQuerySet;
    }

    public void Test()
    {
        StartCoroutine(querier.Testing("Test"));
    }

    public void SetPuzzleScene(Difficulty difficulty)
    {
        gettingPuzzle = true;
        timeOut = getPuzzleTimeOut;
        puzzleDifficulty = difficulty;
        StartCoroutine(GetPuzzle(difficulty));
    }

    private void AbortSetPuzzleScene(Difficulty difficulty)
    {
        Debug.Log("Aborted");
        StopCoroutine(GetPuzzle(difficulty));
        GameManager.SetBackUpPuzzle((int)difficulty);
        GameManager.LoadScene("Puzzle");
    }
    
    public IEnumerator GetPuzzle(Difficulty difficulty)
    {
        yield return StartCoroutine(querier.GetPuzzle("GetPuzzle", difficulty, (s) => GameManager.Puzzle = s));
        if(GameManager.Puzzle != null)
            gettingPuzzle = false;
        GameManager.SetBackUpPuzzle((int)difficulty);
        //Debug.Log(GameManager.Puzzle);
        GameManager.LoadScene("Puzzle");
    }

    private void Update()
    {
        if(gettingPuzzle)
        {
            timeOut -= Time.deltaTime;
            if(timeOut <= 0)
            {
                AbortSetPuzzleScene(puzzleDifficulty);
            }
        }
    }
}
