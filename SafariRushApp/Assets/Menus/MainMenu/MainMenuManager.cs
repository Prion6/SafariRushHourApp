using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : SceneManager
{
    public GameObject disclaimer;
    public SurveyPanel surveyPanel;
    public Text playBtnTxt;

    private void Start()
    {
        disclaimer.SetActive(!GameManager.IsRegistered());
    }

    public void PlayLevel()
    {
        GameManager.LoadPuzzleScene(0);
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
           
}
