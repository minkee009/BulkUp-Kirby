using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InhaleableObj : MonoBehaviour
{
    public SpecialAbility ability = SpecialAbility.None;
    public bool isItem;
    public Sprite inhaleSprite;

    public GameObject CreateDoll()
    {
        var doll = new GameObject();

        doll.transform.position = transform.position;
        doll.transform.rotation = transform.rotation;
        doll.transform.localScale = transform.localScale;

        var spriteRender = doll.AddComponent<SpriteRenderer>();
        spriteRender.sprite = inhaleSprite;

        var inhaleableObj = doll.AddComponent<InhaleableObj>();
        inhaleableObj.ability = ability;
        inhaleableObj.isItem = isItem;

        return doll;
    }
}
