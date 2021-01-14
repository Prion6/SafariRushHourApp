using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerData
{
    public PlayerData(string nick, int age, int rushHour, int puzzle, int mobile, int educatinal)
    {
        ID = 0;
        Nickname = nick;
        Ranking = 0;
        Age = age;
        RushHourExpertise = rushHour;
        PuzzleGameExpertise = puzzle;
        MobileGameExpertise = mobile;
        EducationalLevel = educatinal;
    }

    public int ID { get; set; }
    public string Nickname { get; set; }
    public int Ranking { get; set; }
    public int Age { get; set; }
    public int RushHourExpertise { get; set; }
    public int PuzzleGameExpertise { get; set; }
    public int MobileGameExpertise { get; set; }
    public int EducationalLevel { get; set; }
}
