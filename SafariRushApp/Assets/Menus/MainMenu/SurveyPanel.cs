﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
    
public class SurveyPanel : MultiLanguageUI
{
    public InputField nickname;

    public Dropdown ageTens;
    public Dropdown ageUnits;

    public Slider educationalLevel;
    public List<string> educationalOptions;
    public Toggle educationComplete;
    public Text educationalOptionDisplay;

    public Slider expertiseRushHour;
    public List<string> expertiseRushHourOptions;
    public Text rhExpertiseDisplay;
    public Slider expertisePuzzle;
    public List<string> expertisePuzzleOptions;
    public Text pExpertiseDisplay;
    public Slider expertiseMobile;
    public List<string> expertiseMobileOptions;
    public Text mExpertiseDisplay;

    public GameObject nameError;
    public GameObject ageError;

    public Image image;

    private void Start()
    {
        SetRHExpertiseDisplay();
        SetPExpertiseDisplay();
        SetMExpertiseDisplay();
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

    public PlayerData GetPlayerData()
    {
        int i = 0;
        if (educationComplete.isOn)
            i = 1;

        return new PlayerData(nickname.text, ageTens.value * 10 + ageUnits.value, (int)(expertiseRushHour.value),
            (int)(expertisePuzzle.value), (int)(expertiseMobile.value), (int)educationalLevel.value*2 + i);
    }

    public SurveyError CheckPlayerData()
    {
        string s = nickname.text.Trim(' ', '\n', '\t');
        if (s.Equals("") || nickname.text == null)
            return SurveyError.INVALID_NAME;
        if ((ageTens.value * 10 + ageUnits.value) <= GameManager.LowerAge)
            return SurveyError.INVALID_AGE;
        return SurveyError.NONE;
    }

    public void Continue()
    {
        nameError.SetActive(false);
        ageError.SetActive(false);
        switch (CheckPlayerData())
        {
            case SurveyError.INVALID_NAME:
                nameError.SetActive(true);
                break;
            case SurveyError.INVALID_AGE:
                ageError.SetActive(true);
                break;
            case SurveyError.NONE: RegisterNewPlayer();
                gameObject.SetActive(false);
                break;
        }
    }

    public void RegisterNewPlayer()
    {
        GameManager.IsRegistered = true;
        GameManager.RegisterNewPlayer(GetPlayerData());
        if (GameManager.IsRegistered) image.color = Color.blue;
    }

    public override void LoadLanguage()
    {
        base.LoadLanguage();
        SetEducationalOptions(GameManager.GetText(educationalOptionDisplay.name));
        SetRHExpertiseOptions(GameManager.GetText(rhExpertiseDisplay.name));
        SetPExpertiseOptions(GameManager.GetText(pExpertiseDisplay.name));
        SetMExpertiseOptions(GameManager.GetText(mExpertiseDisplay.name));
    }

    public void SetRHExpertiseDisplay()
    {
        rhExpertiseDisplay.text = expertiseRushHourOptions[(int)expertiseRushHour.value];
    }

    public void SetPExpertiseDisplay()
    {
        pExpertiseDisplay.text = expertisePuzzleOptions[(int)expertisePuzzle.value];
    }

    public void SetMExpertiseDisplay()
    {
        mExpertiseDisplay.text = expertiseMobileOptions[(int)expertiseMobile.value];
    }

    public void SetRHExpertiseOptions(string s)
    {
        string[] ops = s.Split('\n');
        expertiseRushHour.maxValue = ops.Length - 1;
        expertiseRushHourOptions = new List<string>(ops);
        SetRHExpertiseDisplay();
    }

    public void SetPExpertiseOptions(string s)
    {
        string[] ops = s.Split('\n');
        expertisePuzzle.maxValue = ops.Length - 1;
        expertisePuzzleOptions = new List<string>(ops);
        SetPExpertiseDisplay();
    }

    public void SetMExpertiseOptions(string s)
    {
        string[] ops = s.Split('\n');
        expertiseMobile.maxValue = ops.Length - 1;
        expertiseMobileOptions = new List<string>(ops);
        SetMExpertiseDisplay();
    }
}

public enum SurveyError
{
    INVALID_NAME,
    INVALID_AGE,
    NONE
}
