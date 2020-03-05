using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnimation : StateMachineBehaviour
{
    public AnimationClip[] attackAnimationClip;
    protected AnimatorOverrideController animatorOverrideController;
    private int attackAnimationIndex = 0;
    protected AnimationClipOverrides clipOverrides;

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = animatorOverrideController;

        clipOverrides = new AnimationClipOverrides(animatorOverrideController.overridesCount);
        animatorOverrideController.GetOverrides(clipOverrides);

        attackAnimationIndex = (attackAnimationIndex + 1) % attackAnimationClip.Length;
        clipOverrides["Sword And Shield Attack"] = attackAnimationClip[attackAnimationIndex];
        animatorOverrideController.ApplyOverrides(clipOverrides);

        ////Debug.Log("Attack index: " + attackAnimationIndex.ToString());

        //animator.GetComponentInParent<Invector.CharacterController.vThirdPersonMotor>()._rigidbody.AddForce(animator.GetComponentInParent<Transform>().up * 10f, ForceMode.Impulse);

        //_rigidbody.AddForce(transform.up * 10f, ForceMode.Impulse);
    }
}
