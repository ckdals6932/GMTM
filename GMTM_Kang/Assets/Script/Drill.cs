using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drill : MonoBehaviour
{
    public vaultDoor vaultdoor;

    public PlayerTest playertest;

    private float drillFirst = 10;

    private float drillsecond = 10;

    public EventManager eventManager;

    public Animator dooranim;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DrillTime();
    }

    public void DrillTime()
    {
        if (drillFirst > 0)
        {
            drillFirst -= Time.deltaTime;
            Debug.Log(drillFirst);
            Debug.Log(drillsecond);
        }
        else if (drillFirst <= 0 && !eventManager.drillFixSucces)
        {
            eventManager.drillBroken = true;
            Debug.Log(drillFirst);
            Debug.Log(drillsecond);
        }
        else if (eventManager.drillFixSucces && drillsecond > 0 && !eventManager.drillBroken)
        {
            drillsecond -= Time.deltaTime;
            Debug.Log(drillFirst);
            Debug.Log(drillsecond);
        }
        else if (drillsecond < 0 && drillFirst < 0)
        {
            Destroy(gameObject);
            gameObject.SetActive(false);
            dooranim.SetBool("isopen", true);
            eventManager.vaultEvent = true;

        }
    }
}
