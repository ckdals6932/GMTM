using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drill : MonoBehaviour
{
    public vaultDoor vaultdoor;

    public PlayerTest playertest;

    private float drillFirst = 10;

    private float drillsecond = 10;

    public EventManager eventManager;

    public Animator dooranim;
    public GameObject drillpriview;


    public bool DrillOnOff = false;

    public bool DrillBroken = false;

    private AudioSource theAudio;

    public GameObject arrampanel;

    [SerializeField]
    private AudioClip[] clip;

    private bool firstSound = false;
    // Start is called before the first frame update
    void Start()
    {
        drillpriview.SetActive(false);
        theAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        DrillTime();
    }

    public void DrillTime()
    {
        if (drillFirst > 0)
        {
            if (!firstSound)
            {
                theAudio.clip = clip[0];
                theAudio.Play();
            }

            firstSound = true;

            drillFirst -= Time.deltaTime;
            //Debug.Log(drillFirst);
            //Debug.Log(drillsecond);
        }
        else if (drillFirst <= 0 && !eventManager.drillFixSucces)
        {
            if (firstSound)
            {
                theAudio.clip = clip[0];
                theAudio.Stop();

                theAudio.clip = clip[1];
                theAudio.Play();
            }

            firstSound = false;

            eventManager.drillBroken = true;
            DrillBroken = true;
            //Debug.Log(drillFirst);
            //Debug.Log(drillsecond);
        }
        else if (eventManager.drillFixSucces && drillsecond > 0 && !eventManager.drillBroken)
        {
            if (!firstSound)
            {
                theAudio.clip = clip[1];
                theAudio.Stop();

                theAudio.clip = clip[0];
                theAudio.Play();
            }

            firstSound = true;

            DrillBroken = true;
            drillsecond -= Time.deltaTime;
            //Debug.Log(drillFirst);
            //Debug.Log(drillsecond);
        }
        else if (drillsecond < 0 && drillFirst < 0)
        {
            theAudio.clip = clip[0];
            theAudio.Stop();
            playertest.arrampanels[2].SetActive(false);

            Destroy(gameObject);
            drillpriview.SetActive(false);
            gameObject.SetActive(false);
            dooranim.SetBool("isopen", true);
            eventManager.vaultEvent = true;
            arrampanel.SetActive(true);



        }
    }
}
