using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArm : MonoBehaviour {
    
    private SpriteRenderer spriteR;
    public Sprite[] armSprites;

    void Start()
    {
        spriteR = gameObject.GetComponent<SpriteRenderer>();
    }

    public void ChangeArmSprite(int index)
    {
        int spriteIndex = 0;
        
        if (index == 6 || index == 0)
        {
            spriteIndex = 0;
        }
        if (index == 1 || index == 2 || index == 3 || index == 4 || index == 5 )
        {
            spriteIndex = 1;
        }
        if (index == 8)
        {
            spriteIndex = 2;
        }

        if (index == 7)
        {
            spriteIndex = 7;
        }

        if (index == 9)
        {
            spriteIndex = 3;
        }
        if (index == 10)
        {
            spriteIndex = 4;
        }
        if (index == 11)
        {
            spriteIndex = 5;
        }
        if (index == 12)
        {
            spriteIndex = 6;
        }
        ChangeArmSpriteWithIndex(spriteIndex);
    }

    public void ChangeArmSpriteWithIndex(int index)
    {
        spriteR.sprite = armSprites[index];
    }



}
