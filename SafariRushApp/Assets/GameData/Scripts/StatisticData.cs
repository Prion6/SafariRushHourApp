using System;

[System.Serializable]
public struct StatisticData
{
    public int PuzzleID { get; set; }
    public int PlayerID { get; set; }
    public DateTime Date { get; set; }
    public int Duration { get; set; }
    public string RawMoves { get; set; }
    public int PlayerDifficultyEvaluation { get; set; }
    public int Hintused { get; set; }
    public int Restartused { get; set; }
    public int UndoUsed { get; set; }
    public int PlayerRanking { get; set; }
    public int PuzzleRanking { get; set; }
}
