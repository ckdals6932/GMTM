using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerToTarger : MonoBehaviour
{
    public TextMeshProUGUI playerDistance;

    public Image image;

    public Transform player;

    public Transform objectUI;

    public float Dist;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void LookRotationToTarget()
    {
        //��ǥ ��ġ
        Vector3 to = new Vector3(player.position.x, 0, player.position.z);

        //�� ��ġ
        Vector3 from = new Vector3(transform.position.x, 0, transform.position.z);

        //�ٷ� ����
        transform.rotation = Quaternion.LookRotation(to - from);
    }
    private void DistToTarget()
    {
        Dist = Vector3.Distance(player.transform.position, transform.position);

        playerDistance.text = Mathf.Round(Dist) + "m";

    }

        // Update is called once per frame
        void Update()
    {
        DistToTarget();
        LookRotationToTarget();

    }
}
