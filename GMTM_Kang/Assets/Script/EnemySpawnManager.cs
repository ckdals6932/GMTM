using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    //��ȯ ����
    public enum SpawnState
    {
        None, Spawn
    }
    public SpawnState spawnState = SpawnState.None;

    //��ȯ ����Ʈ
    public Transform spawnPoint;

    //�� ������
    public GameObject enemyPrefab;

    //��ȯ �ð�
    private float spawnTime;

    //���� ��ȯ �ð�
    public float setSpawnTime;

    //���� ��ǥ(�÷��̾�)
    [SerializeField]
    private Transform target;

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
                    //��ȯ �ð� ����
                    spawnTime += Time.deltaTime;

                    //��ȯ �ð��� ������ ��ȯ�ð��� �Ǿ��� �� ��ȯ�ϴ� �˰���
                    if (spawnTime >= setSpawnTime)
                    {
                        //�� ����
                        CreateEnemy();

                        //��ȯ �ð� �ʱ�ȭ
                        spawnTime = 0;
                    }
                    break;
                }
        }
    }

    //��ȯ ����
    void SpawnStart()
    {
        //�� ����
        CreateEnemy();

        //��ȯ ���¸� Spawn ���·� ����
        spawnState = SpawnState.Spawn;
    }

    //�� ��ȯ
    public void CreateEnemy()
    {
        //�� ���� Instantiate(����, ��ġ, ȸ��)
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

        //���� Ÿ�� ����
        enemy.GetComponent<EnemyFSM>().Setup(target);
    }
}
