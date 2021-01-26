using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PlayerData
{
    public PlayerData(string nick, int age, int rushHour, int puzzle, int mobile, int educatinal)
    {
        ID = 0;
        Nickname = nick;
        Ranking = 1000;
        Age = age;
        RushHourExpertise = rushHour;
        PuzzleGameExpertise = puzzle;
        MobileGameExpertise = mobile;
        EducationalLevel = educatinal;
        Matches = 0;
    }

    public int ID { get; set; }
    public string Nickname { get; set; }
    public int Ranking { get; set; }
    public int Age { get; set; }
    public int RushHourExpertise { get; set; }
    public int PuzzleGameExpertise { get; set; }
    public int MobileGameExpertise { get; set; }
    public int EducationalLevel { get; set; }
    public int Matches { get; set; }
}
