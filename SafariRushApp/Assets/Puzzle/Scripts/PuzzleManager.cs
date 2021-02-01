using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleManager : SceneManager
{
    Puzzle Puzzle { get; set; }
    public HUDController hudController;
    public WinPanelController winPanel;
    public DifficultyPanel difficultyPanel;
    Camera cam;
    Piece selectedPiece;

    Vector3 startPoint;
    Vector3 endPoint;

    public GameObject UI;
    public GameObject gameOverPanel;

    private List<MoveData> register;
    private List<RestartInfo> restarts;
    private Slab selectedSlab;

    private bool running;

    private double StartTime { get; set; }
    private double EndTime { get; set; }
    private double Timer { get; set; }
    private int HintUsed { get; set; }
    private int UndoUsed { get; set; }

    private int Moves { get; set; }
    private bool Win { get; set; }


    // Start is called before the first frame update
    void Awake()
    {
        Timer = 0;
        Moves = 0;
        hudController.SetMoves(0);
        hudController.SetTime(0);
        register = new List<MoveData>();
        restarts = new List<RestartInfo>();
        string s = GameManager.Puzzle.Puzzle.Trim(' ');
        if (!s[0].Equals('P'))
        {
            //Obtener del pool interno
            Debug.LogError("Format error, initial value: " + s[0] + " Puzzle: " + s);

        }
        Puzzle = FindObjectOfType<Puzzle>();
        //hudController = FindObjectOfType<HUDController>();
        //winPanel = FindObjectOfType<WinPanelController>();
        Puzzle.Init(GameManager.Puzzle);
        //matrixtext.text = Puzzle.PrintMatrix();
        GameManager.OnLanguageChange.AddListener(LoadLanguage);
        LoadLanguage();
        difficultyPanel.SetDifficulty(GameManager.Puzzle.Ranking);
        GameManager.OnVolumeChange.Invoke();
    }

    public void Init()
    {
        SetInitialConditions();
        Puzzle = FindObjectOfType<Puzzle>();
        //hudController = FindObjectOfType<HUDController>();
        //winPanel = FindObjectOfType<WinPanelController>();
        Puzzle.Init(GameManager.Puzzle);
    }

    public void SetInitialConditions()
    {
        Timer = 0;
        Moves = 0;
        UndoUsed = 0;
        HintUsed = 0;
        hudController.SetMoves(0);
        hudController.SetTime(0);
        register = new List<MoveData>();
        string s = GameManager.Puzzle.Puzzle.Trim(' ');
        if (!s[0].Equals('P'))
        {
            //Obtener del pool interno
            Debug.LogError("Format error, initial value: " + s[0] + " Puzzle: " + s);

        }
    }

    void Start()
    {
        LoadLanguage();
        cam = Camera.main;
        register = new List<MoveData>();
        Timer = 0;
        Moves = 0;
        StartTime = Time.time;
        running = true;
    }
  
    // Update is called once per frame
    void Update()
    {
        if(running)
        {
            Timer += Time.deltaTime;
            hudController.SetTime(Timer);
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    Piece piece = hit.collider.gameObject.GetComponent<Piece>();
                    if (piece == null) return;
                    if (piece.type.Equals(PieceType.WALL) || piece.type.Equals(PieceType.EMPTY)) return;
                    selectedPiece = piece;
                    startPoint = Input.mousePosition + new Vector3(0, 0, cam.transform.position.y);
                    startPoint = cam.ScreenToWorldPoint(startPoint);
                }
            }
            else if(Input.GetMouseButton(0))
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    Slab slab = hit.collider.gameObject.GetComponent<Slab>();
                    if (slab == null) return;
                    if (slab.Equals(selectedSlab)) return;
                    if (selectedPiece == null) return;
                    selectedSlab = slab;
                    Puzzle.HighLightPath(selectedPiece, slab);
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                Puzzle.TurnPathOff();
                endPoint = Input.mousePosition + new Vector3(0, 0, cam.transform.position.y);
                endPoint = cam.ScreenToWorldPoint(endPoint);
                if (selectedPiece != null)
                {
                    AddRegister(selectedPiece.TryMove(new Vector2(endPoint.x - startPoint.x, endPoint.z - startPoint.z)));
                    hudController.SetMoves(Moves);
                    selectedPiece = null;
                }
                //Debug.Log("Movement: " + register[register.Count-1].identifier + register[register.Count - 1].direction + register[register.Count - 1].magnitude);
                //matrixtext.text = Puzzle.PrintMatrix();
                if (selectedSlab != null)
                {
                    selectedSlab.SetSelected(false);
                    selectedSlab = null;
                }
            }
        }
    }

    public void SetRunning(bool b)
    {
        running = b;
    }

    public void LoadLanguage()
    {
        hudController.LoadLanguage();
        winPanel.LoadLanguage();
    }

    private void AddRegister(MoveData md)
    {
        if (md.magnitude == 0) return;
        register.Add(md);
        Moves++;
    }

    public void UndoMovement()
    {
        if (register.Count == 0) return;
        MoveData md = register[register.Count - 1];
        MoveData undo = new MoveData();
        foreach(Piece p in Puzzle.gamePieces)
        {
            if(p.Identifier.Equals(md.identifier))
            {
                switch(md.direction)
                {
                    case Direction.r:
                        undo = p.TryMove(new Vector2(0, -md.magnitude));
                        break;
                    case Direction.l:
                        undo = p.TryMove(new Vector2(0, md.magnitude));
                        break;
                    case Direction.u:
                        undo = p.TryMove(new Vector2(md.magnitude,0));
                        break;
                    case Direction.d:
                        undo = p.TryMove(new Vector2(-md.magnitude,0));
                        break;
                }
            }
        }
        Debug.Log(md.identifier.ToString() + md.direction.ToString() + md.magnitude + " - " + undo.identifier + undo.direction + undo.magnitude);
        if (undo.magnitude == 0) return;
        UndoUsed++;
        register.RemoveAt(register.Count-1);
        Moves--;
        hudController.SetMoves(Moves);
        Debug.Log("Count: " + register.Count);
    }

    private void GetMovementRegister(out string rawMovements, out string effectiveMovements)
    {
        rawMovements = "";
        effectiveMovements = "";
        if (register == null) return;
        if (register.Count <= 0) return;
        MoveData s = register[0];
        rawMovements += s.identifier + "" + s.direction + "" + s.magnitude.ToString() + " ";
        for(int i = 1; i < register.Count; i++)
        {
            rawMovements += register[i].identifier+ "" + register[i].direction + "" + register[i].magnitude + " ";
            if (s.identifier.Equals(register[i].identifier))
            {
                if (s.direction.Equals(register[i].direction))
                {
                    s.magnitude += register[i].magnitude;
                    continue;
                }
                switch (s.direction)
                {
                    case Direction.d:
                        if (register[i].direction.Equals(Direction.u))
                        {
                            s.magnitude -= register[i].magnitude;
                            if(s.magnitude < 0)
                            {
                                s.magnitude *= -1;
                                s.direction = Direction.u;
                            }
                            continue;
                        }
                        break;
                    case Direction.l:
                        if (register[i].direction.Equals(Direction.r))
                        {
                            s.magnitude -= register[i].magnitude;
                            if (s.magnitude < 0)
                            {
                                s.magnitude *= -1;
                                s.direction = Direction.r;
                            }
                            continue;
                        }
                        break;
                    case Direction.r:
                        if (register[i].direction.Equals(Direction.l))
                        {
                            s.magnitude -= register[i].magnitude;
                            if (s.magnitude < 0)
                            {
                                s.magnitude *= -1;
                                s.direction = Direction.l;
                            }
                            continue;
                        }
                        break;
                    case Direction.u:
                        if (register[i].direction.Equals(Direction.d))
                        {
                            s.magnitude -= register[i].magnitude;
                            if (s.magnitude < 0)
                            {
                                s.magnitude *= -1;
                                s.direction = Direction.d;
                            }
                            continue;
                        }
                        break;
                }
            }
            effectiveMovements += s.identifier + "" + s.direction + "" + s.magnitude + "" + " ";
            s = register[i];
        }
        effectiveMovements += s.identifier + "" + s.direction + "" + s.magnitude + " ";
    }

    public void GameOver(bool won)
    {
        Win = won;
        running = false;
        UI.SetActive(false);
        GetMovementRegister(out string raw, out string effective);
        double proficiency = CalculatePerformance(raw);
        gameOverPanel.SetActive(true);
        winPanel.DisplayPerformance(proficiency);
        EndTime = Time.time;
        double totalTime = EndTime - StartTime;
    }

    public void GoToMainMenu()
    {
        GetMovementRegister(out string raw, out string effective);
        double proficiency = CalculatePerformance(raw);
        RegisterGameData(raw, proficiency);
        LoadScene("MainMenu");
    }

    public double CalculatePerformance(string movements)
    {
        double proficiency = 0;
        if (Win)
        {
            proficiency = 0.5f;
            proficiency += 0.5 * ((float)Puzzle.data.OptimalMoves / ((float)movements.Split(' ').Length-1));
            proficiency *= Mathf.Pow(0.8f, restarts.Count);
            foreach (RestartInfo r in restarts)
            {
                HintUsed += r.hintUsed;
            }
            proficiency *= Mathf.Pow(0.9f, HintUsed);
            // + 
            //hint*0.9
            //restart*0.8
        }
        return proficiency;
    }

    public void RegisterGameData(string movements, double proficiency)
    {
        int evaluation = (int)winPanel.GetPlayerEvaluation();

        StatisticData data = new StatisticData(Puzzle.data.ID, GameManager.PlayerID, DateTime.Now, (int)Timer, movements,
                                            evaluation, HintUsed, restarts.Count, UndoUsed, (float)proficiency);
        GameManager.RegisterGame(data);
    }

    public void Continue()
    {
        GetMovementRegister(out string raw, out string effective);
        double proficiency = CalculatePerformance(raw);
        RegisterGameData(raw, proficiency);
        int delta = (int)winPanel.GetPlayerPreference();
        GameManager.LoadPuzzleScene(Puzzle.data.Ranking + delta*GameManager.SelectionOffset);
    }

    public void Restart()
    {
        //Puzzle.Clear();
        GetMovementRegister(out string raw, out string effective);
        string[] raws = raw.Split(' ');
        string[] effectives = effective.Split(' ');
        RestartInfo ri = new RestartInfo(raws.Length, effectives.Length, (int)Timer, UndoUsed, HintUsed);
        restarts.Add(ri);
        SetInitialConditions();
        Puzzle.Restart();
    }

    public void GetHint()
    {
        string p = Puzzle.PrintMatrix().Replace("0", "");
        string s = string.Join(" ", "P", Puzzle.data.Label, "\n", p, "\n");
        //Debug.Log(s);
        GameManager.GetHint(s);
    }

    private void OnApplicationQuit()
    {
        GetMovementRegister(out string raw, out string effective);
        double proficiency = CalculatePerformance(raw);
        RegisterGameData(raw, proficiency);
        GameManager.OnQuit();
    }
}

public struct RestartInfo
{
    public int rawMoves;
    public int effectiveMoves;
    public int time;
    public int undos;
    public int hintUsed;

    public RestartInfo(int rawMoves, int effectiveMoves, int time, int undos, int hintUsed)
    {
        this.rawMoves = rawMoves;
        this.effectiveMoves = effectiveMoves;
        this.time = time;
        this.undos = undos;
        this.hintUsed = hintUsed;
    }
}
