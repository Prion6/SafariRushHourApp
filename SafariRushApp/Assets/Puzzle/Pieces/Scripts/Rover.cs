using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rover : Piece
{
    public Animator anim;
    public GameObject model;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override IEnumerator Translate(Vector2 move)
    {
        int dir = 0;
        if(move.x != 0)
        {
            dir = move.x > 0 ? 1 : 3;
        }
        else
        {
            dir = move.y > 0 ?  0 : 2;
        }

        model.transform.rotation = Quaternion.Euler(0,90*dir,0);
        anim.SetBool("Moving",true);
        yield return base.Translate(move);
        anim.SetBool("Moving", false);
    }
}
