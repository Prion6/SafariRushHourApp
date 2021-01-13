using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MenuManager
{
    public SurveyPanel surveyPanel;
    public Text playBtnTxt;

    public void PlayLevel(int difficulty)
    {
        GameManager.LoadPuzzleScene(difficulty);
    }

    public void LoadLanguage()
    {
        foreach(Text t in surveyPanel.texts)
        {
            t.text = GameManager.GetText(t.name);
        }
        surveyPanel.SetDropDown(GameManager.GetText(surveyPanel.educationalLevel.name));
        playBtnTxt.text = GameManager.GetText(playBtnTxt.name);
    }

    public void RegisterNewPlayer()
    {
        GameManager.RegisterNewPlayer(surveyPanel.GetPlayerData());
    }
}
