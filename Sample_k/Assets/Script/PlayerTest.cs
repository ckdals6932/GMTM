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

    //충돌체 확인
    public GameObject hitObject;

    private AudioSource theAudio;

    [SerializeField]
    private AudioClip[] clip;

    public warnning warnning;

    public EventManager eventManager;

    public bool EventKey;

    // 소요 시간
    public float LimitTime = 5f;

    //텍스트 문자 
    public TextMeshProUGUI text_Timer;

    //시간를 나타내는 텍스트
    public GameObject text;

    //돈을 얻으면 뒤에 가방
    public GameObject bag;

    //얻은 물품의 이름을 가져오는 코드
    public string idcode;

    //문을 열 열쇠를 갖고있는가
    public bool isCardkey = false;

    public GameObject key;

    public bool keyboardSound = false;

    //상호작용시 원이 돌아가는것
    [Header("Radial Timers")]
    [SerializeField] private float indicatorTimer = 1.0f;

    [Header("UI Indicator")]
    [SerializeField] private Image radialIndicatorUI = null;
    [SerializeField] public GameObject CircleImage;


    // Start is called before the first frame update
    void Start()
    {
        theAudio = GetComponent<AudioSource>();
    }


    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        //왼쪽 트리거 버튼을 눌렀을 때
        if (LeftTriger.action.WasPressedThisFrame())
        {
            if (Physics.Raycast(LeftHand.transform.position, LeftHand.transform.forward, out hit))
            {
                //충돌체 등록
                hitObject = hit.transform.gameObject;

                //이벤트 매체에 대한 상호작용
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

                        //활성화된 오브젝트 해결을 하였을 때
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

                //드릴 오브젝트 수리
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

                //열쇠에 대한 상호작용
                if (hit.transform.gameObject.tag == "Key" && !isCardkey)
                {
                    key.SetActive(false);
                    isCardkey = true;
                    Destroy(hit.transform.gameObject, 0.1f);
                }

                // 돈 오브젝트에 대한 상호작용
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
                // 가방에 대한 상호작용
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

        //왼쪽 트리거 버튼을 땠을 때
        if (LeftTriger.action.WasReleasedThisFrame())
        {
            //리미트 시간 설정 
            LimitTime = 5f;
            text.SetActive(false);

            if(keyboardSound)
            {
                keyboardSound = false;
                theAudio.clip = clip[0];
                theAudio.Stop();
            }

            CircleImage.SetActive(false);
            indicatorTimer = 0f;
            radialIndicatorUI.fillAmount = indicatorTimer;
        }
    }
}
