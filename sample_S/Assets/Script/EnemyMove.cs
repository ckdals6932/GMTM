using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyMove : MonoBehaviour
{
    //Enemy hp
    public float enemyHP = 20;

    //Enemy 최대 체력
    private float maxHp;

    //HP의 백분율
    private float hpPer;

    //적 UI(적 HP)
    public Transform enemyUI;

    //HP 슬라이더
    public Slider enemySlider;

    public enum EnemyState { None = -1, Idle = 0, Wander, Pursuit, Attack }

    public Animator enemyAnim;

    [Header("Pursuit")]
    [SerializeField]
    private float targetRecognitionRange = 15;         // ν      (         ȿ         "Pursuit"    ·      )
    [SerializeField]
    private float pursuitLimitRange = 20;           //          (         ٱ             "Wander"    ·      )

    [Header("Attack")]
    [SerializeField]
    private GameObject enemyBulletPrefab;            // ߻ ü       
    [SerializeField]
    private Transform enemyBulletPoint;             // ߻ ü        ġ
    [SerializeField]
    private float attackRange = 10;                  //         (         ȿ         "Attack"    ·      )
    [SerializeField]
    private float attackRate = 1;                   //      ӵ 

    [SerializeField]
    private EnemyState enemyState = EnemyState.None; //          ൿ

    public float attackCoolTime = 0;               //      ֱ                

    private Stateus status;                     //  ̵  ӵ           
    private NavMeshAgent navMeshAgent;          //  ̵           NavMeshAgent
    public Transform target;                   //              ( ÷  ̾ )

    RaycastHit[] rayHits;

    bool lookCheck;

    public PlayerTest playerTest;

    //private void Awake()
    public void Setup(GameObject target)
    {
        status = GetComponent<Stateus>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        this.target = target.transform;
        playerTest = target.GetComponent<PlayerTest>();

        // NavMeshAgent       Ʈ     ȸ           Ʈ      ʵ        
        navMeshAgent.updateRotation = false;

        //초기 HP를 설정
        maxHp = enemyHP;

        //HP 백문율화
        hpPer = enemyHP / maxHp;
    }
    private void OnEnable()
    {
        //      Ȱ  ȭ              ¸  "   "       
        ChangeState(EnemyState.Idle);

        //StartCoroutine(Pursuit());
    }

    private void Start()
    {
        
    }


    private void OnDisable()
    {
        //        Ȱ  ȭ                      ¸       ϰ ,    ¸  "None"         
        StopCoroutine(enemyState.ToString());

        enemyState = EnemyState.None;
    }
    private void ChangeState(EnemyState newState)
    {
        //                ¿   ٲٷ     ϴ     °          ٲ   ʿ䰡             return
        if (enemyState == newState) return;

        //             ̴           
        StopCoroutine(enemyState.ToString());

        //             ¸  newState       
        enemyState = newState;

        //   ο          
        StartCoroutine(enemyState.ToString());
    }

    private IEnumerator Idle()
    {
        enemyAnim.SetInteger("EnemyState", 0);
        //n    Ŀ  "  ȸ"    ·       ϴ   ڷ ƾ     
        StartCoroutine("AutoChangeFromIdleToWander");

        while (true)
        {

            //Ÿ       Ÿ          ൿ      (  ȸ,  ߰ ,    Ÿ      )
            CalculateDistanceToTargetAndSelectState();

            yield return null;
        }
    }

    private IEnumerator Attack()
    {
        //             ̵       ߵ        
        navMeshAgent.ResetPath();
        while (true)
        {
            //Ÿ         ֽ 
            LookRotationToTarget();


            //Ÿ       Ÿ          ൿ      (  ȸ,  ߰ ,    Ÿ      )
            CalculateDistanceToTargetAndSelectState();

            if (Time.time - attackCoolTime > attackRate)
            {
                enemyAnim.SetInteger("EnemyState", 2);

                //     ֱⰡ  Ǿ              ֵ     ϱ             ð      
                attackCoolTime = Time.time;

                // ߻ ü     
                GameObject clone = Instantiate(enemyBulletPrefab, enemyBulletPoint.position, enemyBulletPoint.rotation);
                clone.GetComponent<EnemyProjectile>().Setup(target.position);
            }

            yield return null;
        }
    }

    private IEnumerator AutoChangeFromIdleToWander()
    {
        //1~4    ð     
        int changeTime = UnityEngine.Random.Range(1, 3);

        yield return new WaitForSeconds(changeTime);

        //   ¸  "  ȸ"       
        ChangeState(EnemyState.Wander);
    }

    private IEnumerator Wander()
    {
        float currentTime = 0.0f;
        float maxTime = 5.0f;

        enemyAnim.SetInteger("EnemyState", 0);

        // ̵   ӵ      
        navMeshAgent.speed = status.WalkSpeed;



        //  ǥ   ġ     
        navMeshAgent.SetDestination(CalculateWanderPosition());

        //  ǥ   ġ   ȸ  
        Vector3 to = new Vector3(navMeshAgent.destination.x, 0, navMeshAgent.destination.z);
        Vector3 from = new Vector3(transform.position.x, 0, transform.position.z);
        transform.rotation = Quaternion.LookRotation(to - from);

        while (true)
        {
            currentTime += Time.deltaTime;

            //  ǥ   ġ        ϰ       ϰų   ʹ       ð        ȸ ϱ     ¿   ӹ          
            to = new Vector3(navMeshAgent.destination.x, 0, navMeshAgent.destination.z);
            from = new Vector3(transform.position.z, 0, transform.position.z);
            if ((to - from).sqrMagnitude < 0.01f || currentTime >= maxTime)
            {
                //   ¸  "   "       

                ChangeState(EnemyState.Idle);

            }

            //Ÿ       Ÿ          ൿ     (  ȸ,  ߰ ,    Ÿ      )/////////////////////
            CalculateDistanceToTargetAndSelectState();

            yield return null;
        }
    }

    private Vector3 CalculateWanderPosition()
    {
        float wanderRadius = 10;            //        ġ             ϴ             
        int wanderJitter = 0;               //    õ       (wanderJitterMin ~ wanderJitterMax)
        int wanderJitterMin = 0;            //  ּ      
        int wanderJitterMax = 360;          //  ִ      

        //         ĳ   Ͱ   ִ          ߽    ġ   ũ   (            ൿ         ʵ   )
        Vector3 rangePosition = Vector3.zero;
        Vector3 rangeScale = Vector3.one * 100.0f;

        //  ڽ      ġ    ߽            (wanderRadius)  Ÿ ,    õ      (wanderJitter)     ġ     ǥ     ǥ             
        wanderJitter = UnityEngine.Random.Range(wanderJitterMin, wanderJitterMax);
        Vector3 targetPosition = transform.position + SetAngle(wanderRadius, wanderJitter);

        //          ǥ  ġ    ڽ     ̵               ʰ      
        targetPosition.x = Mathf.Clamp(targetPosition.x, rangePosition.x - rangeScale.x * 0.5f, rangePosition.x + rangeScale.x * (0.5f));
        targetPosition.y = 0.0f;
        targetPosition.z = Mathf.Clamp(targetPosition.z, rangePosition.z - rangeScale.z * 0.5f, rangePosition.z + rangeScale.z * (0.5f));

        return targetPosition;
    }

    private Vector3 SetAngle(float radius, int angle)
    {
        Vector3 position = Vector3.zero;

        position.x = Mathf.Cos(angle) * radius;
        position.z = Mathf.Sin(angle) * radius;

        return position;
    }

    private IEnumerator Pursuit()
    {
        while (true)
        {
            enemyAnim.SetInteger("EnemyState", 1);

            // ̵   ӵ       (  ȸ          ȴ   ӵ     ̵ ,  ߰          ٴ   ӵ     ̵ )
            navMeshAgent.speed = status.RunSpeed;


            //  ǥ  ġ         ÷  ̾      ġ       
            navMeshAgent.SetDestination(target.position);






            //Ÿ               ֽ  ϵ      
            LookRotationToTarget();

            //Ÿ       Ÿ          ൿ     (  ȸ,  ߰ ,    Ÿ      )
            CalculateDistanceToTargetAndSelectState();

            yield return null;
        }
    }

    private void CalculateDistanceToTargetAndSelectState()
    {
        if (target == null) return;

        // ÷  ̾ (target)         Ÿ         Ÿ          ൿ     
        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= attackRange && rayHits.Length > 0)
        {
            //lookCheck = true;
            //navMeshAgent.isStopped = true;
            ChangeState(EnemyState.Attack);
        }
        //else if (distance <= targetRecognitionRange)
        else if (distance >= attackRange)
        {
            //lookCheck = false;
            //navMeshAgent.isStopped = false;
            ChangeState(EnemyState.Pursuit);
        }
        //else if (distance >= pursuitLimitRange)
        //{
        //    //lookCheck = false;
        //    //navMeshAgent.isStopped = false;
        //    ChangeState(EnemyState.Wander);
        //}
    }

    private void LookRotationToTarget()
    {

        //  ǥ   ġ
        Vector3 to = new Vector3(target.position.x, 0, target.position.z);

        //     ġ
        Vector3 from = new Vector3(transform.position.x, 0, transform.position.z);

        // ٷ      
        //transform.rotation = Quaternion.LookRotation(to - from);

        //           
        Quaternion rotation = Quaternion.LookRotation(to - from);
        transform.rotation = rotation;
    }

    private void onDrawGizmos()
    {
        //"  ȸ"            ̵        ǥ  
        Gizmos.color = Color.black;
        Gizmos.DrawRay(transform.position, navMeshAgent.destination - transform.position);

        //  ǥ  ν      
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, targetRecognitionRange);

        // ߰      
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, pursuitLimitRange);

        //         

    }




    public void EnemyAnimUpdate(int i)
    {
        enemyAnim.SetInteger("EnemyState", i);
    }

    // Update is called once per frame
    void Update()
    {
        rayHits =
            Physics.SphereCastAll(transform.position, attackRate, transform.forward, attackRange, LayerMask.GetMask("Player"));
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.tag == "PlayerBullet") //플레이어로 바꿀 예정
        {
            Debug.Log("Player Iron door Enter");
            int damage = 5;
            //HP감소
            enemyHP -= damage;

            //HP 백분율 계산
            hpPer = enemyHP / maxHp;

            //HP 슬라이더에 HP 잔여량 표시
            enemySlider.value = hpPer;

            //HP가 없을 때
            if (enemyHP <= 0)
            {
                playerTest.killcunt++;
                Destroy(gameObject);
            }
        }

    }
}