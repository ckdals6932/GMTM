using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlayerTest : MonoBehaviour
{
    public warnning warnning;


    public bool EventKey;

    // �ҿ� �ð�
    public float LimitTime;

    //�ؽ�Ʈ ���� 
    public TextMeshProUGUI text_Timer;

    //�ð��� ��Ÿ���� �ؽ�Ʈ
    public GameObject text;

    //���� ������ �ڿ� ����
    public GameObject bag;

    public string idcode;

    // Start is called before the first frame update
    void Start()
    {
        LimitTime = 5.5f;
        //ThrowingTutorial call = GameObject.Find("NoSteamVRFallbackObjects").GetComponent<ThrowingTutorial>();

    }

    // Update is called once per frame
    void Update()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Input.GetMouseButton(0))
        {
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.transform.gameObject);

                //�̺�Ʈ ��ü�� ���� ��ȣ�ۿ�
                if (hit.transform.gameObject.tag == "EventKey")
                {
                    //����Ʈ �ð� ���� 

                    text.SetActive(true);
                    LimitTime -= Time.deltaTime;
                    text_Timer.text = "" + Mathf.Round(LimitTime);
                    if (LimitTime <= 1)
                    {
                        Debug.Log("Event key Check");
                        text.SetActive(false);

                        //Ȱ��ȭ�� ������Ʈ �ذ��� �Ͽ��� ��
                        if (hit.transform.gameObject.name == "mesh_props_keyboard_01_key")
                        {
                            warnning.GetComponent<warnning>().event_check_1 = true;
                            for(int i = 0; i < warnning.event_1.Count; i++)
                            {
                                warnning.event_1[i].GetComponent<Outline>().enabled = false;
                            }
                        }
                        else
                        {
                            hit.transform.gameObject.GetComponent<MeshCollider>().enabled = false;
                            hit.transform.gameObject.GetComponent<Outline>().enabled = false;
                        }

                        
                    }
                }

                // �� ������Ʈ�� ���� ��ȣ�ۿ�
                if (hit.transform.gameObject.tag == "Money")
                {

                    idcode = hit.transform.gameObject.name;
                    Debug.Log("Money Check");
                    text.SetActive(true);
                    LimitTime -= Time.deltaTime;
                    text_Timer.text = "" + Mathf.Round(LimitTime);

                    if (LimitTime <= 1)
                    {
                        Debug.Log("Check");
                        GetComponent<ThrowingTutorial>().HaveThrows = true;
                        text.SetActive(false);
                        Destroy(hit.transform.gameObject, 0.1f);
                        bag.SetActive(true);
                    }
                }

                // ���濡 ���� ��ȣ�ۿ�
                if (hit.transform.gameObject.tag == "Bag")
                {
                    idcode = hit.transform.gameObject.name;
                    Debug.Log("bag Check");
                    text.SetActive(true);
                    LimitTime -= Time.deltaTime;
                    text_Timer.text = "" + Mathf.Round(LimitTime);

                    if (LimitTime <= 1)
                    {
                        Debug.Log("Check");
                        GetComponent<ThrowingTutorial>().HaveThrows = true;
                        text.SetActive(false);
                        Destroy(hit.transform.gameObject, 0.1f);
                        bag.SetActive(true);
                    }
                }


            }
        }

        if(Input.GetMouseButtonUp(0))
        {
            //����Ʈ �ð� ���� 
            LimitTime = 5.5f;
            text.SetActive(false);
        }

    }

}