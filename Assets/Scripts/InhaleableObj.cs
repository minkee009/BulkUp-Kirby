using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InhaleableObj : MonoBehaviour
{
    public SpecialAbility ability = SpecialAbility.None;
    public bool isItem;
    public bool isBoss;
    public Sprite inhaleSprite;

    public GameObject CreateDoll()
    {
        var doll = new GameObject();

        doll.transform.position = transform.position;
        doll.transform.rotation = transform.rotation;
        doll.transform.localScale = transform.localScale;

        doll.name = gameObject.name + " Doll";

        var spriteRender = doll.AddComponent<SpriteRenderer>();

        var g = GetComponent<SpriteRenderer>();
        spriteRender.flipX = g.flipX;

        if (inhaleSprite != null)
        {
            spriteRender.sprite = inhaleSprite;
        }
        else
        {
           
            spriteRender.sprite = g.sprite;
        }
        

        var inhaleableObj = doll.AddComponent<InhaleableObj>();
        inhaleableObj.ability = ability;
        inhaleableObj.isItem = isItem;
        inhaleableObj.isBoss = isBoss;

        return doll;
    }
}
