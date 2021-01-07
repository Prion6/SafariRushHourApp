using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    Puzzle p;
    Camera cam;
    Piece selectedPiece;

    Vector2 startPoint;
    Vector2 endPoint;


    // Start is called before the first frame update
    void Awake()
    {
        string s = GameManager.Puzzle;
        s = s.Trim(' ');
        if (!s[0].Equals('P'))
        {
            //Obtener del pool interno
            Debug.LogError("Format error, initial value: " + s[0] + " Puzzle: " + s);
            
        }
        p = FindObjectOfType<Puzzle>();
        p.Init(s);
    }

    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit))
            {
                Piece piece = hit.collider.gameObject.GetComponent<Piece>();
                if (piece == null) return;
                if (piece.type.Equals(PieceType.WALL) || piece.type.Equals(PieceType.EMPTY)) return;
                selectedPiece = piece;
                Debug.Log(piece);
            }
        }
    }
}
