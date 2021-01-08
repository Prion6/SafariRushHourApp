using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Puzzle : MonoBehaviour
{
    private char[,] matrix;
    private string label;
    
    public List<Piece> piecePrefs;
    private Dictionary<PieceType,GameObject> pieces;

    public GameObject slabPref;

    public int step;

    // Start is called before the first frame update
    void Awake()
    {
        pieces = new Dictionary<PieceType, GameObject>();
        foreach(Piece p in piecePrefs)
        {
            pieces.Add(p.type, p.gameObject);
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
                PieceType t = GetPieceType(matrix[i, j]);
                if (t.Equals(PieceType.EMPTY)) continue;
                GameObject g = pieces[t];
                
                if (g == null) continue;

                Piece p = g.GetComponent<Piece>();
                if (p == null) continue;

                g = Instantiate(g);
                p = g.GetComponent<Piece>();

                Direction d = Direction.HORIZONTAL;
                if (matrix[i, j + 1].Equals(matrix[i, j]))
                {
                    if(matrix[i + 1, j].Equals(matrix[i, j]))
                        d = Direction.BOTH;
                    else
                        d = Direction.VERTICAL;
                }
                p.Init(d,i,j,matrix[i,j],this, start + new Vector3(step * i, 0, step * j));
            }
        }
    }
    
    public Vector2 RequestMovement(char identifier, List<Vector2> start, List<List<Vector2>> coords, Vector2 step, List<char> stepables, out List<Vector2> end)
    {
        Vector2 move = Vector2.zero;
        end = start;
        for(int i = 0; i < coords.Count; i++)
        {
            bool check = true;
            foreach(Vector2 c in coords[i])
            {
                bool canStep = false;
                if(c.x >= matrix.GetLength(0) || c.x < 0 || c.y >= matrix.GetLength(0) || c.y < 0)
                {
                    check = false;
                    break;
                }
                foreach(char s in stepables)
                {
                    if((matrix[(int)c.x, (int)c.y].Equals(s)))
                    {
                        canStep = true;
                        break;
                    }    
                }
                if (!canStep) check = false;

            }
            if (!check) break;
            end = coords[i];
            move += step;
        }
        MovePiece(identifier, start, end);
        return move;
    }
    
    public void MovePiece(char identifier, List<Vector2> start, List<Vector2> end)
    {
        foreach(Vector2 v in start)
        {
            matrix[(int)v.x, (int)v.y] = '.';
        }
        foreach (Vector2 v in end)
        {
            matrix[(int)v.x, (int)v.y] = identifier;
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
            case '!': piece = PieceType.EMPTY;
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
                s += matrix[i, j] + "\t";
            }
            m += s + "\n";
            //Debug.Log(s);
        }
        return m;
    }
}