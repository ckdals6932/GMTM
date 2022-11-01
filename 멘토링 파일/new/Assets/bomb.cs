using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class bomb : MonoBehaviour
{
    public GameObject exp;
    public float expForce, radius;
    public int explodeCount = 10;
    public float minMagnitudeToExplode = 1f;
    private Interactable interactable;
    

    private void Start()
    {
        interactable = this.GetComponent<Interactable>();
    }
    private void OnCollisionEnter(Collision other)
    {
        if(interactable != null && interactable.attachedToHand != null )
            return;
        
        if (other.impulse.magnitude > minMagnitudeToExplode)
            {
                for (int explodeIndex = 0; explodeIndex < explodeCount; explodeIndex++)
                {
                    GameObject explodePart = (GameObject)GameObject.Instantiate(exp, this.transform.position, this.transform.rotation);
                    explodePart.GetComponentInChildren<MeshRenderer>().material.SetColor("_TintColor", Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f));
                }

                Destroy(this.gameObject);
            }
        GameObject _exp = Instantiate(exp, transform.position, transform.rotation);
        Destroy(_exp, 3);
        //Destroy(gameObject);
    }

    void knockBack()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider neardyby in colliders)
        {
            Rigidbody rig = neardyby.GetComponent<Rigidbody>();
            if(rig != null)
            {
                rig.AddExplosionForce(expForce, transform.position, radius);
            }
        }
    }
}
