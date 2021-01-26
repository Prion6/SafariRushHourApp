using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slab : MonoBehaviour
{
    public Material selected;
    public Material hint;
    public Material normal;
    public Renderer rend;
    // Start is called before the first frame update
    void Start()
    {
        //SetSelected(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSelected(bool b)
    {
        if(b)
        {
            rend.material = selected;
        }
        else
        {
            rend.material = normal;
        }
    }

    public void SetHint(bool b)
    {
        if (b)
        {
            rend.material = hint;
        }
        else
        {
            rend.material = normal;
        }
    }
}
