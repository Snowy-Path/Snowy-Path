using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{

    [Min(0)]
    public float attackRange = 1.0f; //Range from animation ???

    [Min(0)]
    public int attackDamage = 1;

    [Min(0)]
    public float attackRecovery = 0.2f;
    private float attackRecoveryTimer = 0f;

    [Min(0)]
    public float castAttack = 0.4f;

    public Animator animator;

}
