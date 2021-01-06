using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MenuManager
{
    public void PlayLevel(int difficulty)
    {
        GameManager.LoadPuzzleScene(difficulty);
    }
}
