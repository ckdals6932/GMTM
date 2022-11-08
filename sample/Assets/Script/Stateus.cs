using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stateus : MonoBehaviour
{
    [Header("Walk, Run Speed")]
    [SerializeField]
    private float runSpeed;
    [SerializeField]
    private float walkSpeed;

    public float WalkSpeed => walkSpeed;
    public float RunSpeed => runSpeed;
}
