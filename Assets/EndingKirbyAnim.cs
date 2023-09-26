using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingKirbyAnim : MonoBehaviour
{
    public Animator kirbyAnim;
    private void Start()
    {
        kirbyAnim.Play("Char_Kirby_Inhaled_Flying");
    }
}
