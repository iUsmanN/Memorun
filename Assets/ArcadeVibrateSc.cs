using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ArcadeVibrateSc : MonoBehaviour
{

    public float Value = 0.09f;
    public float Time = 5f;

    public IEnumerator vibrate()
    {
        while (true)
        {
            transform.DOPunchPosition(new Vector3(0f, 0f, Value), Time, 1, 1);
            yield return new WaitForSeconds(Time);
        }
    }
}
