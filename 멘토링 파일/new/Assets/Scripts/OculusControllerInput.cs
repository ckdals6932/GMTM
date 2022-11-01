using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class OculusControllerInput : MonoBehaviour
{
    //입력소스정의
    //왼쪽 컨트롤러
    public SteamVR_Input_Sources leftHand = SteamVR_Input_Sources.LeftHand;
    
    //오른쪽 컨트롤러
    public SteamVR_Input_Sources rightHand = SteamVR_Input_Sources.RightHand;
    
    //양쪽 컨트롤러 전부
    public SteamVR_Input_Sources any = SteamVR_Input_Sources.Any;
    
    //헤드셋의 입력 소스
    public SteamVR_Input_Sources head = SteamVR_Input_Sources.Head;


    //액션 - 트리거 버튼(InteractUI)
    public SteamVR_Action_Boolean trigger = SteamVR_Actions.default_InteractUI;

    public SteamVR_Action_Boolean buttonX = SteamVR_Actions.default_X_Button;
    public SteamVR_Action_Boolean buttonY = SteamVR_Actions.default_Y_Button;

    //액션 - 그립 버튼의 잡기(GrapGrip)
    public SteamVR_Action_Boolean grip = SteamVR_Input.GetBooleanAction("GrabGrip");
    //public SteamVR_Action_Boolean grip1 = SteamVR_Actions.default_GrabGrip;

    //액션 - 햅틱(Haptic)
    public SteamVR_Action_Vibration haptic = SteamVR_Actions.default_Haptic;

    //액션 - 헤드셋의 착용 여부
    public SteamVR_Action_Boolean headSet = 
        SteamVR_Input.GetAction<SteamVR_Action_Boolean>("HeadsetOnHead", true);

    public GameObject onItemL, onItemR;
    public Transform handL, handR;

    public enum HandItemState
    {
        None, ItemHave
    }
    public HandItemState handItemStateLeft = HandItemState.None;
    public HandItemState handItemStateRight = HandItemState.None;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        ////Vive 장비의 초기화 여부 확인
        //if(SteamVR.initializedState != SteamVR.InitializedStates.InitializeSuccess)
        //{
        //    return;
        //}
       
        //헤드셋을 착용
        if(headSet.GetStateDown(head))
        {
            Debug.Log("HeadSet On");
        }
        //헤드셋 미착용
        else if(headSet.GetStateUp(head))
        {
            Debug.Log("HeadSet Removed");
        }

        //왼쪽 컨트롤러의 트리거를 당긴다면
	  if(SteamVR_Actions.default_InteractUI.GetStateDown(SteamVR_Input_Sources.RightHand))
        {
            Debug.Log("Right Trigger Clicked");
        }

      

      //양손 컨트롤러의 그립을 잡았을 경우
      if(grip.GetStateDown(any))
        {
            Debug.Log("Grip Button");
            //그립버튼을 누른 컨트롤러의 진동을 발생시킴
            //haptic.Execute(실행 시간, 지속 시간, 진동의 빈도, 진폭, 입력소스)
            haptic.Execute(0.2f, 0.2f, 50.0f, 0.5f, any);
        }

      //왼손 컨트롤러 트리거를 당겼을 경우
      if(trigger.GetStateDown(leftHand))
        {
            if(onItemL)
            {
                switch(handItemStateLeft)
                {
                    case HandItemState.None:
                        {
                            onItemL.SendMessage("HandOnItemLeft");
                            break;
                        }
                    case HandItemState.ItemHave:
                        {
                            onItemL.SendMessage("ItemInteractionOn");
                            break;
                        }
                }
            }
        }

        if (trigger.GetStateDown(rightHand))
        {
            if (onItemR)
            {
                onItemR.SendMessage("HandOnItemRight");
            }
        }

        if (buttonX.GetStateDown(SteamVR_Input_Sources.LeftHand))
        {
            Debug.Log("X버튼");
        }

        if (buttonY.GetStateDown(SteamVR_Input_Sources.LeftHand))
        {
            Debug.Log("Y버튼");
        }
    }

    public void OnItemUpdateLeft(GameObject item)
    {
        onItemL = item;
    }
    public void OnItemUpdateRight(GameObject item)
    {
        onItemR = item;
    }

    public void PlayerHandOnLeft(GameObject item)
    {
        handL = item.transform.parent;
        item.transform.position = handL.position;
        item.transform.rotation = handL.rotation;
        handItemStateLeft = HandItemState.ItemHave;
    }

    public void PlayerHandOnRight(GameObject item)
    {
        handR = item.transform.parent;
        item.transform.position = handR.position;
        item.transform.rotation = handR.rotation;
        handItemStateRight = HandItemState.ItemHave;
    }
}
