using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drill : MonoBehaviour
{
    public vaultDoor vaultdoor;

    public PlayerTest playertest;

    private float drillFirst = 10;

    private float drillsecond = 10;

    public bool drillFixCheck = false;

    public bool drillBroken = false;

    public bool m_IsButtonDowning;

    public EventManager eventManager;


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
        }
    }

    public void PointerDown()
    {
        m_IsButtonDowning = true;
        playertest.LimitTime = 10f;
    }

    public void PointerUp()
    {
        m_IsButtonDowning = false;
        playertest.text.SetActive(false);
    }
}
