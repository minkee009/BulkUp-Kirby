using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootDustMaker : MonoBehaviour
{
    public float setTime;
    public GameObject dustPrefab;
    public int dustMaxCount = 4;
    public Vector3 dustDir;
    public float dustSpeed;

    GameObject[] _dusts;
    ProjectileMovement _currentMove;

    int _currentIndex;
    float _timer;
    
    private void Start()
    {
        _dusts = new GameObject[dustMaxCount];

        for(int i = 0; i < _dusts.Length; i++)
        {
            _dusts[i] = Instantiate(dustPrefab,transform);
            _dusts[i].transform.localPosition = Vector3.zero;
            _dusts[i].SetActive(false);
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < _dusts.Length; i++)
        {
            _dusts[i].SetActive(false);
        }
        _timer = 0f;
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        if(_timer > setTime)
        {
            _dusts[_currentIndex].TryGetComponent(out _currentMove);
            _dusts[_currentIndex].transform.localPosition = Vector3.zero;
            _dusts[_currentIndex].SetActive(true);
            _currentMove.dir = dustDir.normalized;
            _currentMove.speed = dustSpeed;
            _currentIndex++;
            _currentIndex %= _dusts.Length;
            _timer = 0f;
        }
    }

    public void ImmediateActivate()
    {
        _timer = setTime;
    }
}
