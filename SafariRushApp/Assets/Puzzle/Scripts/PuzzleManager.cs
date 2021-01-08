using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleManager : MonoBehaviour
{
    Puzzle Puzzle { get; set; }
    Camera cam;
    Piece selectedPiece;

    Vector3 startPoint;
    Vector3 endPoint;

    public Text matrixtext;

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
        Puzzle = FindObjectOfType<Puzzle>();
        Puzzle.Init(s);
        matrixtext.text = Puzzle.PrintMatrix();
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
                startPoint = Input.mousePosition + new Vector3(0,0,cam.transform.position.y);
                startPoint = cam.ScreenToWorldPoint(startPoint);
            }
        }
        if(Input.GetMouseButtonUp(0))
        {
            endPoint = Input.mousePosition + new Vector3(0, 0, cam.transform.position.y);
            endPoint = cam.ScreenToWorldPoint(endPoint);
            selectedPiece.TryMove(new Vector2(endPoint.x - startPoint.x, endPoint.z - startPoint.z));
            matrixtext.text = Puzzle.PrintMatrix();
        }
    }
}
