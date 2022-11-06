using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class People : MonoBehaviour
{
    // Start is called before the first frame update

    public Animator peopleAnim;

    public bool Running;

    public GameObject Event;

    public Transform exitPosition;

    private NavMeshAgent navMeshAgent;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        peopleUpdate();
        if (Running)
        {
            navMeshAgent.SetDestination(exitPosition.position);
        }
    }
    public void peopleUpdate()
    {
        Running = Event.GetComponent<warnning>().Eventwarnning;
        peopleAnim.SetBool("Running", Running);
    }
}
