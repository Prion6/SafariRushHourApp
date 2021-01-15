using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SurveyPanel : MonoBehaviour
{
    public List<Text> texts;

    public InputField nickname;

    public Dropdown ageTens;
    public Dropdown ageUnits;

    public Slider educationalLevel;
    public List<string> educationalOptions;
    public Toggle educationComplete;
    public Text educationalOptionDisplay;

    public Slider expertiseRushHour;
    public Slider expertisePuzzle;
    public Slider expertiseMobile;

    public GameObject nameError;
    public GameObject ageError;


    public void SetDropDown(string s)
    {
        string[] ops = s.Split('\n');
        educationalLevel.maxValue = ops.Length - 1;
        educationalOptions = new List<string>(ops);
    }

    public void SetEducationalOptionText()
    {
        educationalOptionDisplay.text = educationalOptions[(int)educationalLevel.value];
    }

    public PlayerData GetPlayerData()
    {
        return new PlayerData(nickname.text, ageTens.value * 10 + ageUnits.value, (int)expertiseRushHour.value * 100,
            (int)expertisePuzzle.value * 100, (int)expertiseMobile.value * 100, (int)educationalLevel.value);
    }

    public SurveyError CheckPlayerData()
    {
        string s = nickname.text.Trim(' ', '\n', '\t');
        if (s.Equals("") || nickname.text == null)
            return SurveyError.INVALID_NAME;
        if ((ageTens.value * 10 + ageUnits.value) <= GameManager.LowerAge())
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
        GameManager.SetRegistered(true);
        GameManager.RegisterNewPlayer(GetPlayerData());
    }
}

public enum SurveyError
{
    INVALID_NAME,
    INVALID_AGE,
    NONE
}
