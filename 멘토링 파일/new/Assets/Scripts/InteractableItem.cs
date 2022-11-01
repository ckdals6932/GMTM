using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class InteractableItem : MonoBehaviour
{
    public OculusControllerInput player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnItemUpdate(Hand hand)
    {
        Debug.Log(hand.gameObject.transform.name);
        switch(hand.gameObject.transform.name)
        {
            case "LeftHand":
                {
                    player.SendMessage("OnItemUpdateLeft", gameObject);
                    break;
                }
            case "RightHand":
                {
                    player.SendMessage("OnItemUpdateRight", gameObject);
                    break;
                }
        }
        
    }

    public void HandOnItemLeft()
    {
        player.SendMessage("PlayerHandOnLeft", gameObject);
    }

    public void HandOnItemRight()
    {
        player.SendMessage("PlayerHandOnRight", gameObject);
    }
}
