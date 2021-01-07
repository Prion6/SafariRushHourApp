using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Puzzle : MonoBehaviour
{
    private char[,] matrix;
    private string label;
    
    public List<PiecePref> piecePrefs;
    private Dictionary<PieceType,GameObject> pieces;

    public GameObject slabPref;

    public int step;

    // Start is called before the first frame update
    void Awake()
    {
        pieces = new Dictionary<PieceType, GameObject>();
        foreach(PiecePref pp in piecePrefs)
        {
            pieces.Add(pp.type, pp.go);
        }
    }

    public void Init(string s)
    {
        InitMatrix(s);
        InitBoard();
        InitPieces();
    }

    public void InitMatrix(string s)
    {
        string[] lines = s.Split('\n');
        label = lines[0].Trim('P');
        matrix = new char[(lines.Length) + 1, (lines.Length) + 1]; // -1 debido al header, +2 para agregar murallas y puertas
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(0); j++)
            {
                if(i == 0 || i >= matrix.GetLength(0) - 1 || j == 0 || (j >= matrix.GetLength(0) - 1 && j >= lines[i].Length))
                {
                    matrix[i, j] = '0';
                    continue;
                }
                matrix[i, j] = lines[i][j-1];
            }
        }
        //PrintMatrix();
    }

    public void InitBoard()
    {
        Vector3 start = new Vector3(-matrix.GetLength(0) * step / 2, 0, -matrix.GetLength(0) * step / 2);
        GameObject board = Instantiate(new GameObject());
        board.name = "Board";

        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(0); j++)
            {
                GameObject slab = Instantiate(slabPref);
                slab.transform.localScale = new Vector3(step, 1, step);
                slab.transform.position = start + new Vector3(step * i, 0, step * j);
                slab.name = "Slab" + (i * matrix.GetLength(0) + j);
                slab.transform.SetParent(board.transform);
            }
        }
    }

    public void InitPieces()
    {
        Vector3 start = new Vector3(-matrix.GetLength(0) * step / 2, 0, -matrix.GetLength(0) * step / 2);
        GameObject walls = Instantiate(new GameObject());
        walls.name = "Walls";

        List<char> visited = new List<char>();

        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(0); j++)
            {
                if (matrix[i, j].Equals('.')) continue;
                if (visited.Contains(matrix[i, j])) continue;
                if (matrix[i, j].Equals('0'))
                {
                    
                    GameObject w = Instantiate(pieces[PieceType.WALL]);
                    w.transform.position = start + new Vector3(step * i, 0, step * j);
                    w.name = "Wall-Piece: " + (i * matrix.GetLength(0) + j);
                    w.transform.SetParent(walls.transform);
                    continue;
                }

                visited.Add(matrix[i, j]);
                GameObject g = pieces[GetPieceType(matrix[i, j])];
                
                if (g == null) continue;

                Piece p = g.GetComponent<Piece>();
                if (p == null) continue;

                g = Instantiate(g);
                p = g.GetComponent<Piece>();
                p.direction = Direction.HORIZONTAL;
                if (matrix[i, j + 1].Equals(matrix[i, j]))
                {
                    g.transform.Rotate(new Vector3(0, -90, 0));
                    if(matrix[i + 1, j].Equals(matrix[i, j]))
                        p.direction = Direction.BOTH;
                    p.direction = Direction.VERTICAL;
                }
                g.transform.position = start + new Vector3(step * i, 0, step * j);
                p.identifier = matrix[i, j];
            }
        }
    }

    public Vector3 RequestMovement(char identifier, Vector2 v)
    {
        if (GetPieceType(identifier).Equals(PieceType.WALL) || GetPieceType(identifier).Equals(PieceType.WALL)) return Vector3.zero;

        Vector2 pos = GetPieceCorrdinates(identifier);

        Vector2 move = GetValidMove(identifier, v, pos);

        MovePiece(identifier, move);

        return new Vector3(move.x, 0, move.y);
        
    }

    public Vector2 GetValidMove(char identifier, Vector2 v, Vector2 pos)
    {
        int move = 0;
        float x = Mathf.Abs(v.x);
        float y = Mathf.Abs(v.y);
        int direction = 1;
        int offset = 0;

        if (x > y)
        {
            if (v.x < 0)
            {
                direction = -1;
            }
            for (int i = 0; i < x; i++)
            {
                if (pos.x + i * direction > matrix.GetLength(0) || pos.x + i * direction < 0) break;
                if (matrix[(int)pos.x + i * direction, (int)pos.y].Equals(identifier))
                {
                    x++;
                    offset++;
                    move++;
                    continue;
                }
                if (!matrix[(int)pos.x + i, (int)pos.y].Equals('.')) break;
                move++;
            }
            move -= offset;
            move *= direction;
            return new Vector2(move, 0);
        }
        else
        {
            if (v.y < 0)
            {
                direction = -1;
            }
            for (int i = 0; i < y; i++)
            {
                if (pos.y + i * direction > matrix.GetLength(0) || pos.y + i * direction < 0) break;
                if (matrix[(int)pos.x, (int)pos.y + i * direction].Equals(identifier))
                {
                    y++;
                    offset++;
                    move++;
                    continue;
                }
                if (!matrix[(int)pos.x + i, (int)pos.y].Equals('.')) break;
                move++;
            }
            move -= offset;
            move *= direction;
            return new Vector2(0, move);
        }
    }

    public void MovePiece(char identifier, Vector2 move)
    {
        List<Vector2> positions = new List<Vector2>();
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(0); j++)
            {
                if (matrix[i, j].Equals(identifier))
                {
                    positions.Add(new Vector2(i, j));
                }
            }
        }
        foreach(Vector2 v in positions)
        {
            matrix[(int)v.x, (int)v.y] = '.';
            matrix[(int)(v.x + move.x), (int)(v.y + move.y)] = identifier;
        }
    }

    public Vector2 GetPieceCorrdinates(char identifier)
    {
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(0); j++)
            {
                if (matrix[i, j].Equals(identifier))
                    return new Vector2(i, j);
            }
        }
        return new Vector2(-1, -1);
    }

    private PieceType GetPieceType(char c)
    {
        PieceType piece = PieceType.EMPTY;
        switch(c)
        {
            case 'x': piece = PieceType.ROVER;
                break;
            case 'u'://two_by_two
            case 'v': piece = PieceType.TWO_BY_TWO;
                break;
            case 'o'://three_by_one
            case 'q':
            case 's':
            case 'p':
            case 'r': piece = PieceType.THREE_BY_ONE;
                break;
            case 'a'://two_by_one
            case 'f':
            case 'd':
            case 'g':
            case 'j':
            case 'b':
            case 'h':
            case 'i':
            case 'k':
            case 'c':
            case 'e': piece = PieceType.TWO_BY_ONE;
                break;
            case '0': piece = PieceType.WALL;
                break;
            case '!': piece = PieceType.EXIT;
                break;
            default: piece = PieceType.EMPTY;
                break;
        }
        return piece;
    }

    public string PrintMatrix()
    {
        string m = "";
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            string s = "";
            for (int j = 0; j < matrix.GetLength(0); j++)
            {
                s += matrix[i, j];
            }
            m += s + "\n";
            Debug.Log(s);
        }
        return m;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[System.Serializable]
public struct PiecePref
{
    public PieceType type;
    public GameObject go;
}