using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerMenu : MultiLanguageUI
{
    public List<PlayerRankingInfo> players;
    public Slider educationalLevel;
    public List<string> educationalOptions;
    public Text educationalOptionDisplay;
    public Toggle complete;
    public Dropdown AgeU;
    public Dropdown AgeT;

    // Start is called before the first frame update
    void OnEnable()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init()
    {
        SetEducationalOptions(GameManager.GetText(educationalOptionDisplay.name));
        LoadLanguage();
        int educational = GameManager.PlayerData.EducationalLevel;
        int age = GameManager.PlayerData.Age;

        educationalLevel.value = educational / 2;
        complete.isOn = (educational % 2 == 1);

        AgeU.value = age % 10;
        AgeT.value = age / 10;

        GetPlayerData();
        GetLeaderBoard();
    }

    public void GetPlayerData()
    {
        string name = GameManager.PlayerData.Nickname;
        string ranking = "";
        string matches = "";
        GameManager.GetPlayerInfo((s) =>
        {
            string[] data = s.Split(';');
            ranking = data[0];
            matches = data[1];
            SetPlayer(0, name, ranking, matches);
        });
    }


    public void SetEducationalOptions(string s)
    {
        string[] ops = s.Split('\n');
        educationalLevel.maxValue = ops.Length - 1;
        educationalOptions = new List<string>(ops);
        SetEducationalOptionText();
    }

    public void SetEducationalOptionText()
    {
        educationalOptionDisplay.text = educationalOptions[(int)educationalLevel.value];
    }

    public void GetLeaderBoard()
    {
        GameManager.GetLeaderBoard((s) =>
        {
            string[] players = s.Split('|');
            for (int i = 0; (i < players.Length - 1 && i < 3) ; i++)
            {
                string[] data = players[i].Split(';');
                SetPlayer(i + 1, data[0], data[1], data[2]);
            }
        });
    }

    public void SetPlayer(int i, string name, string ranking, string matches)
    {
        //Debug.Log(ranking + "|" + matches);
        players[i].Settext(name, int.Parse(ranking), int.Parse(matches)); 
    }

    public void ChangeData()
    {
        int age = AgeT.value * 10 + AgeU.value;
        int educational = (int)educationalLevel.value * 2 + (complete.isOn ? 1 : 0);
        GameManager.SetPlayerData(age, educational);
    }
}
