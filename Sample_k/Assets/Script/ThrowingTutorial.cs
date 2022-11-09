using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class ThrowingTutorial : MonoBehaviour
{


    [Header("References")]
    public Transform LeftHand;
    public Transform attackPoint;
    public GameObject objectToThrow;

    [Header("Settings")]
    public bool HaveThrows;
    public float throwCooldown;

    [Header("Throwing")]
    public float throwForce;
    public float throwUpwardForce;

    public InputActionProperty XButton;




    // Start is called before the first frame update
    void Start()
    {
        HaveThrows = false;
    }

    // Update is called once per frame
    void Update()
    {
        // ���� Ű�� ������ ���� �غ� �Ǿ�������, ���� ���� �ִٴ� ���� 
        if (XButton.action.WasPressedThisFrame() && HaveThrows)
        {
            Throw ();
        }
    }

    private void Throw()
    {
        //readyToThrow = false;

        //�ν��Ͻ�ȭ ����� ����Ͽ� ������ ���� ��ü ����
        GameObject projectile = Instantiate(objectToThrow, attackPoint.position, LeftHand.rotation);

        //���������ϴ� ��ü�� ������ٵ� ������Ҹ� ������ �������� �ϴ� ���� ��� 
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

        Vector3 forceDirection = LeftHand.transform.forward;

        RaycastHit hit;

        if (Physics.Raycast(LeftHand.position, LeftHand.forward, out hit, 500f))
        {
            forceDirection = (hit.point - attackPoint.position).normalized;
        }

        //�׻� �����ִ� �������� ����.
        Vector3 forceToAdd = forceDirection * throwForce + transform.up * throwUpwardForce;

        projectileRb.AddForce(forceToAdd, ForceMode.Impulse);

        HaveThrows = false;

        //������ ���� ������ �Ⱥ��̰� ����
        GetComponent<PlayerTest>().bag.SetActive(false);
    }
}
