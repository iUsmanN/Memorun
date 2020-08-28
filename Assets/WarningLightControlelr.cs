using System.Collections;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class WarningLightControlelr : MonoBehaviour
{

    Light A;

    // Use this for initialization
    void Start()
    {
        A = GetComponent<Light>();
        A.intensity = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
           // Blink();
    }

    public void Blink()
    {
        if(A)
        StartCoroutine(BlinkCR());
    }

    IEnumerator BlinkCR()
    {
        A.DOIntensity(7f, 0.3f);
        yield return new WaitForSeconds(0.3f);

        A.DOIntensity(0f, 0.3f);
        yield return new WaitForSeconds(0.3f);

        A.DOIntensity(8f, 0.15f);
        yield return new WaitForSeconds(0.2f);

        A.DOIntensity(0f, 0.25f);
        //yield return new WaitForSeconds(0.1f);
    }
}
