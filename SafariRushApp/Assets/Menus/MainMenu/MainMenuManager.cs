using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : SceneManager
{
    public GameObject disclaimer;
    public Text disclaimerTxt;
    public SurveyPanel surveyPanel;
    public Text playBtnTxt;

    private void Start()
    {
        disclaimer.SetActive(!GameManager.IsRegistered);
        LoadLanguage();
    }

    public void PlayLevel()
    {
        GameManager.LoadPuzzleScene(0);
    }

    public void LoadLanguage()
    {
        disclaimerTxt.text = GameManager.GetText(disclaimerTxt.name);
        foreach(Text t in surveyPanel.texts)
        {
            t.text = GameManager.GetText(t.name);
        }
        surveyPanel.SetEducationalOptions(GameManager.GetText(surveyPanel.educationalOptionDisplay.name));
        playBtnTxt.text = GameManager.GetText(playBtnTxt.name);
    }
           
}
