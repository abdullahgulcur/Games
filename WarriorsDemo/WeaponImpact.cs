using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponImpact : StateMachineBehaviour
{
    public GameObject person;
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        person.GetComponent<PaladinController>().isAttacked = false;
    }

}
