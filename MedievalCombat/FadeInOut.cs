using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FadeInOut : MonoBehaviour {

    public int speed;

    public GameObject cube;

    bool b = false;

	void Start () {

        AddFade(cube);
        RemoveFade(cube);
    }

    void AddFade(GameObject gameObj)
    {
        StartCoroutine(FadeInOutFunction(1f, -1, cube));
    }

    void RemoveFade(GameObject gameObj)
    {
        StartCoroutine(FadeInOutFunction(1f, -1, cube));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            b = true;
        }
    }

    IEnumerator FadeInOutFunction(float val, int dir, GameObject obj)
    {
        yield return new WaitForSeconds(0.1f / speed);

        if (b)
            Debug.Log("xxxxxxxxxxxxxxxx");
        
        if (val < 0)
            dir *= -1;
        else if (val > 1)
            dir *= -1;
            
        if(dir == -1)
        {
            Color color = new Color(1, val - 0.01f, val - 0.01f);
            obj.GetComponent<SpriteRenderer>().color = color;
            StartCoroutine(FadeInOutFunction(val - 0.01f, dir, obj));
        }
        else if(dir == 1)
        {
            Color color = new Color(1, val + 0.01f, val + 0.01f);
            obj.GetComponent<SpriteRenderer>().color = color;
            StartCoroutine(FadeInOutFunction(val + 0.01f, dir,obj));
        }
    }

}
