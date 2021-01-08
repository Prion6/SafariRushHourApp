﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DataBase/PuzzlePool")]
public class PuzzlePool : ScriptableObject
{
    [TextArea]
    public List<string> puzzles;

    public string GetRandom()
    {
        return puzzles[Random.Range(0,puzzles.Count)];
    }
}
