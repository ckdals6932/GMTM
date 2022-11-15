using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Endding : MonoBehaviour
{
    public GameObject enddingpanel;
    public GameObject arrampanel;

    public EndingUI EndingUI;

    public EndTotal EndTotal;

    public PlayerTest PlayerTest;
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
        if(other.tag == "Player")
        {
            EndTotal.money.text = EndTotal.money.text + EndingUI.getmoney.ToString();
            EndTotal.bag.text = EndTotal.bag.text + EndingUI.getbag.ToString();
            EndTotal.kill.text = EndTotal.kill.text + PlayerTest.killcunt.ToString();
            arrampanel.SetActive(false);

            enddingpanel.SetActive(true);
        }
    }
}
