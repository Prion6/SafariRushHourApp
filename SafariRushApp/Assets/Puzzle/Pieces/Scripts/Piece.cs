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

    public void Move(Vector2 v)
    {
        if(direction.Equals(Direction.HORIZONTAL))
        {
            int x = (int)v.x;
            if (Mathf.Abs(v.x - x) > 0.6)
            {
                if (v.x > 0) x++;
                else x--;
            }
            Debug.Log(x);
            v = new Vector2(x,0);
        }
        else if (direction.Equals(Direction.VERTICAL))
        {
            Debug.Log(direction + " - X: " + v.x + " - Z: " + v.y);
            return;
        }
        else if (direction.Equals(Direction.BOTH))
        {
            Debug.Log(direction + " - X: " + v.x + " - Z: " + v.y);
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
    EMPTY
}
