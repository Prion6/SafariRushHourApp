using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PHPManager : MonoBehaviour
{
    PHPQuerySet querier;
    private List<QuerieCoroutine> queries;
    
    public  double fetchTimeOut;
    public double sendTimeOut;

    private bool conected;
    private bool conectedLastFrame;

    // Start is called before the first frame update
    void Awake()
    {
        conected = false;
        conectedLastFrame = false;
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

    private void LateUpdate()
    {
        conectedLastFrame = conected;
        conected = !(Application.internetReachability == NetworkReachability.NotReachable);
        if (!conectedLastFrame && !conected)
        {
            foreach (QuerieCoroutine q in queries)
            {
                StopCoroutine(q.Coroutine);
                q.Abort();
            }
        }
    }

    ///PUZZLES///
    public void SetPuzzleScene(int id, int delta)
    {
        QuerieCoroutine c = new QuerieCoroutine(fetchTimeOut, new System.Action(() => AbortSetPuzzleScene(delta)));
        c.Coroutine = StartCoroutine(GetPuzzle(id, delta, (b) => c.Running = b));
        queries.Add(c);
    }

    private void AbortSetPuzzleScene(int delta)
    {
        GameManager.SetBackUpPuzzle(delta);
        GameManager.LoadScene("Puzzle");
    }
    
    private IEnumerator GetPuzzle(int id, int delta, System.Action<bool> setRunning)
    {
        setRunning(true);
        string puzzle = null;
        yield return StartCoroutine(querier.GetPuzzle("GetPuzzle",id, delta, (s) => puzzle = s));
        if (puzzle != null)
        {
            string[] data = puzzle.Split(';');
            if (data.Length != 4 || !int.TryParse(data[0], out int pID) || !int.TryParse(data[2], out int rank))
            {
                //Debug.Log("Format Error");
                Debug.Log(puzzle);
                GameManager.SetBackUpPuzzle(delta);
            }
            else
            {
                PuzzleData p = new PuzzleData(pID, data[1], rank, data[3]);
                GameManager.Puzzle = p;
            }
        }
        else
        {
            GameManager.SetBackUpPuzzle(delta);
        }
        setRunning(false);
        GameManager.LoadScene("Puzzle");
    }


    ///ADDPlayer///
    public void RegisterUser(PlayerData playerData)
    {
        QuerieCoroutine c = new QuerieCoroutine(sendTimeOut, new System.Action(() => AbortRegisterUser()));

        c.Coroutine = StartCoroutine(AddPlayer(playerData, (i) => playerData.ID = i, (b) => c.Running = b));
        queries.Add(c);
    }

    private void AbortRegisterUser()
    {
        Debug.Log("Couldn't Add Player to the DataBase");
    }

    private IEnumerator AddPlayer(PlayerData playerData, System.Action<int> callback, System.Action<bool> setRunning)
    {
        setRunning(true);
        int id = -1;
        bool succes = false;
        yield return StartCoroutine(querier.RegisterPlayer("RegisterPlayer",playerData,(s) => succes = int.TryParse(s, out id)));
        if (succes)
            callback(id);
        setRunning(false);
    }

}

public class QuerieCoroutine
{
    public bool Running { get; set; }
    public Coroutine Coroutine { get; set; }
    public double TimeOut { get; set; }
    public System.Action AbortFunction { get; set; }

    public QuerieCoroutine(double timeOut, System.Action abortFunction)
    {
        TimeOut = timeOut;
        AbortFunction = abortFunction;
    }

    public void Abort()
    {
        Running = false;
        Debug.Log("Coroutine Aborted: " + Coroutine.ToString());
        AbortFunction.Invoke();
    }
}
