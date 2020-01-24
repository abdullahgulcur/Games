using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class ArrowCtrl : MonoBehaviour {

    //transform.up = Vector3.Lerp(transform.up, -GetComponent<Rigidbody2D>().velocity, Time.deltaTime * 10);

    Rigidbody2D rb2d;

    public int ember; // 0 bos, 1 embered

    public AudioClip bodyhit0;
    public AudioClip bodyhit1;
    public AudioClip bodyhit2;

    public ParticleSystem shieldParticles;
    public ParticleSystem hotParticles;

    public GameObject contactPoint;
    public GameObject stabbedArrow;
    public GameObject shieldArrow;

    public AudioClip shieldhit_iron_0;
    public AudioClip shieldhit_iron_1;
    public AudioClip shieldhit_iron_2;

    public AudioClip shieldhit_wood_0;
    public AudioClip shieldhit_wood_1;
    public AudioClip shieldhit_wood_2;

    Player playerScript;
    PlayerHealth ph;

    public ParticleSystem blood_0;

    HitAudioSource has;

    bool damaged;

    private int damage;

    // there is no need to use setfixed method because arrow prefab instances is used only once. 

    void Start () {

        has = this.GetComponent<HitAudioSource>();
        damaged = false;
        rb2d = GetComponent<Rigidbody2D>();
    }
	
	void Update () {

        transform.up = Vector3.Lerp(transform.up, -GetComponent<Rigidbody2D>().velocity, Time.deltaTime * 10);
    }

    void BloodEffect(Transform pos)
    {
        Instantiate(blood_0, contactPoint.transform.position, Quaternion.identity);
    }

    void ShieldParticles(Transform pos)
    {
        Instantiate(shieldParticles, pos.position, Quaternion.identity);
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if ((other.gameObject.CompareTag("Destroy")))
        {
            Destroy(this.gameObject);
        }

        if (!damaged)
        {
            if ((other.gameObject.CompareTag("AchillesBody")))
            {
                
                
                playerScript = other.transform.parent.GetComponent<Player>();
                ph = other.transform.parent.GetComponent<PlayerHealth>();

                if (!playerScript.GetAnim().GetCurrentAnimatorStateInfo(1).IsName("Defense"))
                {
                    Quaternion eulerDegree = Quaternion.Euler(0, 0, 0);
                    if (rb2d.velocity.x > 1f)
                        eulerDegree = Quaternion.Euler(0, 180, 0);
                    else if(rb2d.velocity.x < -1f)
                        eulerDegree = Quaternion.Euler(0, 0, 0);

                    GameObject sa = Instantiate(stabbedArrow, contactPoint.transform.position, eulerDegree) as GameObject;
                    sa.transform.parent = other.transform;

                    Destroy(transform.GetChild(0).gameObject);
                    Destroy(transform.GetChild(1).gameObject);

                    CameraShaker.Instance.ShakeOnce(3f, 4f, 0.2f, 0.5f);

                    playerScript.SetIsAttacked(true);
                    playerScript.BackDirection(Utility.EnemyDirection(other.transform.parent.transform, this.transform));
                    if (ph.GetPlayerHealth() > 0)
                    {
                        if (ember == 1)
                            HotParticles(contactPoint.transform);

                        has.RandomizeSfxArrowHitBody(bodyhit0, bodyhit1, bodyhit2);
                        ph.ReceiveDamage(damage);//GetArrowDamageAmount(int index)
                        BloodEffect(this.transform);
                    }



                }
                damaged = true;
                StartCoroutine(SetShockAnimation());

            }
            
            if (other.gameObject.CompareTag("AchillesShield"))
            {

                playerScript = other.transform.parent.parent.parent.parent.parent.GetComponent<Player>();
                ph = other.transform.parent.parent.parent.parent.parent.GetComponent<PlayerHealth>();

                if (playerScript.GetAnim().GetCurrentAnimatorStateInfo(1).IsName("Defense"))
                {
                    Quaternion eulerDegree = Quaternion.Euler(0, 0, 0);
                    if (rb2d.velocity.x > 1f)
                        eulerDegree = Quaternion.Euler(0, 180, 0);
                    else if (rb2d.velocity.x < -1f)
                        eulerDegree = Quaternion.Euler(0, 0, 0);

                    Destroy(transform.GetChild(0).gameObject);
                    Destroy(transform.GetChild(1).gameObject);

                    GameObject sa = Instantiate(shieldArrow, contactPoint.transform.position, eulerDegree) as GameObject;
                    sa.transform.parent = other.transform;

                    if (ph.GetPlayerShieldHealth() > 0)
                    {
                        ph.ReceiveShieldDamage(damage / 3);

                        if (ember == 1)
                            HotParticles(contactPoint.transform);

                        if (other.transform.parent.parent.parent.parent.parent.GetComponent<PlayerState>().GetShield() > 2)
                            has.RandomizeSfxArrowHitShieldIron(shieldhit_iron_0, shieldhit_iron_1, shieldhit_iron_2);
                        else
                            has.RandomizeSfxShieldWood(shieldhit_wood_0, shieldhit_wood_1, shieldhit_wood_2);
                    }
                    else if (ph.GetPlayerShieldHealth() <= 0)
                    {
                        ShieldParticles(other.transform);
                        other.gameObject.SetActive(false);
                    }

                }
                damaged = true;

            }
            
        }
        
    }

    void HotParticles(Transform pos)
    {
        Instantiate(hotParticles, pos.position, Quaternion.identity);
    }

    public void SetDamageAmount(int x)
    {
        damage = x;
    }

    IEnumerator SetShockAnimation()
    {
        yield return new WaitForSeconds(0.3f);
        playerScript.SetIsAttacked(false);
    }

}
