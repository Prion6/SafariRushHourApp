using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleManager : MonoBehaviour
{
    Puzzle Puzzle { get; set; }
    Camera cam;
    Piece selectedPiece;

    Vector3 startPoint;
    Vector3 endPoint;

    public Text matrixtext;

    public GameObject UI;
    public GameObject gameOverPanel;

    private List<MoveData> register;

    private bool running;

    public double StartTime { get; set; }
    public double EndTime { get; set; }

    // Start is called before the first frame update
    void Awake()
    {
        string s = GameManager.Puzzle.Puzzle;
        s = s.Trim(' ');
        if (!s[0].Equals('P'))
        {
            //Obtener del pool interno
            Debug.LogError("Format error, initial value: " + s[0] + " Puzzle: " + s);
            
        }
        Puzzle = FindObjectOfType<Puzzle>();
        Puzzle.Init(s);
        matrixtext.text = Puzzle.PrintMatrix();
    }

    void Start()
    {
        cam = Camera.main;
        running = true;
        register = new List<MoveData>();
        StartTime = Time.time;
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
                AddRegister(selectedPiece.TryMove(new Vector2(endPoint.x - startPoint.x, endPoint.z - startPoint.z)));
                //Debug.Log("Movement: " + register[register.Count-1].identifier + register[register.Count - 1].direction + register[register.Count - 1].magnitude);
                matrixtext.text = Puzzle.PrintMatrix();
            }
        }
    }

    private void AddRegister(MoveData md)
    {
        if (md.magnitude == 0) return;
        register.Add(md);
    }

    private void StoreMovementRegister(out string rawMovements, out string effectiveMovements)
    {
        rawMovements = "";
        effectiveMovements = "";
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
        Debug.Log("Raw Movements: " + rawMovements);
        Debug.Log("Effective Movements: " + effectiveMovements);
    }

    public void GameOver()
    {
        running = false;
        UI.SetActive(false);
        gameOverPanel.SetActive(true);
        EndTime = Time.time;
        StoreMovementRegister(out string raw, out string effective);
        double totalTime = EndTime - StartTime;
        Debug.Log("Total Time: " + totalTime);
    }

    public void GoToMainMenu()
    {
        GameManager.LoadScene("MainMenu");
    }
}
