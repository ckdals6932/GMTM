using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class M4A1 : MonoBehaviour
{
    public GameObject bullet;
    public Transform barrel;
    public float speed = 40;
    public AudioSource audioSource;
    public AudioClip audioClip;

    public void Start()
    {
        XRGrabInteractable grabbable = GetComponent<XRGrabInteractable>();
        grabbable.activated.AddListener(FireBullet);
    }

    public void FireBullet(ActivateEventArgs args)
    {
        GameObject spawnedBullet = Instantiate(bullet);
        spawnedBullet.transform.position = barrel.position;
        spawnedBullet.GetComponent<Rigidbody>().velocity = barrel.forward * speed;
        audioSource.PlayOneShot(audioClip);
        Destroy(spawnedBullet, 5);
    }

}
