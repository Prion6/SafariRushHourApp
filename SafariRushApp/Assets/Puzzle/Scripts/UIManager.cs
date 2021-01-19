using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public WinPanelController winPanel;
    public HUDController hud;
    PuzzleManager puzzleManager;
    // Start is called before the first frame update
    void Start()
    {
        LoadLanguage();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadLanguage()
    {
        foreach(Text t in winPanel.texts)
        {
            GameManager.GetText(t.name);
        }
        foreach (Text t in hud.texts)
        {
            GameManager.GetText(t.name);
        }
    }
}
