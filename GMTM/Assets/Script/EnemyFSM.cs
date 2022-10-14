using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;



public class EnemyFSM : MonoBehaviour
{
    public enum EnemyState { None = -1, Idle = 0, Wander, Pursuit, Attack }

    public Animator enemyAnim;

    private RaycastHit hit;

    public bool seePlayer;

    [Header("Pursuit")]
    [SerializeField]
    private float targetRecognitionRange = 1000000000000;         //�ν� ����(�� ���� �ȿ� ������ "Pursuit" ���·� ����)
    [SerializeField]
    private float pursuitLimitRange = 10;           //���� ���� (�� ���� �ٱ����� ������ "Wander" ���·� ����)

    [Header("Attack")]
    [SerializeField]
    private GameObject enemyBulletPrefab;            //�߻�ü ������
    [SerializeField]
    private Transform enemyBulletPoint;             //�߻�ü ���� ��ġ
    [SerializeField]
    private float attackRange = 20;                  //���� ����(�� ���� �ȿ� ������ "Attack" ���·� ����)
    [SerializeField]
    private float attackRate = 1;                   //���� �ӵ�

    private EnemyState enemyState = EnemyState.None; // ���� �� �ൿ

    public float attackCoolTime = 0;               //���� �ֱ� ����          

    private Stateus status;                     // �̵��ӵ� ���� ����
    private NavMeshAgent navMeshAgent;          // �̵� ��� ���� NavMeshAgent
    private Transform target;                   // ���� ���� ���(�÷��̾�)

    //private void Awake()
    public void Setup(Transform target)
    {
        status = GetComponent<Stateus>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        this.target = target;

        // NavMeshAgent ������Ʈ���� ȸ���� ������Ʈ���� �ʵ��� ����
        navMeshAgent.updateRotation = false;
    }
    private void OnEnable()
    {
        // ���� Ȱ��ȭ�� �� ���� ���¸� "���"�� ����
        ChangeState(EnemyState.Idle);

        EnemyAnimUpdate(0);
    }


    private void OnDisable()
    {
        // ���� ��Ȱ��ȭ�� �� ���� ������� ���¸� �����ϰ�, ���¸� "None"���� ����
        StopCoroutine(enemyState.ToString());

        enemyState = EnemyState.None;
    }
    private void ChangeState(EnemyState newState)
    {
        //���� ������� ���¿� �ٲٷ��� �ϴ� ���°� ������ �ٲ� �ʿ䰡 ���� ������ return
        if (enemyState == newState) return;

        //������ ������̴� ���� ����
        StopCoroutine(enemyState.ToString());

        //���� ���� ���¸� newState�� ����
        enemyState = newState;

        //���ο� ���� ���
        StartCoroutine(enemyState.ToString());
    }

    private IEnumerator Idle()
    {
        //n�� �Ŀ� "��ȸ" ���·� �����ϴ� �ڷ�ƾ ����
        StartCoroutine("AutoChangeFromIdleToWander");

        while (true)
        {
            //"���" ������ �� �ϴ� �ൿ
            EnemyAnimUpdate(0);

            //Ÿ����� �Ÿ��� ���� �ൿ ���� (��ȸ, �߰�, ���Ÿ� ����)
            CalculateDistanceToTargetAndSelectState();

            yield return null;
        }
    }

    private IEnumerator Attack()
    {
        //������ ���� �̵��� ���ߵ��� ����
        navMeshAgent.ResetPath();
        EnemyAnimUpdate(2);
        while (true)
        {
            //Ÿ�� ���� �ֽ�
            LookRotationToTarget();

            //Ÿ����� �Ÿ��� ���� �ൿ ���� (��ȸ, �߰�, ���Ÿ� ����)
            CalculateDistanceToTargetAndSelectState();

            if (Time.time - attackCoolTime > attackRate)
            {
                //�����ֱⰡ �Ǿ�� ������ �� �ֵ��� �ϱ� ���� ���� �ð� ����
                attackCoolTime = Time.time;

                //�߻�ü ����
                GameObject clone = Instantiate(enemyBulletPrefab, enemyBulletPoint.position, enemyBulletPoint.rotation);
                clone.GetComponent<EnemyProjectile>().Setup(target.position);
            }

            yield return null;
        }
    }

    private IEnumerator AutoChangeFromIdleToWander()
    {
        //1~4�� �ð� ���
        int changeTime = UnityEngine.Random.Range(1, 4);
        EnemyAnimUpdate(0);

        yield return new WaitForSeconds(changeTime);

        //���¸� "��ȸ"�� ����
        ChangeState(EnemyState.Pursuit);
    }

    private IEnumerator Pursuit()
    {
        float currentTime = 0;
        float maxTime = 6;

        while (true)
        {
            currentTime += Time.deltaTime;

            //�̵� �ӵ� ���� (��ȸ �� ���� �ȴ� �ӵ��� �̵�, �߰��� ���� �ٴ� �ӵ��� �̵�)
            navMeshAgent.speed = status.RunSpeed;

            //��ǥ��ġ�� ���� �÷��̾��� ��ġ�� ����
            navMeshAgent.SetDestination(target.position);

            if (seePlayer)
            {
                //Ÿ�� ������ ��� �ֽ��ϵ��� ��
                LookRotationToTarget();
            }
            else//////////////////////////////////////////////////////////�����ʿ�
            {
                LookRotationToTarget();
            }
            

            if (currentTime >= maxTime)
            {
                navMeshAgent.ResetPath();
                EnemyAnimUpdate(0);
                ChangeState(EnemyState.Idle);

            }

            //Ÿ����� �Ÿ��� ���� �ൿ ����(��ȸ, �߰�, ���Ÿ� ����)
            CalculateDistanceToTargetAndSelectState();

            yield return null;
        }
    }

    private void CalculateDistanceToTargetAndSelectState()
    {
        if (target == null) return;

        //�÷��̾�(target)�� ���� �Ÿ� ����� �Ÿ��� ���� �ൿ ����
        float distance = Vector3.Distance(target.position, transform.position);

        //�÷��̾ �ٶ󺸰� �ִ°�?
        EnemyRayCast();

        if (distance <= attackRange && seePlayer)
        {
            ChangeState(EnemyState.Attack);
        }
        else if(!seePlayer)
        {
            Vector3 to = new Vector3(navMeshAgent.destination.x, 0, navMeshAgent.destination.z);
            Vector3 from = new Vector3(transform.position.z, 0, transform.position.z);
            if ((to - from).sqrMagnitude <= attackRange-1)
            {
                CalculateDistanceToTargetAndSelectState();
            }
        }
    }

    private void LookRotationToTarget()
    {
        //��ǥ ��ġ
        Vector3 to = new Vector3(target.position.x, 0, target.position.z);

        //�� ��ġ
        Vector3 from = new Vector3(transform.position.x, 0, transform.position.z);

        //�ٷ� ����
        //transform.rotation = Quaternion.LookRotation(to - from);

        //������ ����
        Quaternion rotation = Quaternion.LookRotation(to - from);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.05f);
    }

    public void EnemyRayCast()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            Debug.Log(hit.collider.ToString());
            if (hit.collider.tag == "Player")
            {
                seePlayer = true;
            }
            else
            {
                seePlayer = false;
            }
        }
    }

    void Start()
    {

    }

    public void EnemyAnimUpdate(int i)
    {
        enemyAnim.SetInteger("EnemyState", i);
    }

    // Update is called once per frame
    void Update()
    {
        //EnemyRayCast();
        EnemyAnimUpdate(2);
    }
}