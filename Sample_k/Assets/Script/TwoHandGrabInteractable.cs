using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TwoHandGrabInteractable : XRGrabInteractable
{
    public List<XRSimpleInteractable> secondHandGrabpoints = new List<XRSimpleInteractable>();
    private IXRSelectInteractor secondInteractor;
    private Quaternion attachIntialRoation;
    public enum TwoHandRotationType { None, First, Second }
    public TwoHandRotationType twoHandRotationType;
    public bool SnapToSecondHand = true;
    private Quaternion intialRotationOffset;

    void Start()
    {
        foreach (var item in secondHandGrabpoints)
        {
            item.selectEntered.AddListener(OnSecondHandGrab);
            item.selectExited.AddListener(OnSecondHandRelease);
        }
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        if (secondInteractor != null && firstInteractorSelecting != null)
        {
            if (SnapToSecondHand)
            {
                firstInteractorSelecting.transform.rotation = GetTwoHandRotation();
            }

            else
            {
                firstInteractorSelecting.transform.rotation = GetTwoHandRotation() * intialRotationOffset;
            }
        }
        base.ProcessInteractable(updatePhase);
    }

    private Quaternion GetTwoHandRotation()
    {
        Transform attachTransform1 = firstInteractorSelecting.transform;
        Transform attachTransform2 = secondInteractor.transform;

        switch (twoHandRotationType)
        {
            case TwoHandRotationType.None:
                return Quaternion.LookRotation(attachTransform2.position - attachTransform1.position);

            case TwoHandRotationType.First:
                return Quaternion.LookRotation(attachTransform2.position - attachTransform1.position, firstInteractorSelecting.transform.up);

            case TwoHandRotationType.Second:
                return Quaternion.LookRotation(attachTransform2.position - attachTransform1.position, secondInteractor.transform.up);

            default:
                return Quaternion.LookRotation(attachTransform2.position - attachTransform1.position, secondInteractor.transform.up);
        }

    }

    public void OnSecondHandGrab(SelectEnterEventArgs args)
    {
        secondInteractor = args.interactorObject;
        intialRotationOffset = Quaternion.Inverse(GetTwoHandRotation()) * secondInteractor.transform.rotation;
    }

    public void OnSecondHandRelease(SelectExitEventArgs args)
    {

        secondInteractor = null;
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {

        attachIntialRoation = args.interactorObject.transform.localRotation;
        base.OnSelectEntered(args);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        secondInteractor = null;
        args.interactorObject.transform.localRotation = attachIntialRoation;
        base.OnSelectExited(args);
    }

    public override bool IsSelectableBy(IXRSelectInteractor interactor)
    {
        bool isalreadygrabbed = firstInteractorSelecting != null && !interactor.Equals(firstInteractorSelecting);
        return base.IsSelectableBy(interactor) && !isalreadygrabbed;
    }
}
