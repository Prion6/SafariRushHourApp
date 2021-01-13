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
    public Dropdown educationalLevel;

    public Slider expertiseRushHour;
    public Slider expertisePuzzle;
    public Slider expertiseMobile;

    public void SetDropDown(string s)
    {
        string[] ops = s.Split('\n');
        educationalLevel.ClearOptions();
        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
        foreach (string o in ops)
        {
            options.Add(new Dropdown.OptionData(o));
        }
        educationalLevel.options = options;
    }

    public PlayerData GetPlayerData()
    {
        return new PlayerData(nickname.text, ageTens.value * 10 + ageUnits.value, (int)expertiseRushHour.value * 100,
            (int)expertisePuzzle.value * 100, (int)expertiseMobile.value * 100, educationalLevel.value);
    }
}
