using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableUI : MonoBehaviour
{
    private BoxCollider boxCollider;
    private RectTransform rectTransform;
    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        boxCollider = gameObject.AddComponent<BoxCollider>();

        boxCollider.size = rectTransform.sizeDelta;
    }
}
