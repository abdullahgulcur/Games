using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ButtonShine : MonoBehaviour {

    public Transform shine;

    public float offset;
    public float duration;
    public float minDelay;
    public float maxDelay;

    void Start () {
        Animate();
	}
	
    public void Animate()
    {
        shine.DOLocalMoveX(offset, duration).SetEase(Ease.Linear).SetDelay(Random.Range(minDelay, maxDelay)).OnComplete(() => {

            shine.DOLocalMoveX(-offset, 0);
            Animate();
        });
    }
}
