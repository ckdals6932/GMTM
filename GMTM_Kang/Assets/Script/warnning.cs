using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class warnning : MonoBehaviour
{
    //AudioSource audioSoure;

    public List<GameObject> event_1 = new List<GameObject>();

    public bool event_check_1;

    public bool Eventwarnning;


    // Start is called before the first frame update
    void Start()
    {
        event_check_1 = false;
    }

    // Update is called once per frame
    void Update()
    {
        //audioSoure.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("check");
            for (int i = 0; i < event_1.Count; i++)
            {
                event_1[i].GetComponent<Outline>().enabled = true;
                Eventwarnning = true;
            }
        }
    }

    public bool warnningCheck()
    {
        return Eventwarnning;
    }
}
