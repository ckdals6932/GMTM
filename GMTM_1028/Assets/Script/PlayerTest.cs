using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTest : MonoBehaviour
{
    ThrowingTutorial throwtutorial;
    public bool EventKey;

    // Start is called before the first frame update
    void Start()
    {
        //ThrowingTutorial call = GameObject.Find("NoSteamVRFallbackObjects").GetComponent<ThrowingTutorial>();

    }

    // Update is called once per frame
    void Update()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.transform.gameObject);

                if (hit.transform.gameObject.tag == "EventKey")
                {
                    Debug.Log("Check");
                    GetComponent<ThrowingTutorial>().HaveThrows = true;
                    Destroy(hit.transform.gameObject, 0.1f);
                }
            }
        }
    }

}
