using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_3 : MonoBehaviour
{
    public PlayerTest playertest;



    public GameObject Drillpreview;

    public GameObject Drill;

    public bool vaultbutton_Click = false;

    public bool m_IsButtonDowning = false   ;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (vaultbutton_Click)
        //{
        //    canvas.SetActive(false);
        //    button.SetActive(false);
        //    text.SetActive(false);
        //    Drillpreview.SetActive(false);
        //}

        //if (m_IsButtonDowning)
        //{
        //    Debug.Log("UI Check");
        //    playertest.text.SetActive(true);
        //    playertest.LimitTime -= Time.deltaTime;
        //    playertest.text_Timer.text = "" + Mathf.Round(playertest.LimitTime);

        //    if (playertest.LimitTime <= 1)
        //    {
        //        canvas.SetActive(false);
        //        button.SetActive(false);
        //        Drillpreview.SetActive(false);
        //        playertest.text.SetActive(false);

        //        m_IsButtonDowning = false;

        //        Drill.SetActive(true);
        //    }
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        //canvas.SetActive(true);
        //button.SetActive(true);
        //text.SetActive(true);
        Drillpreview.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        gameObject.GetComponent<BoxCollider>().enabled = false;
    }

    public void PointerDown()
    {
        //m_IsButtonDowning = true;
        //playertest.LimitTime = 10f;
    }

    public void PointerUp()
    {
        //m_IsButtonDowning = false;
        //playertest.text.SetActive(false);
    }
}
