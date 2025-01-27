using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PlayerRankingInfo : MonoBehaviour
{
    public Text playerName;
    public Text playerRanking;
    public Text playerMatches;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Settext(string name, int ranking, int matches)
    {
        playerName.text = name;
        playerRanking.text = ranking.ToString();
        playerMatches.text = matches.ToString();
    }
}
