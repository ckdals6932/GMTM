using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_3 : MonoBehaviour
{
    public GameObject canvas;
    public GameObject button;
    public GameObject text;

    public GameObject Drillpreview;

    public bool vaultbutton_Click = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (vaultbutton_Click)
        {
            canvas.SetActive(false);
            button.SetActive(false);
            text.SetActive(false);
            Drillpreview.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        canvas.SetActive(true);
        button.SetActive(true);
        text.SetActive(true);
        Drillpreview.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        gameObject.GetComponent<BoxCollider>().enabled = false;
    }
}
