using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DataBase/PuzzlePool")]
public class PuzzlePool : ScriptableObject
{
    public List<PuzzleData> puzzles; 
}
