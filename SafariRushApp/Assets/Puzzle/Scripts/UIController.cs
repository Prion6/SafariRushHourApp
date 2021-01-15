using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIController : MonoBehaviour
{
    public Text timeTxt;
    public Text timeCounterTxt;
    public Text movesTxt;
    public Text movesCounterTxt;

    // Start is called before the first frame update
    void Start()
    {
        
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
}
