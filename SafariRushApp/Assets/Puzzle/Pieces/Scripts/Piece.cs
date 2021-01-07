using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public Vector2 dimension;
    public char identifier;
    public List<Vector2> coordenates;
    public PieceType type;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
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
