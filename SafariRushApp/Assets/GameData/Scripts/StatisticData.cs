using System;

[System.Serializable]
public struct StatisticData
{
    public StatisticData(int puzzleID, int playerID, DateTime date, int duration, string rawMoves, 
        int playerDifficultyEvaluation, int hintused, int restartused, int undoUsed, float proficiency)
    {
        PuzzleID = puzzleID;
        PlayerID = playerID;
        Date = date;
        Duration = duration;
        RawMoves = rawMoves;
        PlayerDifficultyEvaluation = playerDifficultyEvaluation;
        HintUsed = hintused;
        RestartUsed = restartused;
        UndoUsed = undoUsed;
        Proficiency = proficiency;
    }

    public int PuzzleID { get; set; }
    public int PlayerID { get; set; }
    public DateTime Date { get; set; }
    public int Duration { get; set; }
    public string RawMoves { get; set; }
    public int PlayerDifficultyEvaluation { get; set; }
    public int HintUsed { get; set; }
    public int RestartUsed { get; set; }
    public int UndoUsed { get; set; }
    public float Proficiency { get; set; }
}
