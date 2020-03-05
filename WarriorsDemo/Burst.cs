using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burst : MonoBehaviour
{
    /*
     *  Particle Type
     *  
     *  Bood_0 : 0
     *  Blood_1 : 2
     *  Hot Particles : 1
     * 
     */

    public int particleType;
    private void Start()
    {
        switch (particleType)
        {
            case 0:
                StartCoroutine(DestroyBloodParticles());
                break;
            case 1:
                StartCoroutine(DestroyHotParticles());
                break;
            case 2:
                StartCoroutine(DestroyBlood_1_Particles());
                break;
        }

        
    }

    private IEnumerator DestroyBloodParticles()
    {
        yield return new WaitForSeconds(GetComponent<ParticleSystem>().duration);
        Destroy(gameObject);

    }

    private IEnumerator DestroyBlood_1_Particles()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);

    }

    private IEnumerator DestroyHotParticles()
    {
        float duration = GetComponent<Transform>().GetChild(3).GetComponent<ParticleSystem>().duration; // the last childs duration time is also the duration of particle system...
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }
}
