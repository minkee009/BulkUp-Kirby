using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    private float _bossHp { get; set; } = 5;

    private Transform kirbyTransform;

    [SerializeField] private GameObject fallDumbbell;
    [SerializeField] private GameObject throwDumbbell;

    private int randomNumber = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        kirbyTransform = GameObject.FindWithTag("Kirby").transform;
    }

    IEnumerator FallDumbbell()
    {
        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(1f);

            GameObject fall = Instantiate(this.fallDumbbell);
            fall.transform.position = kirbyTransform.transform.position + new Vector3(0, 6, 0);
        }
        
    }

    IEnumerator ThrowDumbbel()
    {
        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(1f);
            
            GameObject wave = Instantiate(throwDumbbell);
            wave.transform.position = transform.position;
        }
    }
}
