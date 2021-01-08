using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : Piece
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Piece p = other.gameObject.GetComponent<Piece>();
        if (p == null) return;
        if (p.type.Equals(PieceType.ROVER))
        {
            FindObjectOfType<PuzzleManager>().GameOver();
        }
    }
}
