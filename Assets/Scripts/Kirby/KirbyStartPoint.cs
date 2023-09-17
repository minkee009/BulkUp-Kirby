using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class KirbyStartPoint : MonoBehaviour
{
    public GameObject kirbyPrefab;

    // Update is called once per frame
    void Update()
    {
        if (Gamemanager.instance.kirbyController == null)
        {
            var kirby = Instantiate(kirbyPrefab);
            kirby.transform.position = transform.position;
            Gamemanager.instance.kirbyController = kirby.GetComponent<KirbyController>();
            DontDestroyOnLoad(kirby);
            Destroy(gameObject);
        }
        else
        {
            Gamemanager.instance.kirbyController.transform.position = transform.position;
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireCube(transform.position, new Vector3(0.75f, 1f, 0f));
    }
}
