using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingUI : MonoBehaviour
{

    //가방을 정산하는 곳에 던졌을 때 생기는 UI
    public List<GameObject> bagUI = new List<GameObject>();

    //획득한 가방 스택
    public int bagstack = 0;

    //획득한 돈의 양
    public int getmoney;

    //획득한 가방의 수
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
