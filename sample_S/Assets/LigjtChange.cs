using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LigjtChange : MonoBehaviour
{
    public Slider slider;
    public Light sceneLight;

    private void Update()
    {
        sceneLight.intensity = slider.value;
    }
}
