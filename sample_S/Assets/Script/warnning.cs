using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class warnning : MonoBehaviour
{
    //AudioSource audioSoure;

    public List<GameObject> event_1 = new List<GameObject>();

    public bool event_check_1;

    public bool Eventwarnning;

    private AudioSource theAudio;

    private bool Sound = false;

    public GameObject arrampanel;

    // Start is called before the first frame update
    void Start()
    {
        theAudio = GetComponent<AudioSource>();
        event_check_1 = false;
    }

    // Update is called once per frame
    void Update()
    {
        //audioSoure.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            arrampanel.SetActive(true);

            Debug.Log("check");

            theAudio.Play();

            for (int i = 0; i < event_1.Count; i++)
            {
                event_1[i].GetComponent<Outline>().enabled = true;
                Eventwarnning = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }

    public bool warnningCheck()
    {
        return Eventwarnning;
    }
}
