using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public Vector2 dimension;
    public char identifier;
    public List<Vector2> coordenates;
    public PieceType type;
    public Direction direction;
    public Puzzle Puzzle { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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

        if (direction.Equals(Direction.HORIZONTAL))
        {
            v = new Vector2(x,0);
        }
        else if (direction.Equals(Direction.VERTICAL))
        {
            v = new Vector2(0, z);
            return;
        }
        else if (direction.Equals(Direction.BOTH))
        {

            v = new Vector2(x,z);
        }
        Move(Puzzle.RequestMovement(identifier,v));
    }

    public void Move(Vector3 v)
    {
        transform.Translate(v);
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
