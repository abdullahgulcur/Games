using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PocketMoney : MonoBehaviour {
    
	void Start () {

        StartCoroutine(IdleState());
	}
	
	IEnumerator IdleState()
    {
        yield return new WaitForSeconds(0.66f);
        GetComponent<Animator>().SetInteger("State", 1);
    }

}
