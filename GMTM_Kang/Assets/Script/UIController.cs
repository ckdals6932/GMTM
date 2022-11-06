using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public PlayerTest playertest;

    public GameObject canvas;

    public GameObject button;

    public GameObject text;

    public Animator anim;

    public bool m_IsButtonDowning;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (m_IsButtonDowning)
        {
            Debug.Log("UI Check");
            playertest.text.SetActive(true);
            playertest.LimitTime -= Time.deltaTime;
            playertest.text_Timer.text = "" + Mathf.Round(playertest.LimitTime);

            if (playertest.LimitTime <= 1)
            {
                canvas.SetActive(false);
                button.SetActive(false);
                m_IsButtonDowning = false;
                playertest.text.SetActive(false);
                anim.SetBool("isCheck", true);
                gameObject.GetComponent<BoxCollider>().enabled = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        canvas.SetActive(true);

        // �÷��̾ öâ �ݸ����� ������ �� 
        if (other.CompareTag("Player")) //�÷��̾�� �ٲ� ����
        {
            Debug.Log("Player Iron door Enter");
            
            if (playertest.isCardkey) {
                Debug.Log("player ī�� ��������");
                button.SetActive(true);
            }
            else
            {
                Debug.Log("player ī�� ����");
                text.SetActive(true);
            }
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        //�÷��̾ öâ�� �ݸ����� ������ �� 
        if (other.CompareTag("Player")) //�÷��̾�� �ٲ� ����
        {
            canvas.SetActive(false);
            Debug.Log("Player Iron door Exit");
            button.SetActive(false);
            text.SetActive(false);

        }
        
    }
    public  void PointerDown()
    {
        m_IsButtonDowning = true;
        playertest.text.SetActive(true);;
    }

    public void PointerUp()
    {
        m_IsButtonDowning = false;
        playertest.text.SetActive(false);
        playertest.LimitTime = 5.5f;
    }
    
}
