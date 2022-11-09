using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class RadialIndicatorClick : MonoBehaviour
{
    //상호작용시 원이 돌아가는것
    [Header("Radial Timers")]
    [SerializeField] private float indicatorTimer = 1.0f;
    [SerializeField] private float maxIndicatorTimer = 1.0f;

    [Header("UI Indicator")]
    [SerializeField] private Image radialIndicatorUI = null;

    [Header("key Codes")]
    [SerializeField] private KeyCode selectKey = KeyCode.Mouse0;

    [Header("Unity Event")]
    [SerializeField] private UnityEvent myEvent = null;

    private bool shouldUpdate = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(selectKey))
        {

            indicatorTimer += Time.deltaTime / 5;
            radialIndicatorUI.fillAmount = indicatorTimer;

            
        }

        if (Input.GetKeyUp(selectKey))
        {
            indicatorTimer = 0f;
            radialIndicatorUI.fillAmount = indicatorTimer;

        }
    }
}
