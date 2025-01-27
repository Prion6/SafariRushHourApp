using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyPanel : MonoBehaviour
{
    public List<Color> difficultyLevels;
    public List<Image> skulls;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDifficulty(int ranking)
    {
        for(int c = 0; c < difficultyLevels.Count; c++)
        {
            for(int i = 0; i < skulls.Count; i++)
            {
                skulls[i].enabled = true;
                skulls[i].color = difficultyLevels[c];
                ranking -= GameManager.SelectionOffset;
                if (ranking <= 0)
                {
                    return;
                }
            }
        }
    }
}
