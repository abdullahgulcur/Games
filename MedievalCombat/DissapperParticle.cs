using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissapperParticle : MonoBehaviour {

    private SpriteRenderer sr;

	void Start () {

        sr = this.GetComponent<SpriteRenderer>();
        
        StartCoroutine(CallDecrease());
    }

    IEnumerator CallDecrease()
    {
        yield return new WaitForSeconds(20f);

        InvokeRepeating("DecreaseFade", 0, 0.1f); // InvokeRepeating("functionName", startTime, inEverySeconds);
    }

    void DecreaseFade()
    {
        sr.color = new Color(sr.color.r, sr.color.b, sr.color.g, sr.color.a - 0.01f);

        if (sr.color.a <= 0f)
            Destroy(this.gameObject);
    }
}
