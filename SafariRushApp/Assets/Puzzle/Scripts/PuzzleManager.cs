using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleManager : MonoBehaviour
{
    Puzzle Puzzle { get; set; }
    public HUDController hudController;
    public WinPanelController winPanel;
    Camera cam;
    Piece selectedPiece;

    Vector3 startPoint;
    Vector3 endPoint;

    public Text matrixtext;

    public GameObject UI;
    public GameObject gameOverPanel;

    private List<MoveData> register;

    private bool running;

    private double StartTime { get; set; }
    private double EndTime { get; set; }
    private double Timer { get; set; }
    private int HintUsed { get; set; }
    private int UndoUsed { get; set; }
    private int RestartUsed { get; set; }

    private int Moves { get; set; }

    // Start is called before the first frame update
    void Awake()
    {
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
        matrixtext.text = Puzzle.PrintMatrix();
    }

    void Start()
    {
        cam = Camera.main;
        running = true;
        register = new List<MoveData>();
        StartTime = Time.time;
        Timer = 0;
        Moves = 0;
        hudController.LoadLanguage();
        winPanel.LoadLanguage();
    }
  
    // Update is called once per frame
    void Update()
    {
        if(running)
        {
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
            if (Input.GetMouseButtonUp(0))
            {
                endPoint = Input.mousePosition + new Vector3(0, 0, cam.transform.position.y);
                endPoint = cam.ScreenToWorldPoint(endPoint);
                if (selectedPiece != null)
                {
                    AddRegister(selectedPiece.TryMove(new Vector2(endPoint.x - startPoint.x, endPoint.z - startPoint.z)));
                    Moves++;
                    hudController.SetMoves(Moves);
                    selectedPiece = null;
                }
                //Debug.Log("Movement: " + register[register.Count-1].identifier + register[register.Count - 1].direction + register[register.Count - 1].magnitude);
                matrixtext.text = Puzzle.PrintMatrix();
            }
            Timer += Time.deltaTime;
            hudController.SetTime(Timer);
        }
    }

    private void AddRegister(MoveData md)
    {
        if (md.magnitude == 0) return;
        register.Add(md);
    }

    public void UndoMovement()
    {
        if (register.Count == 0) return;
        MoveData md = register[register.Count - 1];
        UndoUsed++;
        register.Remove(md);
        foreach(Piece p in Puzzle.gamePieces)
        {
            if(p.Identifier.Equals(md.identifier))
            {
                switch(md.direction)
                {
                    case Direction.r:
                        p.TryMove(new Vector2(0, -md.magnitude));
                        break;
                    case Direction.l:
                        p.TryMove(new Vector2(0, md.magnitude));
                        break;
                    case Direction.u:
                        p.TryMove(new Vector2(md.magnitude,0));
                        break;
                    case Direction.d:
                        p.TryMove(new Vector2(-md.magnitude,0));
                        break;
                }
            }
        }
        Moves--;
        hudController.SetMoves(Moves);
    }

    private void GetMovementRegister(out string rawMovements, out string effectiveMovements)
    {
        rawMovements = "";
        effectiveMovements = "";
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

    public void GameOver()
    {
        running = false;
        UI.SetActive(false);
        gameOverPanel.SetActive(true);
        EndTime = Time.time;
        GetMovementRegister(out string raw, out string effective);
        double totalTime = EndTime - StartTime;
    }

    public void GoToMainMenu()
    {
        GameManager.LoadScene("MainMenu");
    }

    public void RegisterGameData()
    {
        int evaluation = (int)winPanel.GetData().x;
        GetMovementRegister(out string raw, out string effective);
        StatisticData data = new StatisticData(Puzzle.data.ID, GameManager.PlayerID, DateTime.Now, (int)Timer, raw,
                                            evaluation, HintUsed, RestartUsed, UndoUsed);
        GameManager.RegisterGame(data);
    }

    public void Continue()
    {
        int delta = (int)winPanel.GetData().y;
        GameManager.LoadPuzzleScene(Puzzle.data.Ranking + delta);
    }

    private void OnApplicationQuit()
    {
        int evaluation = (int)winPanel.GetData().x;
        GetMovementRegister(out string raw, out string effective);
        StatisticData data = new StatisticData(Puzzle.data.ID, GameManager.PlayerID, DateTime.Now, (int)Timer, raw,
                                            evaluation, HintUsed, RestartUsed, UndoUsed);
        GameManager.StoreGameData(data);
        GameManager.OnQuit();
    }
}
