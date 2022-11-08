using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class vaultDoor : MonoBehaviour
{
    public PlayerTest playertest;

    public Event_3 event_3;

    public Animator anim;

    public GameObject drill;

    public bool m_IsButtonDowning;

    //�帱�� �Ϻ��� ����ƴ���.
    public bool drillCheck;

    public bool drillInstall = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (m_IsButtonDowning)
        {
            playertest.text.SetActive(true);
            playertest.LimitTime -= Time.deltaTime;
            playertest.text_Timer.text = "" + Mathf.Round(playertest.LimitTime);

            if (playertest.LimitTime <= 1)
            {
                event_3.vaultbutton_Click = true;

                //Ŭ���� ��� �ҽ�
                m_IsButtonDowning = false;
                playertest.text.SetActive(false);

                //��ȣ�ۿ��� �Ϸ�� �� �帱 ��ġ.
                drill.SetActive(true);
                drillInstall = true;
            }
        }

        if(drillCheck)
        {
            anim.SetBool("isOpen", true);
        }
    }

    public void PointerDown()
    {
        m_IsButtonDowning = true;
    }

    public void PointerUp()
    {
        m_IsButtonDowning = false;
        playertest.text.SetActive(false);
    }
}
