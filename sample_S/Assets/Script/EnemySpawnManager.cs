using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    //소환 상태
    public enum SpawnState
    {
        None, Spawn
    }
    public SpawnState spawnState = SpawnState.None;

    //소환 포인트
    public Transform spawnPoint;

    //적 프리팹
    public GameObject enemyPrefab;

    //소환 시간
    private float spawnTime;

    //설정 소환 시간
    public float setSpawnTime;

    //적의 목표(플레이어)
    [SerializeField]
    private GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("SpawnStart", 2.0f);
    }

    // Update is called once per frame
    void Update()
    {
        switch (spawnState)
        {
            case SpawnState.Spawn:
                {
                    //소환 시간 증가
                    spawnTime += Time.deltaTime;

                    //소환 시간이 설정한 소환시간이 되었을 때 소환하는 알고리즘
                    if (spawnTime >= setSpawnTime)
                    {
                        //적 생성
                        CreateEnemy();

                        //소환 시간 초기화
                        spawnTime = 0;
                    }
                    break;
                }
        }
    }

    //소환 시작
    void SpawnStart()
    {
        //적 생성
        CreateEnemy();

        //소환 상태를 Spawn 상태로 변경
        spawnState = SpawnState.Spawn;
    }

    //적 소환
    public void CreateEnemy()
    {
        //적 생성 Instantiate(원본, 위치, 회전)
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

        //적의 타겟 설정
        enemy.GetComponent<EnemyMove>().Setup(target);

        //enemy.GetComponent<Enemy>().Setup(target);
    }
}
