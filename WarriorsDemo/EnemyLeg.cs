using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class EnemyLeg : MonoBehaviour
{
    public GameObject enemy;
    PaladinController pc;

    Invector.CharacterController.vThirdPersonController playerCtrl;

    void Start()
    {
        pc = enemy.GetComponent<PaladinController>();
    }

    void OnTriggerEnter(Collider other)
    {
        // for paladin
        if (other.gameObject.CompareTag("Player"))
        {
            if (pc.isKicking)
            {
                playerCtrl = other.transform.GetComponent<Invector.CharacterController.vThirdPersonController>();

                if (!playerCtrl.isAttacked)
                {
                    CameraShaker.Instance.ShakeOnce(3f, 4f, 0.1f, 1f);
                    playerCtrl.isAttacked = true;
                    playerCtrl.isBlocking = false;
                    playerCtrl.animator.SetBool("IsBlocking", false);
                    StartCoroutine(SetFixedForPlayer());
                }

            }

        }
    }

    IEnumerator SetFixedForPlayer()
    {
        yield return new WaitForSeconds(0.35f); // lenght of animation
        playerCtrl.isAttacked = false;
    }
}
