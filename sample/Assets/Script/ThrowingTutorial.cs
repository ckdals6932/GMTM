using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ThrowingTutorial : MonoBehaviour
{


    [Header("References")]
    public Transform cam;
    public Transform attackPoint;
    public GameObject objectToThrow;

    [Header("Settings")]
    public bool HaveThrows;
    public float throwCooldown;

    [Header("Throwing")]
    public KeyCode throwKey = KeyCode.Mouse1;
    public float throwForce;
    public float throwUpwardForce;

    bool readyToThrow;



    // Start is called before the first frame update
    void Start()
    {
        readyToThrow = true;
        HaveThrows = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            Debug.Log("N");
        }
        // 던질 키를 눌렀고 던질 준비가 되어있으며, 던질 것이 있다는 조건 
        if (Input.GetKeyDown(KeyCode.M) && HaveThrows)
        {
            Throw ();
        }
    }

    private void Throw()
    {
        //readyToThrow = false;

        //인스턴스화 기능을 사용하여 던지고 싶은 객체 생성
        GameObject projectile = Instantiate(objectToThrow, attackPoint.position, cam.rotation);

        //던지고자하는 물체의 레지드바디 구성요소를 가져와 던지고자 하는 힘을 계산 
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

        Vector3 forceDirection = cam.transform.forward;

        RaycastHit hit;

        if (Physics.Raycast(cam.position, cam.forward, out hit, 500f))
        {
            forceDirection = (hit.point - attackPoint.position).normalized;
        }

        //항상 보고있는 방향으로 던짐.
        Vector3 forceToAdd = forceDirection * throwForce + transform.up * throwUpwardForce;

        projectileRb.AddForce(forceToAdd, ForceMode.Impulse);

        HaveThrows = false;

        //던지고 나서 가방을 안보이게 설정
        GetComponent<PlayerTest>().bag.SetActive(false);
        
        
        //쿨타임
        //Invoke(nameof(ResetThrow), throwCooldown);


    }

    private void ResetThrow()
    {
        readyToThrow = true;
    }
}
