using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PHPManager : MonoBehaviour
{
    PHPQuerySet querier;
    [SerializeField]
    private List<QuerieCoroutine> queries;
    
    public  double fetchTimeOut;
    public double sendTimeOut;

    private bool conected;
    private bool conectedLastFrame;

    // Start is called before the first frame update
    void Awake()
    {
        conected = true;
        conectedLastFrame = true;
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

        if(!conectedLastFrame && conected)
        {
            GameManager.UpdateData();
        }
    }

    ///GetPuzzle///
    public void GetPuzzle(int id, int delta)
    {
        QuerieCoroutine c = new QuerieCoroutine(fetchTimeOut, new System.Action(() => AbortSetPuzzleScene(delta)));
        c.Coroutine = StartCoroutine(GetPuzzleQuerie(id, delta, (b) => c.Running = b));
        queries.Add(c);
    }

    private void AbortSetPuzzleScene(int delta)
    {
        GameManager.SetBackUpPuzzle(delta);
        GameManager.LoadScene("Puzzle");
    }
    
    private IEnumerator GetPuzzleQuerie(int id, int delta, System.Action<bool> setRunning)
    {
        setRunning(true);
        string puzzle = null;
        yield return StartCoroutine(querier.GetPuzzle("GetPuzzle", delta, id, (s) => puzzle = s));
        if (puzzle != null)
        {
            string[] data = puzzle.Split(';');
            if (data.Length != 5 || !int.TryParse(data[0], out int pID) || !int.TryParse(data[2], out int rank) || !int.TryParse(data[4], out int optimal))
            {
                Debug.Log("Format Error");
                GameManager.SetBackUpPuzzle(delta);
            }
            else
            {
                PuzzleData p = new PuzzleData(pID, data[1], rank, data[3], optimal);
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

        c.Coroutine = StartCoroutine(RegisterUserQuerie(playerData, (i) => GameManager.PlayerID = i, (b) => c.Running = b));
        queries.Add(c);
    }

    private void AbortRegisterUser()
    {
        Debug.Log("Couldn't Add Player to the DataBase");
    }

    private IEnumerator RegisterUserQuerie(PlayerData playerData, System.Action<int> callback, System.Action<bool> setRunning)
    {
        setRunning(true);
        int id = -1;
        bool succes = false;
        yield return StartCoroutine(querier.RegisterPlayer("RegisterPlayer",playerData,(s) => succes = int.TryParse(s, out id)));
        callback(id);
        setRunning(false);
    }


    ///ADDGameData///
    public void RegisterGame(StatisticData data)
    {
        QuerieCoroutine c = new QuerieCoroutine(sendTimeOut, new System.Action(() => AbortRegisterGame()));

        c.Coroutine = StartCoroutine(RegisterGameQuerie(data, (b) => c.Running = b));
        queries.Add(c);
    }

    private void AbortRegisterGame()
    {
        Debug.Log("Couldn't Add Game Data");
    }

    private IEnumerator RegisterGameQuerie(StatisticData data, System.Action<bool> setRunning)
    {
        setRunning(true);
        bool succes = false;
        int playerR = 0;
        int puzzleR = 0;
        yield return StartCoroutine(querier.RegisterGame("RegisterGame", data, (b) => succes = b,
            (s) =>
            {
                Debug.Log(s);
                string[] lines = (s as string).Split(';');
                Debug.Log(lines[0] + " - " + lines[1]);
                playerR = int.Parse(lines[0]);
                puzzleR = int.Parse(lines[1]);
            }));
        if(succes)
        {
            GameManager.FreeGameData(data);
            GameManager.PlayerRanking = playerR;
            GameManager.SetPuzzleRanking(puzzleR, data.PuzzleID);
        }
        else
        {
            GameManager.StoreGameData(data);
        }
        setRunning(false);
    }

    ///GetHint///
    public void GetHint(string puzzle, System.Action<bool> isPaused, System.Action<string> getHint)
    {
        QuerieCoroutine c = new QuerieCoroutine(60, new System.Action(() => AbortGetHint(isPaused)));

        c.Coroutine = StartCoroutine(GetHintQuerie(puzzle, (b) => c.Running = b, isPaused, getHint));
        queries.Add(c);
    }

    private void AbortGetHint(System.Action<bool> isPaused)
    {
        isPaused(false);
        Debug.Log("Couldn't get hint");
    }

    private IEnumerator GetHintQuerie(string puzzle, System.Action<bool> setRunning, System.Action<bool> isPaused, System.Action<string> getHint)
    {
        setRunning(true);
        isPaused(true);
        yield return StartCoroutine(querier.GetHint("GetHint", puzzle, getHint));
        isPaused(false);
        setRunning(false);
    }

    ///GetPlayerInfo///
    public void GetPlayerInfo(System.Action<string> fetchPlayerInfo)
    {
        QuerieCoroutine c = new QuerieCoroutine(fetchTimeOut, new System.Action(() => AbortGetPlayerInfo()));

        c.Coroutine = StartCoroutine(GetPlayerInfoQuerie((b) => c.Running = b, fetchPlayerInfo));
        queries.Add(c);
    }

    public void AbortGetPlayerInfo()
    {
        Debug.Log("Couldn't get player");
    }

    public IEnumerator GetPlayerInfoQuerie(System.Action<bool> setRunning, System.Action<string> fetchPlayerInfo)
    {
        setRunning(true);
        yield return StartCoroutine(querier.GetPlayerInfo("GetPlayerInfo", fetchPlayerInfo));
        setRunning(false);
    }

    ///GetPlayerInfo///
    public void GetLeaderBoard(System.Action<string> fetchLeaderBoard)
    {
        QuerieCoroutine c = new QuerieCoroutine(fetchTimeOut, new System.Action(() => AbortGetLeaderBoard()));

        c.Coroutine = StartCoroutine(GetLeaderBoardQuerie((b) => c.Running = b, fetchLeaderBoard));
        queries.Add(c);
    }

    public void AbortGetLeaderBoard()
    {
        Debug.Log("Couldn't get LeaderBoard");
    }

    public IEnumerator GetLeaderBoardQuerie(System.Action<bool> setRunning, System.Action<string> fetchLeaderBoard)
    {
        setRunning(true);
        yield return StartCoroutine(querier.GetLeaderBoard("GetLeaderBoard", fetchLeaderBoard));
        setRunning(false);
    }
}

[System.Serializable]
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
        //Debug.Log("Coroutine Aborted: " + Coroutine.ToString());
        AbortFunction.Invoke();
    }
}
