using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
public class PlayerTest : MonoBehaviour
{
    public GameObject LeftHand;

    private AudioSource theAudio;

    [SerializeField]
    private AudioClip[] clip;

    public warnning warnning;

    public EventManager eventManager;

    public bool EventKey;

    // �ҿ� �ð�
    public float LimitTime = 5.5f;

    //�ؽ�Ʈ ���� 
    public TextMeshProUGUI text_Timer;

    //�ð��� ��Ÿ���� �ؽ�Ʈ
    public GameObject text;

    //���� ������ �ڿ� ����
    public GameObject bag;

    //���� ��ǰ�� �̸��� �������� �ڵ�
    public string idcode;

    //���� �� ���踦 �����ִ°�
    public bool isCardkey = false;

    public GameObject key;

    public bool keyboardSound = false;

    public InputActionProperty XButton;


    // Start is called before the first frame update
    void Start()
    {
        //ThrowingTutorial call = GameObject.Find("NoSteamVRFallbackObjects").GetComponent<ThrowingTutorial>();
        theAudio = GetComponent<AudioSource>();
    }


    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        
        if (XButton.action.WasPressedThisFrame())
        {
            Debug.Log("tet 1");
            if (Physics.Raycast(LeftHand.transform.position, LeftHand.transform.forward, out hit))
            {
                Debug.Log(hit.transform.gameObject.tag);
                //�̺�Ʈ ��ü�� ���� ��ȣ�ۿ�
                if (hit.transform.gameObject.tag == "EventKey")
                {
                    Debug.Log(transform.gameObject.tag);
                    text.SetActive(true);
                    LimitTime -= Time.deltaTime;
                    text_Timer.text = "" + Mathf.Round(LimitTime);
                    if (!keyboardSound)
                    {
                        keyboardSound = true;
                        theAudio.clip = clip[0];
                        theAudio.Play();
                    }

                    if (LimitTime <= 1)
                    {
                        Debug.Log("Event key Check");
                        text.SetActive(false);

                        theAudio.clip = clip[0];
                        theAudio.Stop();

                        //Ȱ��ȭ�� ������Ʈ �ذ��� �Ͽ��� ��
                        if (transform.gameObject.name == "mesh_props_keyboard_01_key")
                        {
                            warnning.GetComponent<warnning>().event_check_1 = true;
                            for (int i = 0; i < warnning.event_1.Count; i++)
                            {
                                warnning.event_1[i].GetComponent<Outline>().enabled = false;
                            }
                            key.SetActive(true);
                        }
                        else
                        {
                            transform.gameObject.GetComponent<MeshCollider>().enabled = false;
                            transform.gameObject.GetComponent<Outline>().enabled = false;
                        }
                    }
                }
                //�帱 ������Ʈ ����
                if (hit.transform.gameObject.tag == "Drill")
                {
                    if (eventManager.drillBroken)
                    {
                        text.SetActive(true);
                        LimitTime -= Time.deltaTime;
                        text_Timer.text = "" + Mathf.Round(LimitTime);

                        if (LimitTime <= 1)
                        {
                            Debug.Log("Check");
                            eventManager.drillFixSucces = true;
                            text.SetActive(false);
                            eventManager.drillBroken = false;
                        }
                    }
                }

                //���迡 ���� ��ȣ�ۿ�
                if (hit.transform.gameObject.tag == "Key" && !isCardkey)
                {
                    key.SetActive(false);
                    isCardkey = true;
                    Destroy(transform.gameObject, 0.1f);
                }

                // �� ������Ʈ�� ���� ��ȣ�ۿ�
                if (transform.gameObject.tag == "Money")
                {

                    idcode = transform.gameObject.name;
                    Debug.Log("Money Check");
                    text.SetActive(true);
                    LimitTime -= Time.deltaTime;
                    text_Timer.text = "" + Mathf.Round(LimitTime);

                    if (LimitTime <= 1)
                    {
                        Debug.Log("Check");
                        GetComponent<ThrowingTutorial>().HaveThrows = true;
                        text.SetActive(false);
                        Destroy(transform.gameObject, 0.1f);
                        bag.SetActive(true);
                    }
                }

                // ���濡 ���� ��ȣ�ۿ�
                if (transform.gameObject.tag == "Bag")
                {
                    idcode = transform.gameObject.name;
                    Debug.Log("bag Check");
                    text.SetActive(true);
                    LimitTime -= Time.deltaTime;
                    text_Timer.text = "" + Mathf.Round(LimitTime);

                    if (LimitTime <= 1)
                    {
                        Debug.Log("Check");
                        GetComponent<ThrowingTutorial>().HaveThrows = true;
                        text.SetActive(false);
                        Destroy(transform.gameObject, 0.1f);
                        bag.SetActive(true);
                    }
                }
            }
        }
        if (XButton.action.WasReleasedThisFrame())
        {
            //����Ʈ �ð� ���� 
            LimitTime = 5.5f;
            text.SetActive(false);

            if(keyboardSound)
            {
                keyboardSound = false;
                theAudio.clip = clip[0];
                theAudio.Stop();
            }
        }
    }
}
