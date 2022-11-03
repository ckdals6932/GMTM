using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public PlayerTest playertest;

    public GameObject canvas;

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
                m_IsButtonDowning = false;
                playertest.text.SetActive(false);
                anim.SetBool("isCheck", true);
                gameObject.GetComponent<BoxCollider>().enabled = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Player door Enter");
        canvas.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Player door Exit");
        canvas.SetActive(false);
    }
    public  void PointerDown()
    {
        m_IsButtonDowning = true;
    }

    public void PointerUp()
    {
        m_IsButtonDowning = false;
        playertest.text.SetActive(false);
        playertest.LimitTime = 5.5f;
    }
    
}
