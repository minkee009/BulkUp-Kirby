using UnityEngine;

public class MonsterSpawn : MonoBehaviour
{
    public Transform spawnPoint; // 스폰 포인트의 Transform
    public GameObject monsterPrefab; // 생성할 몬스터 프리팹
    private bool hasSpawned = false; // 몬스터가 이미 생성되었는지 여부를 확인하기 위한 변수

    private GameObject _myMonster;

    private void Update()
    {
        if (!hasSpawned && _myMonster == null)
        {
            // 스폰 포인트의 월드 좌표를 뷰포트 좌표로 변환
            Vector3 viewportPos = Camera.main.WorldToViewportPoint(spawnPoint.position);

            // 뷰포트 좌표가 [0, 1] 범위 내에 있을 때 몬스터 생성
            if (viewportPos.x >= 0 && viewportPos.x <= 1 && viewportPos.y >= 0 && viewportPos.y <= 1)
            {
                // 몬스터 생성
                _myMonster = Instantiate(monsterPrefab, spawnPoint.position, Quaternion.identity);
            
                // 몬스터가 이미 생성되었다고 표시
                hasSpawned = true;
            }
        }
        else if (!_myMonster.activeSelf)
        {
            hasSpawned = false;
        }
    }
}
