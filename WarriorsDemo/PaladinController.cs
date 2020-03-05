using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaladinController : MonoBehaviour
{
    public GameObject player;

    private Animator anim;
    private Rigidbody rb;

    public bool isAttacking;
    public bool isKicking;
    public bool isAttacked;


    private bool canMoveBackward;

    float timer;
    int time;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float dist = Vector3.Distance(player.transform.position, transform.position);

        

        if (isAttacked)
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Blow"))
                anim.SetInteger("State", 5);
        }
        else
        {

            transform.LookAt(player.transform);

            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                isAttacking = false;
            }
            else
                isAttacking = true;

            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Sword And Shield Kick"))
                isKicking = false;
            else
                isKicking = true;



            if (dist > 20f)
            {
                anim.SetInteger("State", 0);
            }
            else if (dist <= 20 && dist > 10f) // walk
            {
                anim.SetInteger("State", 2);
            }
            else if (dist <= 10 && dist > 5f)
            {
                anim.SetInteger("State", 1); // slow walk
                anim.SetFloat("DanceSpeed", 1.4f);
            }
            else
            {
                if (!canMoveBackward)
                {
                    if (dist <= 5 && dist > 1.2f)
                    {
                        if (!isAttacking)
                        {
                            //if (Utility.GetRandomNumber(0, 1) == 0)
                                anim.SetInteger("State", 3); // attack
                            //else
                                //anim.SetInteger("State", 10); // defense


                            //anim.SetInteger("State", 3); // attack
                        }
                    }
                    else
                    {
                        if(Utility.GetRandomNumber(0,1) == 0)
                            anim.SetInteger("State", 4); // kick
                        else
                            anim.SetInteger("State", 10); // defense
                    }
                }
                else
                {
                    anim.SetInteger("State", 6); // dance
                    anim.SetFloat("DanceSpeed", 1f);
                }

            }
        }

        //Debug.Log(canMoveBackward.ToString());

        SetCanMoveBackward();
        SetTimer();

        //Debug.Log(time.ToString());
    }

    void SetCanMoveBackward()
    {
        if (time % 10 == 0)
            canMoveBackward = true;

        if (time % 12 == 0)
        {
            canMoveBackward = false;
            timer = 1;
        }
    }

    void SetTimer()
    {
        timer += Time.deltaTime;
        time = (int)timer;
    }

    public Animator GetAnimator()
    {
        return anim;
    }

}

public class AnimationClipOverrides : List<KeyValuePair<AnimationClip, AnimationClip>>
{
    public AnimationClipOverrides(int capacity) : base(capacity) { }

    public AnimationClip this[string name]
    {
        get { return this.Find(x => x.Key.name.Equals(name)).Value; }
        set
        {
            int index = this.FindIndex(x => x.Key.name.Equals(name));
            if (index != -1)
                this[index] = new KeyValuePair<AnimationClip, AnimationClip>(this[index].Key, value);
        }
    }
}