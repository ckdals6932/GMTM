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

    [Header("Attack")]
    [SerializeField]
    private GameObject enemyBulletPrefab;            //발사체 프리팹
    [SerializeField]
    private Transform enemyBulletPoint;             //발사체 생성 위치
    [SerializeField]
    private float attackRange = 20;                  //공격 범위(이 범위 안에 들어오면 "Attack" 상태로 변경)
    [SerializeField]
    private float attackRate = 1;                   //공격 속도

    private EnemyState enemyState = EnemyState.None; // 현재 적 행동

    public float attackCoolTime = 0;               //공격 주기 계산용          

    private Stateus status;                     // 이동속도 등의 정보
    private NavMeshAgent navMeshAgent;          // 이동 제어를 위한 NavMeshAgent
    private Transform target;                   // 적의 공격 대상(플레이어)

    //private void Awake()
    public void Setup(Transform target)
    {
        status = GetComponent<Stateus>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        this.target = target;

        // NavMeshAgent 컴포넌트에서 회전을 업데이트하지 않도록 설정
        navMeshAgent.updateRotation = false;
    }
    private void OnEnable()
    {
        // 적이 활성화될 때 적의 상태를 "대기"로 설정
        ChangeState(EnemyState.Idle);

        EnemyAnimUpdate(0);
    }


    private void OnDisable()
    {
        // 적이 비활성화될 때 현재 재생중인 상태를 종료하고, 상태를 "None"으로 설정
        StopCoroutine(enemyState.ToString());

        enemyState = EnemyState.None;
    }
    private void ChangeState(EnemyState newState)
    {
        //현재 재생중인 상태와 바꾸려고 하는 상태가 같으면 바꿀 필요가 없기 때문에 return
        if (enemyState == newState) return;

        //이전에 재생중이던 상태 종료
        StopCoroutine(enemyState.ToString());

        //현재 적의 상태를 newState로 설정
        enemyState = newState;

        //새로운 상태 재생
        StartCoroutine(enemyState.ToString());
    }

    private IEnumerator Idle()
    {
        EnemyAnimUpdate(2);
        //n초 후에 "배회" 상태로 변경하는 코루틴 실행
        StartCoroutine("AutoChangeFromIdleToWander");

        while (true)
        {
            ////"대기" 상태일 때 하는 행동
            //EnemyAnimUpdate(0);

            //타깃과의 거리에 따라 행동 선택 (배회, 추격, 원거리 공격)
            CalculateDistanceToTargetAndSelectState();

            yield return null;
        }
    }

    private IEnumerator Attack()
    {
        //공격할 때는 이동을 멈추도록 설정
        navMeshAgent.ResetPath();
        Debug.Log("stop");
        while (true)
        {
            //타겟 방향 주시
            LookRotationToTarget();

            //타깃과의 거리에 따라 행동 선택 (배회, 추격, 원거리 공격)
            CalculateDistanceToTargetAndSelectState();

            if (Time.time - attackCoolTime >= attackRate)
            {
                EnemyAnimUpdate(2); Debug.Log("anim");
                //발사체 생성
                GameObject clone = Instantiate(enemyBulletPrefab, enemyBulletPoint.position, enemyBulletPoint.rotation);
                clone.GetComponent<EnemyProjectile>().Setup(target.position);
                Debug.Log("shot");
                //공격주기가 되어야 공격할 수 있도록 하기 위해 현재 시간 저장
                attackCoolTime = Time.time;
            }
            yield return null;
        }
    }

    private IEnumerator AutoChangeFromIdleToWander()
    {
        //1~4초 시간 대기
        int changeTime = UnityEngine.Random.Range(1, 4);
        EnemyAnimUpdate(0);

        yield return new WaitForSeconds(changeTime);

        //상태를 "배회"로 변경
        ChangeState(EnemyState.Pursuit);
    }

    private IEnumerator Pursuit()
    {
        float currentTime = 0;
        float maxTime = 6;

        while (true)
        {
            currentTime += Time.deltaTime;

            //이동 속도 설정 (배회 할 때는 걷는 속도로 이동, 추격할 때는 뛰는 속도로 이동)
            navMeshAgent.speed = status.RunSpeed;

            //목표위치를 현재 플레이어의 위치로 설정
            navMeshAgent.SetDestination(target.position);
            EnemyAnimUpdate(1);

            if (seePlayer)
            {
                //타겟 방향을 계속 주시하도록 함
                LookRotationToTarget();
            }
            else//////////////////////////////////////////////////////////수정필요
            {
                LookRotationToTarget();
            }
            

            if (currentTime >= maxTime)
            {
                navMeshAgent.ResetPath();
                EnemyAnimUpdate(0);
                ChangeState(EnemyState.Idle);

            }

            //타깃과의 거리에 따라 행동 선택(배회, 추격, 원거리 공격)
            CalculateDistanceToTargetAndSelectState();

            yield return null;
        }
    }

    private void CalculateDistanceToTargetAndSelectState()
    {
        if (target == null) return;

        //플레이어(target)와 적의 거리 계산후 거리에 따라 행동 선택
        float distance = Vector3.Distance(target.position, transform.position);

        //플레이어를 바라보고 있는가?
        EnemyRayCast();

        if (distance <= attackRange)
        {
            ChangeState(EnemyState.Attack);
        }
        else
        {
            ChangeState(EnemyState.Pursuit);
        }
        //else if(!seePlayer)
        //{
        //    navMeshAgent.SetDestination(target.position);
        //    Vector3 to = new Vector3(navMeshAgent.destination.x, 0, navMeshAgent.destination.z);
        //    Vector3 from = new Vector3(transform.position.z, 0, transform.position.z);
        //    if ((to - from).sqrMagnitude <= attackRange-1)
        //    {
        //        CalculateDistanceToTargetAndSelectState();
        //    }
        //}
    }

    private void LookRotationToTarget()
    {
        //목표 위치
        Vector3 to = new Vector3(target.position.x, 0, target.position.z);

        //내 위치
        Vector3 from = new Vector3(transform.position.x, 0, transform.position.z);

        //바로 돌기
        //transform.rotation = Quaternion.LookRotation(to - from);

        //서서히 돌기
        Quaternion rotation = Quaternion.LookRotation(to - from);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.05f);
    }

    public void EnemyRayCast()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
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
        EnemyRayCast();
        EnemyAnimUpdate(2);
    }
}