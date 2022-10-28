using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    // �ִ� ü��
    public int maxHealth;
    
    // ���� ü��
    public int curHealth;       
    
    //Ÿ��(�÷��̾�)
    //public Transform Target;
    public BoxCollider meleeArea;   // ���� ���� ����
    public GameObject bullet;
    public bool isChase;
    public bool isAttack;
    public bool isDead;

    protected Rigidbody rigid;
    protected BoxCollider boxCollider;
    protected MeshRenderer[] meshs;//B46 �ǰݷ���
    protected NavMeshAgent nav;// B48 �׺���̼�
    protected Animator anim;// B48 �ִϸ��̼�

    [SerializeField]
    private GameObject enemyBulletPrefab;            //�߻�ü ������
    [SerializeField]
    private Transform enemyBulletPoint;             //�߻�ü ���� ��ġ
    private Transform target;                   // ���� ���� ���(�÷��̾�)

    

    //private void Awake()
    public void Setup(Transform Target)
    {
        this.target = Target;
    }
    private void Awake()//B46
    {
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        meshs = GetComponentsInChildren<MeshRenderer>();//B46 �ǰݷ��� //GetComponent -> GetComponentInChildren ���� B48 ������Ʈ �߰�
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();// B48 �ִϸ��̼�


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
        //1~4�� �ð� ���
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
            targetRadius = 1f;// ȸ�� �ӵ�
            targetRange = 25f;  // Ÿ�� ����
            
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
        rigidBullet.velocity = transform.forward * 20;  // ����ü �ӵ�

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
