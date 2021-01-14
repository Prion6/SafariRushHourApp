using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PHPManager : MonoBehaviour
{
    PHPQuerySet querier;
    private List<QuerieCoroutine> queries;
    
    private double fetchTimeOut;
    private double sendTimeOut;

    // Start is called before the first frame update
    void Awake()
    {
        queries = new List<QuerieCoroutine>();
        querier = Resources.Load("PHP Query Set") as PHPQuerySet;
    }

    private void Update()
    {
        for (int i = 0; i < queries.Count; i++)
        {
            if (!queries[i].Running)
            {
                queries.RemoveAt(i);
                continue;
            }
            queries[i].TimeOut -= Time.deltaTime;
            if (queries[i].TimeOut <= 0)
            {
                StopCoroutine(queries[i].Coroutine);
                queries[i].Abort();
                queries.RemoveAt(i);
            }
        }
    }

    ///PUZZLES///
    public void SetPuzzleScene(int ranking)
    {
        QuerieCoroutine c = new QuerieCoroutine(StartCoroutine(GetPuzzle(ranking)), fetchTimeOut, 
                                                new System.Action(() => AbortSetPuzzleScene(ranking)));
        queries.Add(c);
    }

    private void AbortSetPuzzleScene(int ranking)
    {
        GameManager.SetBackUpPuzzle(ranking);
        GameManager.LoadScene("Puzzle");
    }
    
    public IEnumerator GetPuzzle(int ranking)
    {
        string puzzle = null;
        yield return StartCoroutine(querier.GetPuzzle("GetPuzzle", ranking, (s) => puzzle = s));
        if (puzzle != null)
        {
            GameManager.SetPuzzle(puzzle);
        }
        else
        {
            GameManager.SetBackUpPuzzle(ranking);
        }
        GameManager.LoadScene("Puzzle");
    }


    ///ADDPlayer///
    public void RegisterUser(PlayerData playerData)
    {
        QuerieCoroutine c = new QuerieCoroutine(StartCoroutine(AddPlayer(playerData, (i) => playerData.ID = i)), sendTimeOut,
                                                new System.Action(() => AbortRegisterUser()));
        queries.Add(c);
    }

    public void AbortRegisterUser()
    {
        Debug.Log("Couldn't Add Player to the DataBase");
    }

    public IEnumerator AddPlayer(PlayerData playerData, System.Action<int> callback)
    {
        int id = -1;
        bool succes = false;
        yield return StartCoroutine(querier.RegisterPlayer("RegisterPlayer",playerData,(s) => succes = int.TryParse(s, out id)));
        if (succes)
            callback(id);
    }

}

public class QuerieCoroutine
{
    public bool Running { get; set; }
    public Coroutine Coroutine { get; set; }
    public double TimeOut { get; set; }
    public System.Action AbortFunction { get; set; }

    public QuerieCoroutine(Coroutine coroutine, double timeOut, System.Action abortFunction)
    {
        Running = true;
        Coroutine = coroutine;
        TimeOut = timeOut;
        AbortFunction = abortFunction;
    }

    public void Abort()
    {
        Debug.Log("Coroutine Aborted: " + Coroutine.ToString());
        AbortFunction.Invoke();
    }
}
