using UnityEngine;
using System.Collections;
using System;

namespace Invector.CharacterController
{
    public class vThirdPersonController : vThirdPersonAnimator
    {
        protected virtual void Start()
        {
#if !UNITY_EDITOR
                Cursor.visible = false;
#endif
        }

        public virtual void Sprint(bool value)
        {                                   
            isSprinting = value;            
        }

        public virtual void Strafe()
        {
            if (locomotionType == LocomotionType.OnlyFree) return;
            isStrafing = !isStrafing;
        }

        public virtual void Jump()
        {
            // conditions to do this action
            bool jumpConditions = isGrounded && !isJumping;
            // return if jumpCondigions is false
            if (!jumpConditions) return;
            // trigger jump behaviour
            jumpCounter = jumpTimer;            
            isJumping = true;
            // trigger jump animations            
            if (_rigidbody.velocity.magnitude < 1)
                animator.CrossFadeInFixedTime("Jump", 0.1f);
            else
                animator.CrossFadeInFixedTime("JumpMove", 0.2f);
        }

        public virtual void Attack()
        {
            //if(is)

            // if moving
            if (input.x == 1 || input.y == 1)
            {
                switch(Utility.GetRandomNumber(0, 1)){
                    case 0:
                        animator.CrossFadeInFixedTime("Attack3", 0.1f);

                        break;
                    case 1:
                        animator.CrossFadeInFixedTime("Attack4", 0.1f);

                        break;

                }

            }
            // if not walking
            else
            {
                switch (Utility.GetRandomNumber(0, 2))
                {
                    case 0:
                        animator.CrossFadeInFixedTime("Attack0", 0.1f);
                        break;

                    case 1:
                        animator.CrossFadeInFixedTime("Attack1", 0.1f);

                        break;
                    case 2:
                        animator.CrossFadeInFixedTime("Attack2", 0.1f);

                        break;

                }

            }

            //isAttacking = true;

        }


        public void Block()
        {
            isBlocking = true;
            animator.CrossFadeInFixedTime("Block", 0.1f);
        }

        public virtual void RotateWithAnotherTransform(Transform referenceTransform)
        {
            var newRotation = new Vector3(transform.eulerAngles.x, referenceTransform.eulerAngles.y, transform.eulerAngles.z);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(newRotation), strafeRotationSpeed * Time.fixedDeltaTime);
            targetRotation = transform.rotation;
        }

        public bool IsBlocking()
        {
            return isBlocking;
        }

    }
}