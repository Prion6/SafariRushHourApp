using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PHPManager : MonoBehaviour
{
    PHPQuerySet querier;

    // Start is called before the first frame update
    void Awake()
    {
        querier = Resources.Load("PHP Query Set") as PHPQuerySet;
    }

    public void Test()
    {
        StartCoroutine(querier.Testing("Test"));
    }

    public void SetPuzzleScene(DIFFICULTY difficulty)
    {
        StartCoroutine(GetPuzzle(difficulty));
    }
    
    public IEnumerator GetPuzzle(DIFFICULTY difficulty)
    {
        yield return StartCoroutine(querier.GetPuzzle("GetPuzzle", difficulty, (s) => GameManager.Puzzle = s));
        //Debug.Log(GameManager.Puzzle);
        GameManager.LoadScene("Puzzle");
    }
}
