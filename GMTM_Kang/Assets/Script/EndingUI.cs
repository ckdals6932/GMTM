using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingUI : MonoBehaviour
{

    //È¹µæÇÑ µ·ÀÇ ¾ç
    public int getmoney;

    //È¹µæÇÑ °¡¹æÀÇ ¼ö
    public int getbag;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
                
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Bag")
        {
            Destroy(other, 0.1f);
            getmoney += 1000000;

            getbag++;
        }
    }
}
