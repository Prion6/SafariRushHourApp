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

    public Vector2 GetData()
    {
        return new Vector2(evaluationSldr.value, nextPuzzleSldr.value - nextPuzzleOptions.Count/2);
    }
}
