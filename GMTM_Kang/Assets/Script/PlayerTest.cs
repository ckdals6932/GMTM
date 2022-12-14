using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlayerTest : MonoBehaviour
{
    private AudioSource theAudio;

    [SerializeField]
    private AudioClip[] clip;

    public warnning warnning;

    public EventManager eventManager;

    public bool EventKey;

    // 소요 시간
    public float LimitTime = 5.5f;

    //텍스트 문자 
    public TextMeshProUGUI text_Timer;

    //시간를 나타내는 텍스트
    public GameObject text;

    public GameObject image;

    //돈을 얻으면 뒤에 가방
    public GameObject bag;

    //얻은 물품의 이름을 가져오는 코드
    public string idcode;

    //문을 열 열쇠를 갖고있는가
    public bool isCardkey = false;

    public GameObject key;

    public bool keyboardSound = false;


    // Start is called before the first frame update
    void Start()
    {
        //ThrowingTutorial call = GameObject.Find("NoSteamVRFallbackObjects").GetComponent<ThrowingTutorial>();
        theAudio = GetComponent<AudioSource>();
        LimitTime = 5.5f;
    }


    // Update is called once per frame
    void Update()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;



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
                image.SetActive(true);
                //이벤트 매체에 대한 상호작용
                if (hit.transform.gameObject.tag == "EventKey")
                {
                    text.SetActive(true);
                    LimitTime -= Time.deltaTime;
                    text_Timer.text = "" + LimitTime + "s";
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

                        theAudio.clip = clip[0];
                        theAudio.Stop();

                        //활성화된 오브젝트 해결을 하였을 때
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

                //드릴 오브젝트 수리
                if (hit.transform.gameObject.tag == "Drill")
                {
                    if (eventManager.drillBroken)
                    {
                        text.SetActive(true);
                        LimitTime -= Time.deltaTime;
                        text_Timer.text = "" + LimitTime + "s";

                        if (LimitTime <= 0)
                        {
                            Debug.Log("Check");
                            eventManager.drillFixSucces = true;
                            text.SetActive(false);
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
                    Debug.Log("Money Check");
                    text.SetActive(true);
                    LimitTime -= Time.deltaTime;
                    text_Timer.text = "" + LimitTime + "s";

                    if (LimitTime <= 0)
                    {
                        Debug.Log("Check");
                        GetComponent<ThrowingTutorial>().HaveThrows = true;
                        text.SetActive(false);
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
                    LimitTime -= Time.deltaTime;
                    text_Timer.text = "" + LimitTime + "s";

                    if (LimitTime <= 0)
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
            //리미트 시간 설정 
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
