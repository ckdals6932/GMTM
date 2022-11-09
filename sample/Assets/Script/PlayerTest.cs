using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class PlayerTest : MonoBehaviour
{
    public InputActionProperty LeftTriger;

    public GameObject LeftHand;

    //�浹ü Ȯ��
    public GameObject hitObject;

    private AudioSource theAudio;

    [SerializeField]
    private AudioClip[] clip;

    public warnning warnning;

    public EventManager eventManager;

    public bool EventKey;

    // �ҿ� �ð�
    public float LimitTime = 5f;

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

    public GameObject Drill;

    private GameObject DrillFreeView;

    //��ȣ�ۿ�� ���� ���ư��°�
    [Header("Radial Timers")]
    [SerializeField] private float indicatorTimer = 1.0f;

    [Header("UI Indicator")]
    [SerializeField] private Image radialIndicatorUI = null;
    [SerializeField] public GameObject CircleImage;

    private bool trigerCheck = false;

    // Start is called before the first frame update
    void Start()
    {
        theAudio = GetComponent<AudioSource>();
    }


    // Update is called once per frame
    void Update()
    {
        //���� Ʈ���� ��ư�� ������ ��
        if (LeftTriger.action.WasPressedThisFrame())
        {
            trigerCheck = true;
        }


        //���� Ʈ���� ��ư�� ���� ��
        if (LeftTriger.action.WasReleasedThisFrame())
        {
            trigerCheck = false;
            Debug.Log("released");
            //����Ʈ �ð� ���� 
            LimitTime = 5f;
            text.SetActive(false);

            if (keyboardSound)
            {
                keyboardSound = false;
                theAudio.clip = clip[0];
                theAudio.Stop();
            }

            CircleImage.SetActive(false);
            indicatorTimer = 0f;
            radialIndicatorUI.fillAmount = indicatorTimer;
        }

        triggerOnoff(trigerCheck);

    }

    private void triggerOnoff(bool onoff)
    {
        if (onoff)
        {
            RaycastHit hit;
            if (Physics.Raycast(LeftHand.transform.position, LeftHand.transform.forward, out hit))
            {
                //�浹ü ���
                hitObject = hit.transform.gameObject;

                //�̺�Ʈ ��ü�� ���� ��ȣ�ۿ�
                if (hit.transform.gameObject.tag == "EventKey")
                {
                    text.SetActive(true);
                    CircleImage.SetActive(true);
                    LimitTime -= Time.deltaTime;
                    text_Timer.text = "" + LimitTime.ToString("F1") + "s";
                    indicatorTimer += Time.deltaTime / LimitTime;
                    radialIndicatorUI.fillAmount = indicatorTimer;

                    if (!keyboardSound)
                    {
                        keyboardSound = true;
                        theAudio.clip = clip[0];
                        theAudio.Play();
                    }

                    if (LimitTime <= 0)
                    {
                        Debug.Log("Event key Check");
                        text.SetActive(false);
                        CircleImage.SetActive(false);

                        theAudio.clip = clip[0];
                        theAudio.Stop();

                        //Ȱ��ȭ�� ������Ʈ �ذ��� �Ͽ��� ��
                        if (hit.transform.gameObject.name == "mesh_props_keyboard_01_key")
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
                            hit.transform.gameObject.GetComponent<MeshCollider>().enabled = false;
                            hit.transform.gameObject.GetComponent<Outline>().enabled = false;
                        }
                    }
                }

                //�帱 ������Ʈ ��ġ
                if (hit.transform.gameObject.tag == "DrillFreeView")
                {
                    Debug.Log("Check222");

                    text.SetActive(true);
                    CircleImage.SetActive(true);
                    LimitTime -= Time.deltaTime;
                    text_Timer.text = "" + LimitTime.ToString("F1") + "s";
                    //indicatorTimer += Time.deltaTime / LimitTime;
                    //radialIndicatorUI.fillAmount = indicatorTimer;

                    if (LimitTime <= 0)
                    {
                        Debug.Log("Check");
                        Drill.SetActive(true);
                        text.SetActive(false);
                        CircleImage.SetActive(false);
                        hit.transform.gameObject.SetActive(false);

                    }

                }

                //�帱 ������Ʈ ����
                if (hit.transform.gameObject.tag == "Drill")
                {
                    if (eventManager.drillBroken)
                    {
                        text.SetActive(true);
                        CircleImage.SetActive(true);
                        LimitTime -= Time.deltaTime;
                        text_Timer.text = "" + LimitTime.ToString("F1") + "s";
                        indicatorTimer += Time.deltaTime / LimitTime;
                        radialIndicatorUI.fillAmount = indicatorTimer;

                        if (LimitTime <= 0)
                        {
                            Debug.Log("Check");
                            eventManager.drillFixSucces = true;
                            text.SetActive(false);
                            CircleImage.SetActive(false);
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
                    text.SetActive(true);
                    CircleImage.SetActive(true);
                    LimitTime -= Time.deltaTime;
                    text_Timer.text = "" + LimitTime.ToString("F1") + "s";

                    indicatorTimer += Time.deltaTime / LimitTime;
                    radialIndicatorUI.fillAmount = indicatorTimer;

                    if (LimitTime <= 0)
                    {
                        Debug.Log("Check");
                        GetComponent<ThrowingTutorial>().HaveThrows = true;
                        text.SetActive(false);
                        CircleImage.SetActive(false);
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
                    CircleImage.SetActive(true);
                    LimitTime -= Time.deltaTime;
                    text_Timer.text = "" + LimitTime.ToString("F1") + "s";
                    indicatorTimer += Time.deltaTime / LimitTime;
                    radialIndicatorUI.fillAmount = indicatorTimer;

                    if (LimitTime <= 0)
                    {
                        Debug.Log("Check");
                        GetComponent<ThrowingTutorial>().HaveThrows = true;
                        text.SetActive(false);
                        CircleImage.SetActive(false);
                        Destroy(hit.transform.gameObject, 0.1f);
                        bag.SetActive(true);
                    }
                }
            }
        }
    }
}
