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
        // ���� Ű�� ������ ���� �غ� �Ǿ�������, ���� ���� �ִٴ� ���� 
        if (Input.GetKeyDown(KeyCode.M) && HaveThrows)
        {
            Throw ();
        }
    }

    private void Throw()
    {
        //readyToThrow = false;

        //�ν��Ͻ�ȭ ����� ����Ͽ� ������ ���� ��ü ����
        GameObject projectile = Instantiate(objectToThrow, attackPoint.position, cam.rotation);

        //���������ϴ� ��ü�� ������ٵ� ������Ҹ� ������ �������� �ϴ� ���� ��� 
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

        Vector3 forceDirection = cam.transform.forward;

        RaycastHit hit;

        if (Physics.Raycast(cam.position, cam.forward, out hit, 500f))
        {
            forceDirection = (hit.point - attackPoint.position).normalized;
        }

        //�׻� �����ִ� �������� ����.
        Vector3 forceToAdd = forceDirection * throwForce + transform.up * throwUpwardForce;

        projectileRb.AddForce(forceToAdd, ForceMode.Impulse);

        HaveThrows = false;

        //������ ���� ������ �Ⱥ��̰� ����
        GetComponent<PlayerTest>().bag.SetActive(false);
        
        
        //��Ÿ��
        //Invoke(nameof(ResetThrow), throwCooldown);


    }

    private void ResetThrow()
    {
        readyToThrow = true;
    }
}
