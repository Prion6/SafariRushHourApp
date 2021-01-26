using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinPanelController : MonoBehaviour
{
    public List<Text> texts;
    public List<string> evaluationOptions;
    public Text evaluationTxt;
    public List<string> nextPuzzleOptions;
    public Text nextPuzzleTxt;
    public Slider evaluationSldr;
    public Slider nextPuzzleSldr;
    public List<GameObject> stars;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadLanguage()
    {
        foreach(Text t in texts)
        {
            t.text = GameManager.GetText(t.name);
        }
        SetEvaluationOptions(GameManager.GetText(evaluationTxt.name));
        SetDifficultyOptions(GameManager.GetText(nextPuzzleTxt.name));
        SetEvaluationOptionText();
        SetDifficultyOptionText();
    }

    public void SetEvaluationOptions(string s)
    {
        string[] ops = s.Split('\n');
        evaluationSldr.maxValue = ops.Length - 1;
        evaluationOptions = new List<string>(ops);
    }

    public void SetDifficultyOptions(string s)
    {
        string[] ops = s.Split('\n');
        nextPuzzleSldr.maxValue = ops.Length - 1;
        nextPuzzleOptions = new List<string>(ops);
    }

    public void SetEvaluationOptionText()
    {
        evaluationTxt.text = evaluationOptions[(int)evaluationSldr.value];
    }

    public void SetDifficultyOptionText()
    {
        nextPuzzleTxt.text = nextPuzzleOptions[(int)nextPuzzleSldr.value];
    }

    public double GetPlayerEvaluation()
    {
        return evaluationSldr.value;
    }

    public double GetPlayerPreference()
    {
        Debug.Log(nextPuzzleSldr.value - nextPuzzleOptions.Count / 2f);
        return nextPuzzleSldr.value + 1 - nextPuzzleOptions.Count/2f;
    }

    public void DisplayPerformance(double proficiency)
    {
        int rating = (int)(proficiency * stars.Count);
        for(int i = 0; i < rating; i++)
        {
            stars[i].SetActive(true);
        }
    }
}
