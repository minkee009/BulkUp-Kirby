using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyFX : MonoBehaviour
{
    public float destroyWaitTime = 0;
    private void Start()
    {
        Destroy(gameObject,destroyWaitTime);
    }
}
