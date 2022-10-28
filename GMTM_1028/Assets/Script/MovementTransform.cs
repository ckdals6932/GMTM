using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementTransform : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 0.0f;
    [SerializeField]
    private Vector3 moveDierction = Vector3.zero;

    //�̵������� �����Ǹ� �˾Ƽ� �̵��ϵ��� ��

    private void Update()
    {
        transform.position += moveDierction * moveSpeed * Time.deltaTime;
    }

    //�ܺο��� �Ű������� �̵� ������ ����
    public void MoveTo(Vector3 direction)
    {
        moveDierction = direction;
    }
}
