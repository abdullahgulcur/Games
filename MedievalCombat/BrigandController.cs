using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrigandController : MonoBehaviour
{
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
    float distance;

    bool isAttacked;
    int attackState;

    float timer;
    int time;

    bool defensed;
    bool died;

    EnemySkill es;
    BrigandArmour ea;
    BrigandHealth eh;

    public AudioClip fall0;

    bool foundPlayer = false;

    HitAudioSource has;
    SwingAudioSource sas;
    MovementAudioSource mas;

    public AudioClip whip0;
    public AudioClip whip1;
    public AudioClip whip2;
    public AudioClip whip3;

    private bool weaponActive = false;

    bool moneyInstantiated;

    bool checkAllDied = true;

    void Start()
    {
        mas = GetComponent<MovementAudioSource>();
        has = GetComponent<HitAudioSource>();
        sas = GetComponent<SwingAudioSource>();

        died = false;

        eh = this.GetComponent<BrigandHealth>();
        ea = this.GetComponent<BrigandArmour>();
        es = this.GetComponent<EnemySkill>();

        StartCoroutine(SetSpeedBoost());

        StartCoroutine(SetAttackDistance());

        StartCoroutine(SetPersonSize());

        defensed = false;
        isAttacked = false;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        // InvokeRepeating("FindPlayer", 0, 1);
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

        switch (category)
        {
            case 0:
                distance = 5f + ((float)ea.GetSword() / 12) * 1f;
                break;
            case 1:
                distance = 5f + ((float)ea.GetAxe() / 8) * 0.75f;
                break;
            case 2:
                distance = 5f + ((float)ea.GetMace() / 8) * 0.6f;
                break;

        }

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

    IEnumerator SetSpeedBoost()
    {
        yield return new WaitForSeconds(0.001f);
        speedBoost = es.GetEnemyAgility() / 10f + 4f;
    }

    void FixedUpdate()
    {

        if (GameController.Instance.playerDead)
        {
            anim.SetInteger("State", 0);
            anim.SetInteger("HeadState", 0);

            if(rb != null)
                StopMoving();
        }
        else

        if (!died)
        {
            SetDirection();
            SetTimer();
            SetAnimation();
            HeadAnimations();

            speed *= speedBoost;

            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Shock1"))
                rb.velocity = new Vector2(Utility.EnemyDirection(this.transform, player.transform) * blowSpeed, rb.velocity.y);
            else if (speed != 0)
                MoveHorizontal(speed);
            else
                StopMoving();

            if ((anim.GetCurrentAnimatorStateInfo(0).IsName("QuickAttack1") || anim.GetCurrentAnimatorStateInfo(0).IsName("StrongAttack1"))
                && Mathf.Abs(Utility.GetDistanceBetween(this.transform, player.transform)) > distance - 2f)
            {
                this.rb.velocity = new Vector2(Utility.EnemyDirection(player.transform, this.transform) * 2f, rb.velocity.y);
            }


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
            RemoveCollidersRecursively();
            anim.SetInteger("HeadState", 3);

            if (!moneyInstantiated)
                StartCoroutine(MoneyPocket());

            if ((transform.position-player.transform.position).magnitude > 30f)
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
                }
                else if (speed == 0)
                {
                    if (Mathf.Abs(Utility.GetDistanceBetween(this.transform, player.transform)) < distance)
                    {
                        if (time % 2 == 0)
                        {
                            if (Utility.GetRandomNumber(0, 2) == 0)
                                anim.SetInteger("State", 3);
                            else
                                anim.SetInteger("State", 4);
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

    IEnumerator MoneyPocket()
    {
        moneyInstantiated = true;
        yield return new WaitForSeconds(0.5f);
        GameObject money = Instantiate(moneyPocket) as GameObject;
        money.transform.parent = this.gameObject.transform;
        money.transform.localPosition = new Vector2(-13f, -7f);
        money.transform.parent = null;
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

    public void FallSoundFX()
    {
        mas.PlaySingle(fall0);
    }

    void StopMoving()
    {
        rb.velocity = new Vector2(0, rb.velocity.y);
    }

    void SetDirection()
    {
        if (CanMove() && Utility.AtSameYPosition(player.transform, transform))
            speed = Utility.EnemyDirection(player.transform, this.transform);
        else
            speed = 0;
    }

    public void SetWeaponActive()
    {
        ea.GetWeapon().GetComponent<Collider2D>().enabled = true;
    }

    public void SetWeaponInactive()
    {
        ea.GetWeapon().GetComponent<Collider2D>().enabled = false;
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

    private void RemoveCollidersRecursively()
    {
        var allColliders = GetComponentsInChildren<Collider2D>();

        foreach (var childCollider in allColliders) Destroy(childCollider);
    }

}