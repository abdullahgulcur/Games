using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearmanController : MonoBehaviour {

    public GameObject characterDistance;

    public float blowSpeed;
    float speedBoost = 0;

    public GameObject player;

    public GameObject moneyPocket;

    public GameObject body;
    public GameObject head;

    Rigidbody2D rb;
    Animator anim;

    float speed;
    float distance; // saldirma mesafesi

    bool isAttacked;
    int attackState;

    float timer;
    int time;

    bool defensed;
    bool died;

    public AudioClip whip0;
    public AudioClip whip1;
    public AudioClip whip2;
    public AudioClip whip3;

    public AudioClip fall0;

    EnemySkill es;
    SpearmanArmour ea;
    SpearmanHealth eh;

    bool foundPlayer = false;

    private bool weaponActive = false;

    bool moneyInstantiated;

    MovementAudioSource mas;
    HitAudioSource has;
    SwingAudioSource sas;

    GameObject spear;
        
    int dir;

    bool checkAllDied = true;

    void Start()
    {
        mas = GetComponent<MovementAudioSource>();
        sas = GetComponent<SwingAudioSource>();
        has = GetComponent<HitAudioSource>();
        died = false;

        eh = this.GetComponent<SpearmanHealth>();
        ea = this.GetComponent<SpearmanArmour>();
        es = this.GetComponent<EnemySkill>();

        StartCoroutine(SetSpeedBoost());

        StartCoroutine(SetAttackDistance());

        StartCoroutine(SetPersonSize());

        defensed = false;
        isAttacked = false;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        StartCoroutine(SetSpear());
    }

    IEnumerator SetSpear()
    {
        yield return new WaitForSeconds(0.001f);
        spear = transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(GetComponent<SpearmanArmour>().GetSpear()).gameObject;
    }

    IEnumerator SetPersonSize()
    {
        yield return new WaitForSeconds(0.001f);

        for (int i = 0; i < es.GetEnemyPower(); i++)
            this.transform.localScale += new Vector3(0.0005f, 0.0005f, 0f);
    }

    public void SetPlayer(GameObject p)
    {
        player = p;
    }
    
    IEnumerator SetAttackDistance() // belki biraz daha gelistirilebilir
    {
        yield return new WaitForSeconds(0.001f);

        int category = ea.GetWeaponCategory();

        distance = 5f + ((float)ea.GetSpear() / 12);
        distance *= 1.1f;
    }
    
    IEnumerator SetSpeedBoost()
    {
        yield return new WaitForSeconds(0.001f);
        speedBoost = es.GetEnemyAgility() / 10f + 4f;
        //speed *= speedBoost;
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

            if (dir == 1)
                speed *= speedBoost;
            else
                speed *= speedBoost * 1.5f;

            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Shock1"))
                rb.velocity = new Vector2(Utility.EnemyDirection(this.transform, player.transform) * blowSpeed, rb.velocity.y);
            else if (speed != 0)
                MoveHorizontal(speed);
            else
                StopMoving();
            

        }
        else
        {
            if (checkAllDied)
            {
                GameController.Instance.totalEnemy--;

                if (Utility.CheckAllDied())
                {
                    if (!GameController.Instance.gameFinished)
                        if (GameController.Instance.current_level == GameController.Instance.player_level && !GameController.Instance.isInCastle)
                        {
                            GameController.Instance.levelPassed = true;
                            GameController.Instance.player_level++;
                        }

                    if (!GameController.Instance.isInCastle)
                        player.transform.parent.GetComponent<PlayerMobileCtrl>().FadeinInTime(5f);

                    GameController.Instance.player_total_money += GameController.Instance.tempMoney;
                    GameController.Instance.tempMoney = 0;
                }

                checkAllDied = false;
            }

            Destroy(GetComponent<Rigidbody2D>());
            anim.SetInteger("State", 10);
            anim.SetInteger("HeadState", 3);
            RemoveCollidersRecursively();

            if (!moneyInstantiated)
                StartCoroutine(MoneyPocket());


            if ((transform.position - player.transform.position).magnitude > 30f)
            {
                Destroy(gameObject);
            }
        }

    }

    IEnumerator MoneyPocket()
    {
        moneyInstantiated = true;
        yield return new WaitForSeconds(0.5f);
        GameObject money = Instantiate(moneyPocket) as GameObject;
        money.transform.parent = this.gameObject.transform;
        money.transform.localPosition = new Vector2(-13f, -7f);
        money.transform.parent = null;
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
                }
                else if (speed == 0)
                {
                    if (Mathf.Abs(Utility.GetDistanceBetween(this.transform, player.transform)) > distance &&
                        Mathf.Abs(Utility.GetDistanceBetween(this.transform, player.transform)) < distance * 1.25)
                    {
                        if (time % 2 == 0)
                        {
                            anim.SetInteger("State", (int)(Random.Range(3, 5)));
                        }
                        else
                        {
                            anim.SetInteger("State", 0);
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

    void MoveHorizontal(float speed)
    {
        rb.velocity = new Vector2(speed, rb.velocity.y);



        if (speed < 0)
        {
            if(dir == 1) // adama dogru yuruyosa
            {
                transform.rotation = Quaternion.AngleAxis(180, Vector3.up); 
                anim.SetFloat("Speed", 1);
            }
            else
            {
                transform.rotation = Quaternion.AngleAxis(0, Vector3.up);
                anim.SetFloat("Speed", -1.25f);
            }
                
        }
        else if (speed > 0)
        {
            if (dir == 1) // adama dogru yuruyosa
            {
                transform.rotation = Quaternion.AngleAxis(0, Vector3.up);
                anim.SetFloat("Speed", 1);
            }
            else
            {
                transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
                anim.SetFloat("Speed", -1.25f);
            }
        }
            
                
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

    public void WhipSFX()
    {
        sas.RandomizeSfxWhip(whip0, whip1, whip2, whip3);
    }

    void SetDirection()
    {
        if (CanMove() == 0 && Utility.AtSameYPosition(player.transform, transform))
            speed = Utility.EnemyDirection(player.transform, this.transform);
        else if(CanMove() == 2)
            speed = 0;
        else if(Utility.AtSameYPosition(player.transform, transform))
        {
            speed = -Utility.EnemyDirection(player.transform, this.transform);
        }
    }

    public void FallSoundFX()
    {
        mas.PlaySingle(fall0);
    }

    int CanMove()
    {
        if (Mathf.Abs(Utility.GetDistanceBetween(this.transform, player.transform)) < 20 // adama dogru yurur
            && Mathf.Abs(Utility.GetDistanceBetween(this.transform, player.transform)) > distance * 1.25 && time % 2 == 0 &&
            !(anim.GetCurrentAnimatorStateInfo(0).IsName("QuickAttack1") || anim.GetCurrentAnimatorStateInfo(0).IsName("StrongAttack1")))
        {
            dir = 1;
            return 0;
        }

        else if (Mathf.Abs(Utility.GetDistanceBetween(this.transform, player.transform)) < distance// geri yurur
            &&
            time % 2 == 0 &&
            !(anim.GetCurrentAnimatorStateInfo(0).IsName("QuickAttack1") || anim.GetCurrentAnimatorStateInfo(0).IsName("StrongAttack1")))
        {
            dir  = -1;
            return 1;
        }
        else // durur
            return 2;

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Cliff"))
        {
            GameController.Instance.totalEnemy--;
            StartCoroutine(CallDestroyEnemy(3f));
        }

        if (other.gameObject.CompareTag("Door"))
        {
            other.transform.GetChild(0).GetComponent<CastleInnerDoor>().OpenDoor();
        }

        if (other.gameObject.CompareTag("LockedDoor"))
        {
            if (GameController.Instance.foundKey)
                other.transform.GetChild(0).GetComponent<CastleLockedDoor>().OpenDoor();
        }

        if (other.gameObject.CompareTag("LockedDoor1"))
        {
            if (!other.gameObject.transform.GetChild(1).gameObject.active)
                other.transform.GetChild(0).GetComponent<CastleLockedDoor>().OpenDoor();
        }


    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Door"))
        {
            other.transform.GetChild(0).GetComponent<CastleInnerDoor>().CloseDoor();
        }

        if (other.gameObject.CompareTag("LockedDoor"))
        {
            if (GameController.Instance.foundKey)
                other.transform.GetChild(0).GetComponent<CastleLockedDoor>().CloseDoor();
        }

        if (other.gameObject.CompareTag("LockedDoor1"))
        {
            if (!other.gameObject.transform.GetChild(1).gameObject.active)
                other.transform.GetChild(0).GetComponent<CastleLockedDoor>().CloseDoor();
        }
    }

    IEnumerator CallDestroyEnemy(float duration)
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }

    public void SetWeaponActive()
    {
        spear.GetComponent<Collider2D>().enabled = true;
    }

    public void SetWeaponInactive()
    {
        spear.GetComponent<Collider2D>().enabled = false;
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

    public void SetIsDead(bool b)
    {
        died = b;
    }

    public Animator GetAnim()
    {
        return anim;
    }

    public bool GetWeaponActive()
    {
        return weaponActive;
    }

    private void RemoveCollidersRecursively()
    {
        var allColliders = GetComponentsInChildren<Collider2D>();

        foreach (var childCollider in allColliders) Destroy(childCollider);
    }

}
