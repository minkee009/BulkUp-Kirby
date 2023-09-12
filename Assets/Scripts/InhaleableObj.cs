using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InhaleableObj : MonoBehaviour
{
    public Collider2D col;
    public Rigidbody2D rb;
    
    public SpecialAbility ability = SpecialAbility.None;
    public bool isItem;

    public ParabolaFly flyScript;
}
