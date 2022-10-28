using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    // 최대 체력
    public int maxHealth;
    
    // 현재 체력
    public int curHealth;       
    
    //타겟(플레이어)
    //public Transform Target;
    public BoxCollider meleeArea;   // 근접 공격 범위
    public GameObject bullet;
    public bool isChase;
    public bool isAttack;
    public bool isDead;

    protected Rigidbody rigid;
    protected BoxCollider boxCollider;
    protected MeshRenderer[] meshs;//B46 피격로직
    protected NavMeshAgent nav;// B48 네비게이션
    protected Animator anim;// B48 애니매이션

    [SerializeField]
    private GameObject enemyBulletPrefab;            //발사체 프리팹
    [SerializeField]
    private Transform enemyBulletPoint;             //발사체 생성 위치
    private Transform target;                   // 적의 공격 대상(플레이어)

    

    //private void Awake()
    public void Setup(Transform Target)
    {
        this.target = Target;
    }
    private void Awake()//B46
    {
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        meshs = GetComponentsInChildren<MeshRenderer>();//B46 피격로직 //GetComponent -> GetComponentInChildren 수정 B48 오브젝트 추가
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();// B48 애니매이션


        Invoke("ChaseStart", 2);
    }

    void ChaseStart()
    {
        isChase = true;
        anim.SetBool("isWalk", true);
    }


    void FreezeVelocity()
    {
        if (isChase)
        {
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private IEnumerator waiting()
    {
        //1~4초 시간 대기
        int changeTime = UnityEngine.Random.Range(1, 4);

        yield return new WaitForSeconds(changeTime);

        yield return new WaitForSeconds(changeTime);
    }

    void Targeting()
    {
        if (!isDead)
        {       
            float targetRadius = 0;
            float targetRange = 0;
            targetRadius = 1f;// 회전 속도
            targetRange = 25f;  // 타켓 범위
            
            LookRotationToTarget();
            
            RaycastHit[] rayHits =
            Physics.SphereCastAll(transform.position,
                                        targetRadius,
                                        transform.forward,
                                        targetRange,
                                        LayerMask.GetMask("Player"));

            

            if (rayHits.Length > 0 && !isAttack)
            {
                StartCoroutine(Attack());
            }
        }
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
   
    IEnumerator Attack()
    {
        isChase = false;
        isAttack = true;
        anim.SetBool("isAttack", true);

        yield return new WaitForSeconds(0.5f);
        LookRotationToTarget();
        //GameObject clone = Instantiate(enemyBulletPrefab, enemyBulletPoint.position, enemyBulletPoint.rotation);
        //clone.GetComponent<EnemyProjectile>().Setup(target.position);
        GameObject instantBullet = Instantiate(enemyBulletPrefab, enemyBulletPoint.position, enemyBulletPoint.rotation);
        Rigidbody rigidBullet = instantBullet.GetComponent<Rigidbody>();
        rigidBullet.velocity = transform.forward * 20;  // 투사체 속도

        yield return new WaitForSeconds(0.2f);

        isChase = true;
        isAttack = false;
        anim.SetBool("isAttack", false);
    }

    void FixedUpdate()
    {
        //FreezeVelocity();
        Targeting();
    }

    void timeCheck()
    {
        float currentTime = 0;
        float maxTime = 6;

        currentTime += Time.deltaTime;

        if (currentTime >= maxTime)
        {
            nav.ResetPath();
            StartCoroutine("waiting");
        }

    }

    // Update is called once per frame
    void Update()
    {
        //Player player = playerObj.GetComponent<Player>();
        //if (player.heart <= 0)
        //{
        //    Destroy(selfObject);
        //}
        
        if (nav.enabled) 
        {
            nav.SetDestination(target.position);
            //nav.isStopped = !isChase;
            //anim.SetBool("isWalk", isChase);
        }
    }
}
