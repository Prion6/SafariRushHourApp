using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public Vector2 dimension;
    public char Identifier { get; set; }
    [SerializeField]
    private List<Vector2> Coordenates;// { get; set; }
    public List<char> stepables;
    public PieceType type;
    public Orientation Orientation { get; set; }
    public Puzzle Puzzle { get; set; }
    public AudioEmitter audioEmitter;

    public float moveTime;

    // Start is called before the first frame update
    void Awake()
    {
        audioEmitter = GetComponent<AudioEmitter>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Init(Orientation orientation, int x, int y, char identifier, Puzzle puzzle, Vector3 pos)
    {
        transform.position = pos;
        Orientation = orientation;
        Identifier = identifier;
        Coordenates = new List<Vector2>();
        Vector2 start = new Vector2(x, y);
        
        if(orientation.Equals(Orientation.VERTICAL))
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
        if (audioEmitter != null)
            Debug.Log(audioEmitter.volume);
    }

    public MoveData TryMove(Vector2 v)
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

        switch(Orientation)
        {
            case Orientation.HORIZONTAL: v = new Vector2(x, 0);
                break;
            case Orientation.VERTICAL: v = new Vector2(0, z);
                break;
            case Orientation.BOTH: v = (Mathf.Abs(x) > Mathf.Abs(z)) ? new Vector2(x, 0) : new Vector2(0, z);
                break;
        }

        int m = (int)Mathf.Abs(v.x + v.y);
        Vector2 n = v.normalized;

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
        return Move(move);


        //Move(Puzzle.RequestMovement(Identifier, v));
    }

    public MoveData Move(Vector2 v)
    {
        
        int m = (int)(v.x + v.y);
        if (m == 0) return new MoveData(0, Direction.NONE, Identifier);
        StopAllCoroutines();
        StartCoroutine(Translate(v));

        Direction d;
        if (Mathf.Abs(v.x) > Mathf.Abs(v.y))
        {
            d = (v.x > 0) ? Direction.d : Direction.u;
        }
        else
        {
            d = (v.y > 0) ? Direction.r: Direction.l;
        }

        int n = Mathf.Abs(m);

        MoveData md = new MoveData(n,d,Identifier);
        //Debug.Log("Movement: " + md.identifier + md.direction + md.magnitude);
        return md;
    }

    public virtual IEnumerator Translate(Vector2 move)
    {
        Vector3 v = transform.position + new Vector3(move.x, 0, move.y);
        audioEmitter.MakeSound(audioEmitter.audios[Random.Range(0,audioEmitter.audios.Count)].name);
        float elapsedTime = 0;
        while (elapsedTime < moveTime/GameManager.Speed)
        {
            transform.position = Vector3.Lerp(transform.position, v, (elapsedTime / (moveTime/GameManager.Speed)));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        //audioEmitter.StopBackground();
        transform.position = v;
    }
}

public struct MoveData
{
    public MoveData(int m, Direction d, char i)
    {
        magnitude = m;
        direction = d;
        identifier = i;
    }

    public int magnitude;
    public Direction direction;
    public char identifier;
}

public enum Direction
{
    u,
    d,
    l,
    r,
    NONE
}

public enum Orientation
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
