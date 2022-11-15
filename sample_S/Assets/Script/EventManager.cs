using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    // Start is called before the first frame update
    public bool drillFixSucces = false;

    public bool drillBroken = false;

    public bool vaultEvent = false;

    public GameObject endingUI;

    void Start()
    {
        //GetComponent<EventManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(vaultEvent)
        {
            endingUI.SetActive(true);
        }
    }
}
