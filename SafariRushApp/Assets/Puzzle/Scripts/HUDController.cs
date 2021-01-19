using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HUDController : MonoBehaviour
{
    public List<Text> texts;
    public Text timeCounterTxt;
    public Text movesCounterTxt;

    // Start is called before the first frame update
    void Start()
    {
        SetMoves(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTime(double seconds)
    {
        timeCounterTxt.text = ((int)seconds).ToString();
    }

    public void SetMoves(int moves)
    {
        movesCounterTxt.text = moves.ToString();
    }

    public void LoadLanguage()
    {
        foreach(Text t in texts)
        {
            t.text = GameManager.GetText(t.name);
        }
    }
}
