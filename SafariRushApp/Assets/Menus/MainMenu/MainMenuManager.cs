using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : SceneManager
{
    public GameObject disclaimer;
    public Text disclaimerTxt;
    public SurveyPanel surveyPanel;
    public List<TextDataBase> languages;

    public void Awake()
    {
        disclaimer.SetActive(false);
    }

    private void Start()
    {
        disclaimer.SetActive(!GameManager.IsRegistered);
        LoadLanguage();
        GameManager.OnLanguageChange.AddListener(LoadLanguage);
    }

    public void PlayLevel()
    {
        GameManager.LoadPuzzleScene(0);
    }

    public void LoadLanguage()
    {
        if(!GameManager.IsRegistered)
        {
            surveyPanel.LoadLanguage();
            disclaimerTxt.text = GameManager.GetText(disclaimerTxt.name);
        }
    }
           
}
