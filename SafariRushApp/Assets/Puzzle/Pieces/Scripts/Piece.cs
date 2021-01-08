using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public Vector2 dimension;
    private char Identifier { get; set; }
    [SerializeField]
    private List<Vector2> Coordenates;// { get; set; }
    public List<char> stepables;
    public PieceType type;
    public Direction Direction { get; set; }
    public Puzzle Puzzle { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(Direction direction, int x, int y, char identifier, Puzzle puzzle, Vector3 pos)
    {
        transform.position = pos;
        Direction = direction;
        Identifier = identifier;
        Coordenates = new List<Vector2>();
        Vector2 start = new Vector2(x, y);
        
        if(direction.Equals(Direction.VERTICAL))
        {
            transform.Rotate(new Vector3(0, -90, 0));
            float d = dimension.x;
            dimension.x = dimension.y;
            dimension.y = d;
        }

        for (int i = 0; i < dimension.x; i++)
        {
            for (int j = 0; j < dimension.y; j++)
            {
                Coordenates.Add(start + new Vector2(i, j));
            }
        }
        stepables.Add(identifier);
        Puzzle = puzzle;
    }

    public void TryMove(Vector2 v)
    {
        int x = (int)v.x;
        if (Mathf.Abs(v.x - x) > 0.6)
        {
            if (v.x > 0) x++;
            else x--;
        }

        int z = (int)v.y;
        if (Mathf.Abs(v.y - z) > 0.6)
        {
            if (v.y > 0) z++;
            else z--;
        }

        switch(Direction)
        {
            case Direction.HORIZONTAL: v = new Vector2(x, 0);
                break;
            case Direction.VERTICAL: v = new Vector2(0, z);
                break;
            case Direction.BOTH: v = (Mathf.Abs(x) > Mathf.Abs(z)) ? new Vector2(x, 0) : new Vector2(0, z);
                break;
        }

        int m = (int)Mathf.Abs(v.x + v.y);
        Vector2 n = v.normalized;
        Debug.Log(n);

        List<List<Vector2>> newCoords = new List<List<Vector2>>(); 

        for(int i = 0; i < m; i++)
        {
            newCoords.Add(new List<Vector2>());
            foreach(Vector2 c in Coordenates)
            {
                newCoords[i].Add(c + n*(i+1));
            }
        }
        List<Vector2> end = new List<Vector2>();
        Vector2 move = Puzzle.RequestMovement(Identifier, Coordenates, newCoords, n, stepables, out end);
        Coordenates = end;
        Move(move);


        //Move(Puzzle.RequestMovement(Identifier, v));
    }

    public void Move(Vector2 v)
    {
        if (!Direction.Equals(Direction.BOTH))
        {
            int m = (int)(v.x+v.y);
            transform.Translate(m, 0, 0);
        }
        else
        {
            transform.Translate(v.x,0,v.y);
        }

    }
}

public enum Direction
{
    HORIZONTAL,
    VERTICAL,
    BOTH
}

public enum PieceType
{
    ROVER,
    TWO_BY_TWO,
    TWO_BY_ONE,
    THREE_BY_ONE,
    WALL,
    EXIT,
    EMPTY
}
