using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DancingBehaviour : StateMachineBehaviour ///// hep ayni ilk animasyondan baslatiyor, darbe sonucu geri tepme, olum, daha fazla animasyon, daha duzgun ortam
{
    public AnimationClip[] dancingAnimationClip;
    protected AnimatorOverrideController animatorOverrideController;
    private int dancingAnimationIndex = 0;
    protected AnimationClipOverrides clipOverrides;

    float dancingSpeed;
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = animatorOverrideController;

        clipOverrides = new AnimationClipOverrides(animatorOverrideController.overridesCount);
        animatorOverrideController.GetOverrides(clipOverrides);

        dancingAnimationIndex = (dancingAnimationIndex + 1) % dancingAnimationClip.Length;
        clipOverrides["Sword And Shield Strafe"] = dancingAnimationClip[dancingAnimationIndex];
        animatorOverrideController.ApplyOverrides(clipOverrides);

        switch (dancingAnimationIndex)
        {
            case 0:
                dancingSpeed = .85f;
                break;
            case 1:
                dancingSpeed = .85f;
                break;
            case 2:
                dancingSpeed = .85f;
                break;
            case 3:
                dancingSpeed = .85f;
                break;
        }
        ///dancingSpeed = 1f;
        ///
        //Debug.Log(dancingAnimationIndex.ToString());

        animator.SetFloat("DanceSpeed", dancingSpeed);
    }
}
