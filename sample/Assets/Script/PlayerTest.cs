using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;

public class PlayerTest : MonoBehaviour
{
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

    public XRController controller = null;

    ////��ȣ�ۿ�� ���� ���ư��°�
    //[Header("Radial Timers")]
    //[SerializeField] private float indicatorTimer = 1.0f;
    //[SerializeField] private float maxIndicatorTimer = 1.0f;

    //[Header("UI Indicator")]
    //[SerializeField] private Image radialIndicatorUI = null;

    //[Header("key Codes")]
    //[SerializeField] private KeyCode selectKey = KeyCode.Mouse0;
    //[Header("Unity Event")]
    //[SerializeField] private UnityEvent myEvent = null;

    //private bool shouldUpdate = false;

    // Start is called before the first frame update
    void Start()
    {
        //ThrowingTutorial call = GameObject.Find("NoSteamVRFallbackObjects").GetComponent<ThrowingTutorial>();
        theAudio = GetComponent<AudioSource>();
    }


    // Update is called once per frame
    void Update()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;


        if(controller.inputDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool XButton))
        {
            if (XButton == true)
            {
                
            }
        }
        //if (Input.GetMouseButtonDown(0))
        //{
        //    if (Physics.Raycast(ray, out hit))
        //    {
        //        if (hit.transform.gameObject.tag == "EventKey")
        //        {
        //            theAudio.clip = clip[0];
        //            theAudio.Play();
        //        }
        //    }
        //} 

        if (Input.GetMouseButton(0))
        {
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.transform.gameObject);


                //�̺�Ʈ ��ü�� ���� ��ȣ�ۿ�
                if (hit.transform.gameObject.tag == "EventKey")
                {
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
                        if (hit.transform.gameObject.name == "mesh_props_keyboard_01_key")
                        {
                            warnning.GetComponent<warnning>().event_check_1 = true;
                            for(int i = 0; i < warnning.event_1.Count; i++)
                            {
                                warnning.event_1[i].GetComponent<Outline>().enabled = false;
                            }
                            key.SetActive(true);
                        }
                        else
                        {
                            hit.transform.gameObject.GetComponent<MeshCollider>().enabled = false;
                            hit.transform.gameObject.GetComponent<Outline>().enabled = false;
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
                    Destroy(hit.transform.gameObject, 0.1f);
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
        if (Input.GetMouseButtonUp(0))
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