using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

[System.Serializable]
public class Puzzle : MonoBehaviour
{
    private char[,] matrix;

    public PuzzleData data;
    
    public List<Piece> piecePrefs;
    private Dictionary<PieceType,List<Piece>> pieces;
    public List<Piece> gamePieces;
    public List<GameObject> Board;
    public List<GameObject> Walls;

    public List<GameObject> slabPrefs;

    public int step;

    public List<Slab> highlighted;
    public List<Slab> hinted;

    // Start is called before the first frame update
    void Awake()
    {
        Board = new List<GameObject>();
        Walls = new List<GameObject>();
        gamePieces = new List<Piece>();
        highlighted = new List<Slab>();
        pieces = new Dictionary<PieceType, List<Piece>>();
        foreach(Piece p in piecePrefs)
        {
            if (!pieces.ContainsKey(p.type))
                pieces.Add(p.type, new List<Piece>());
            pieces[p.type].Add(p);
        }
    }

    public void Init(PuzzleData p)
    {
        data = p;
        InitMatrix(p.Puzzle);
        InitBoard();
        InitPieces();
    }

    public void InitMatrix(string s)
    {
        string[] lines = s.Split('\n');
        data.Label = lines[0].Trim('P',' ','\n','\t');
        //Difficulty = GetDifficulty(label[0]);
        matrix = new char[(lines.Length) + 1, (lines.Length) + 1]; // -1 debido al header, +2 para agregar murallas y puertas
        

        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            if(i < lines.Length)
                lines[i] = Regex.Replace(lines[i], @"[^\w\!.@-]", "");
            for (int j = 0; j < matrix.GetLength(0); j++)
            {
                if(i == 0 || i >= matrix.GetLength(0) - 1 || j == 0 || (j >= matrix.GetLength(0) - 1 && j > lines[i].Length))
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
        GameObject board = Instantiate(new GameObject("Board"));
        GameObject walls = Instantiate(new GameObject("Walls"));

        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(0); j++)
            {
                GameObject slab = Instantiate(slabPrefs[Random.Range(0, slabPrefs.Count)]);

                slab.transform.localScale = new Vector3(step, 1, step);
                slab.transform.position = start + new Vector3(step * i, 0, step * j);
                slab.name = "Slab" + (i * matrix.GetLength(0) + j);
                slab.transform.SetParent(board.transform);
                Board.Add(slab);

                if (matrix[i, j].Equals('0'))
                {
                    GameObject w = Instantiate(pieces[PieceType.WALL][Random.Range(0, pieces[PieceType.WALL].Count)].gameObject);
                    w.transform.position = start + new Vector3(step * i, 0, step * j);
                    //w.transform.Rotate(new Vector3(Random.Range(0,4)*90, Random.Range(0, 4) * 90, Random.Range(0, 4) * 90));
                    w.name = "Wall-Piece: " + (i * matrix.GetLength(0) + j);
                    w.transform.SetParent(walls.transform);
                    Walls.Add(w);
                    continue;
                }
            }
        }
    }

    public void InitPieces()
    {
        Vector3 start = new Vector3(-matrix.GetLength(0) * step / 2, 0, -matrix.GetLength(0) * step / 2);

        List<char> visited = new List<char>();

        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(0); j++)
            {
                if (matrix[i, j].Equals('.')) continue;
                if (visited.Contains(matrix[i, j])) continue;
                

                visited.Add(matrix[i, j]);
                PieceType t = GetPieceType(matrix[i, j]);
                if (t.Equals(PieceType.EMPTY)) continue;
                GameObject g = pieces[t][Random.Range(0, pieces[t].Count)].gameObject;
                
                if (g == null) continue;

                Piece p = g.GetComponent<Piece>();
                if (p == null) continue;

                g = Instantiate(g);
                p = g.GetComponent<Piece>();

                Orientation o = Orientation.HORIZONTAL;
                if (j + 1 < matrix.GetLength(0) && matrix[i, j + 1].Equals(matrix[i, j]))
                {
                    if(i + 1 < matrix.GetLength(0) && matrix[i + 1, j].Equals(matrix[i, j]))
                        o = Orientation.BOTH;
                    else
                        o = Orientation.VERTICAL;
                }
                p.Init(o,i,j,matrix[i,j],this, start + new Vector3(step * i, 0, step * j));
                g.transform.SetParent(transform);
                gamePieces.Add(p);
            }
        }
    }

    public void Restart()
    {
        Clear();
        InitMatrix(data.Puzzle);
        InitPieces();
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
        TurnHintOff();
    }

    public Vector2 GetPieceCorrdinates(char identifier)
    {
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(0); j++)
            {
                if (matrix[i, j].Equals(identifier))
                    return new Vector2(j, i);
            }
        }
        return new Vector2(-1, -1);
    }

    public Piece GetPiece(char identifier)
    {
        foreach(Piece p in gamePieces)
        {
            if(p.Identifier.Equals(identifier))
            {
                return p;
            }
        }
        return null;
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
            case '.': piece = PieceType.EMPTY;
                break;
            default: piece = PieceType.WALL;
                break;
        }
        return piece;
    }

    public string PrintMatrix()
    {
        string m = "";
        for (int i = 1; i < matrix.GetLength(0)-1; i++)
        {
            string s = "";
            for (int j = 0; j < matrix.GetLength(0); j++)
            {
                s += matrix[i, j];
            }
            m += s + "\n";
            //Debug.Log(s);
        }
        return m;
    }

    public void Clear()
    {
        /*
        for(int i = 0; i < Board.Count; i++)
        {
            GameObject go = Board[i];
            Destroy(go);
        }
        Board.Clear();*/
        for (int i = 0; i < gamePieces.Count; i++)
        {
            Piece p = gamePieces[i];
            Destroy(p.gameObject);
        }
        gamePieces.Clear();
        /*for (int i = 0; i < Walls.Count; i++)
        {
            GameObject go = Walls[i];
            Destroy(go);
        }
        Walls.Clear();*/
    }

    public void TurnPathOff()
    {
        foreach(Slab s in highlighted)
        {
            s.SetSelected(false);
        }
        highlighted.Clear();
    }

    public Vector2 GetTranslation(Piece piece, Slab slab)
    {
        Vector2 pieceCords = GetPieceCorrdinates(piece.Identifier);
        //Debug.Log("PCoords: " + pieceCords);
        int slabIndex = Board.IndexOf(slab.gameObject);
        Vector2 slabCords = new Vector2(slabIndex % matrix.GetLength(0), slabIndex / matrix.GetLength(0));
        //Debug.Log("SCoords: " + slabCords);
        Vector2 translation = slabCords - pieceCords;
        switch (piece.Orientation)
        {
            case Orientation.HORIZONTAL:
                translation *= new Vector2(0, 1);
                break;
            case Orientation.VERTICAL:
                translation *= new Vector2(1, 0);
                break;
            case Orientation.BOTH:
                translation *= (Mathf.Abs(translation.x) > Mathf.Abs(translation.y)) ? new Vector2(1, 0) : new Vector2(0, 1);
                //faltan condiciones
                break;
        }
        //Debug.Log(piece.Identifier + " Trans: " + translation);
        return translation;
    }

    public void Highlight()
    {
        foreach(Slab s in highlighted)
        {
            s.SetSelected(true);
        }

    }

    public void HighLightPath(Piece piece, Slab slab)
    {
        TurnPathOff();
        Vector2 pieceCords = GetPieceCorrdinates(piece.Identifier);
        Vector2 translation = GetTranslation(piece, slab);
        Vector2 step = translation.normalized;
        highlighted = SetPath(translation, pieceCords, step, piece.Identifier);
        if (piece.Orientation.Equals(Orientation.BOTH))
        {
            if (step.y + step.x > 0)
            {
                pieceCords += new Vector2(step.y, step.x);
            }
            else
            {
                translation += step;
                if (step.x > step.y)
                {
                    pieceCords += new Vector2(0, 1);
                }
                else
                {
                    pieceCords += new Vector2(1, 0);
                }
                highlighted = SetPath(translation, pieceCords, step, piece.Identifier);
                pieceCords -= new Vector2(step.y, step.x);
            }
            highlighted.AddRange(SetPath(translation, pieceCords, step, piece.Identifier));
        }
        Highlight();
    }

    private List<Slab> SetPath(Vector2 translation, Vector2 pieceCords, Vector2 step, char identifier)
    {
        List<Slab> slabs = new List<Slab>();
        int limit = (int)translation.magnitude;
        //Debug.Log(pieceCords + " move : " + limit);
        for (int i = 0; i <= limit; i++)
        {
            int y = (int)(pieceCords + step * i).y;
            int x = (int)(pieceCords + step * i).x;

            //if (!(matrix[x, y].Equals('.') || matrix[x, y].Equals(identifier))) break;

            //Debug.Log("X: " + x + " - Y: " + y);

            Slab s = Board[x + y * matrix.GetLength(0)].GetComponent<Slab>();

            if (s == null)
            {
                Debug.LogError("No Slab");
                continue;
            }
            if (!slabs.Contains(s))
                slabs.Add(s);
        }
        return slabs;
    }

    public void Hint()
    {
        //Debug.Log(hinted.Count);
        foreach (Slab s in hinted)
        {
            s.SetHint(true);
            //Debug.Log(s.gameObject);
        }

    }

    public void TurnHintOff()
    {
        foreach (Slab s in hinted)
        {
            s.SetHint(false);
        }
        highlighted.Clear();
    }

    public void ShowHint(MoveData move)
    {
        Vector2 coord = GetPieceCorrdinates(move.identifier);
        Vector2 step = Vector2.zero;
        Piece p = GetPiece(move.identifier);

        switch(move.direction)
        {
            case Direction.d: step += new Vector2(0, 1);
                move.magnitude += (int)p.dimension.x - 1;
                break;
            case Direction.u: step += new Vector2(0, -1);
                break;
            case Direction.l: step += new Vector2(-1, 0);
                break;
            case Direction.r: step += new Vector2(1, 0);
                move.magnitude += (int)p.dimension.x - 1;
                break;
        }
        Vector2 translation = step*move.magnitude;
        hinted = SetPath(translation,coord,step,move.identifier);

        if (p.Orientation.Equals(Orientation.BOTH))
        {
            if (step.y + step.x > 0)
            {
                coord += new Vector2(step.y, step.x);
            }
            else
            {
                translation += step;
                if (step.x > step.y)
                {
                    coord += new Vector2(0, 1);
                }
                else
                {
                    coord += new Vector2(1, 0);
                }
                hinted = SetPath(translation, coord, step, p.Identifier);
                coord -= new Vector2(step.y, step.x);
            }
            hinted.AddRange(SetPath(translation, coord, step, p.Identifier));
        }
        Hint();
    }
}


[System.Serializable]
public struct PuzzleData
{
    public PuzzleData(int id, string label, int ranking, string puzzle, int optimalMoves)
    {
        ID = id;
        Label = label;
        Ranking = ranking;
        Puzzle = puzzle;
        OptimalMoves = optimalMoves;
        Matches = 0;
    }

    public int ID;
    public string Label;
    public int Ranking;
    [TextArea]
    public string Puzzle;
    public int Matches;
    public int OptimalMoves;
}