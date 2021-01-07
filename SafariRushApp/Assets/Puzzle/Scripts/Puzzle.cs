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
                GameObject g = GetPiece(matrix[i, j]);
                if (g == null) continue;

                Piece p = g.GetComponent<Piece>();
                if (p == null) continue;

                g = Instantiate(g);

                if (matrix[i, j + 1].Equals(matrix[i, j]))
                {
                    g.transform.Rotate(new Vector3(0, -90, 0));
                }
                g.transform.position = start + new Vector3(step * i, 0, step * j);
                g.GetComponent<Piece>().identifier = matrix[i, j];
            }
        }
    }

    private GameObject GetPiece(char c)
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
            default: piece = PieceType.EMPTY;
                break;
        }
        if (piece.Equals(PieceType.EMPTY)) return null;
        return pieces[piece];
    }

    private void PrintMatrix()
    {
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            string s = "";
            for (int j = 0; j < matrix.GetLength(0); j++)
            {
                s += matrix[i, j];
            }
            Debug.Log(s);
        }
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