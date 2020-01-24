using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastBossCtrl : MonoBehaviour {

    public GameObject characterDistance;

    public float blowSpeed;
    float speedBoost = 15;

    GameObject player;

    public GameObject body;
    public GameObject head;

    Rigidbody2D rb;
    Animator anim;

    float speed;
    public float distance = 4f;

    bool isAttacked = false;
    int attackState;

    float timer;
    int time;

    bool defensed = false;
    bool died = false;
    bool isAttacking;

    LastBossPower lbp;

    public GameObject weapon;
    public GameObject shield;

    public AudioClip fall0;

    public AudioClip whip0;
    public AudioClip whip1;
    public AudioClip whip2;
    public AudioClip whip3;

    bool foundPlayer = false;

    bool defenseStatus;

    bool moneyInstantiated;

    HitAudioSource has;
    SwingAudioSource sas;

    bool checkAllDied = true;

    void Start()
    {
        sas = GetComponent<SwingAudioSource>();
        has = GetComponent<HitAudioSource>();
        
        lbp = GetComponent<LastBossPower>();

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    public void SetPlayer(GameObject p)
    {
        player = p;
    }

    void HeadAnimations()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") || anim.GetCurrentAnimatorStateInfo(0).IsName("Walking1"))
        {
            anim.SetInteger("HeadState", 0);
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Shock1"))
        {
            anim.SetInteger("HeadState", 2);
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("QuickAttack1") || anim.GetCurrentAnimatorStateInfo(0).IsName("StrongAttack1"))
        {
            anim.SetInteger("HeadState", 1);
        }


    }

    void FixedUpdate()
    {

        if (GameController.Instance.playerDead)
        {
            anim.SetInteger("State", 0);
            anim.SetInteger("HeadState", 0);

            if (rb != null)
                StopMoving();
        }
        else

        if (!died)
        {
            SetDirection();
            SetTimer();
            SetAnimation();
            HeadAnimations();
            SetIsAttacking();

            speed *= speedBoost;

            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Shock1"))
                rb.velocity = new Vector2(Utility.EnemyDirection(this.transform, player.transform) * blowSpeed, rb.velocity.y);
            else if (speed != 0)
                MoveHorizontal(speed);
            else
                StopMoving();

            if (lbp.GetEnemyShieldHealth() <= 0)
            {
                anim.SetLayerWeight(anim.GetLayerIndex("Layer1"), 0f);
            }

            if ((anim.GetCurrentAnimatorStateInfo(0).IsName("QuickAttack1") || anim.GetCurrentAnimatorStateInfo(0).IsName("StrongAttack1"))
                && Mathf.Abs(Utility.GetDistanceBetween(this.transform, player.transform)) > distance - 2f)
            {
                this.rb.velocity = new Vector2(Utility.EnemyDirection(player.transform, this.transform) * 2f, rb.velocity.y);
            }

            if (Mathf.Abs(Utility.GetDistanceBetween(transform, player.transform)) < distance * 0.75f && !isAttacking)
            {
                rb.velocity = new Vector2(-Utility.EnemyDirection(player.transform, this.transform) * 10f, rb.velocity.y);
                anim.SetInteger("State", 1);
                anim.SetFloat("Speed", -1);
            }

        }
        else
        {
            if (checkAllDied)
            {
                if (GameController.Instance.atBossRoom)
                {
                    GameController.Instance.gameFinished = true;
                    GameController.Instance.lastScenarioDemonstrable = true;
                    player.transform.parent.GetComponent<PlayerMobileCtrl>().FadeinInTime(5f);
                    GameController.Instance.player_total_money += GameController.Instance.tempMoney;
                    //GameController.Instance.skillpoints_left += 12;
                    GameController.Instance.tempMoney = 0;
                }

                checkAllDied = false;
            }

            Destroy(GetComponent<Rigidbody2D>());
            RemoveCollidersRecursively();
            anim.SetInteger("State", 10);
            anim.SetInteger("HeadState", 3);

            if ((transform.position - player.transform.position).magnitude > 30f)
            {
                Destroy(gameObject);
            }
        }

    }

    void SetAnimation()
    {
        if (Utility.AtSameYPosition(player.transform, transform))
        {
            if (isAttacked)
            {
                if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Shock1"))
                    anim.SetInteger("State", 8);
            }
            else
            {
                if (speed != 0)
                {
                    anim.SetInteger("State", 1);
                    anim.SetFloat("Speed", 1);
                }
                else if (speed == 0)
                {
                    if (Mathf.Abs(Utility.GetDistanceBetween(this.transform, player.transform)) < distance && Mathf.Abs(Utility.GetDistanceBetween(transform, player.transform)) > distance * 0.75f)
                    {
                        if (time % 2 == 0)
                        {

                            if (Utility.GetRandomNumber(0, 2) == 0)
                                anim.SetInteger("State", 3);
                            else
                                anim.SetInteger("State", 4);

                            if (defensed)
                            {
                                anim.SetTrigger("DefenseReverse");
                                StartCoroutine(SetDefenseStatus(0));
                                defensed = false;
                            }
                        }
                        else
                        {
                            //anim.SetInteger("State", 0);

                            if (!defensed && lbp.GetEnemyShieldHealth() > 0)
                            {
                                anim.SetTrigger("Defense");
                                StartCoroutine(SetDefenseStatus(1));
                                defensed = true;
                            }
                        }
                    }
                    else
                    {
                        anim.SetInteger("State", 0);
                    }

                }

            }
        }
        else
        {
            anim.SetInteger("State", 0);
        }



    }

    void SetIsAttacking()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("QuickAttack1") || anim.GetCurrentAnimatorStateInfo(0).IsName("StrongAttack1"))
            isAttacking = true;
        else
            isAttacking = false;
    }

    public void SetWeaponActive()
    {
        weapon.GetComponent<Collider2D>().enabled = true;
    }

    public void SetWeaponInactive()
    {
        weapon.GetComponent<Collider2D>().enabled = false;
    }

    IEnumerator SetDefenseStatus(int x) // 1 de savunuyo 0 da bos 
    {
        if (x == 0)
        {
            yield return new WaitForSeconds(0.11f);
            shield.GetComponent<Collider2D>().enabled = false;
            defenseStatus = false;
        }
        else
        {
            yield return new WaitForSeconds(0.05f);
            shield.GetComponent<Collider2D>().enabled = true;
            defenseStatus = true;
        }

        yield return null;
    }

    void MoveHorizontal(float speed)
    {
        rb.velocity = new Vector2(speed, rb.velocity.y);

        if (speed < 0)
            transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
        else if (speed > 0)
            transform.rotation = Quaternion.AngleAxis(0, Vector3.up);
    }

    public void WhipSFX()
    {
        sas.RandomizeSfxWhip(whip0, whip1, whip2, whip3);
    }

    void StopMoving()
    {
        rb.velocity = new Vector2(0, rb.velocity.y);
    }

    void Attack()
    {
        if (Mathf.Abs(Utility.GetDistanceBetween(this.transform, player.transform)) < distance)
        {
            anim.SetInteger("State", 2);
        }
    }

    void SetDirection()
    {
        if (CanMove() && Utility.AtSameYPosition(player.transform, transform))
            speed = Utility.EnemyDirection(player.transform, this.transform);
        else
            speed = 0;
    }

    bool CanMove()
    {
        if (Mathf.Abs(Utility.GetDistanceBetween(this.transform, player.transform)) < 20
            && Mathf.Abs(Utility.GetDistanceBetween(this.transform, player.transform)) > distance && time % 2 == 0 &&
            !(anim.GetCurrentAnimatorStateInfo(0).IsName("QuickAttack1") || anim.GetCurrentAnimatorStateInfo(0).IsName("StrongAttack1")))
        {
            return true;
        }

        else
        {
            return false;
        }

    }

    public void FallSoundFX()
    {
        this.GetComponent<MovementAudioSource>().PlaySingle(fall0);
    }

    void SetTimer()
    {
        timer += Time.deltaTime;
        time = (int)timer;
    }

    public void SetIsAttacked(bool b)
    {
        isAttacked = b;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Cliff"))
        {
            GameController.Instance.totalEnemy--;
            StartCoroutine(CallDestroyEnemy(3f));
        }

        if (other.gameObject.CompareTag("Extinction"))
        {
            transform.SetPositionAndRotation(new Vector3(transform.position.x - 25f, transform.position.y, transform.position.z), Quaternion.identity);
        }

        if (other.gameObject.CompareTag("Extinction0"))
        {
            transform.SetPositionAndRotation(new Vector3(transform.position.x + 25f, transform.position.y, transform.position.z), Quaternion.identity);
        }
    }

    IEnumerator CallDestroyEnemy(float duration)
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }

    public void SetIsDead(bool b)
    {
        died = b;
    }

    public Animator GetAnim()
    {
        return anim;
    }

    public bool GetDefenseStatus()
    {
        return defenseStatus;
    }

    private void RemoveCollidersRecursively()
    {
        var allColliders = GetComponentsInChildren<Collider2D>();

        foreach (var childCollider in allColliders) Destroy(childCollider);
    }


}
