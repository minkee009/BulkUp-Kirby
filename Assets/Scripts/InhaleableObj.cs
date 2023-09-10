using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InhaleableObj : MonoBehaviour
{
    public Rigidbody2D rb;
    public Collider2D col;

    public SpecialAbility ability = SpecialAbility.None;
    public bool isItem;
}
