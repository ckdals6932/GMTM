using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingUI : MonoBehaviour
{

    //������ �����ϴ� ���� ������ �� ����� UI
    public List<GameObject> bagUI = new List<GameObject>();

    //ȹ���� ���� ����
    public int bagstack = 0;

    //ȹ���� ���� ��
    public int getmoney;

    //ȹ���� ������ ��
    public int getbag;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
                
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Bag")
        {
            Destroy(other, 0.1f);
            getmoney += 1000000;

            getbag++;
            bagstack++;
        }
        for(int i = 0; i < bagstack; i++)
        {
            bagUI[i].SetActive(true);
        }
    }
}
