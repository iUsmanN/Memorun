using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class FadeTextBack : MonoBehaviour
{

    public Rigidbody Player;
    public Collider PlayerSpine;
    public GameObject Menu, RT;
    public Text Title;

    public Transform Tut11, Tut22;
    public GameObject Hint;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter()
    {
        StartCoroutine(TEXTFADE());

        if (Tut11)
            Tut11.transform.localScale = new Vector3(Tut11.transform.localScale.x, 0f, Tut11.transform.localScale.z);

        if (Tut22)
            Tut22.transform.localScale = new Vector3(Tut22.transform.localScale.x, 0f, Tut22.transform.localScale.z);

        if (GameObject.FindGameObjectWithTag("Arcade"))
        {
            if (DEBUG.DoLOG) Debug.LogWarning("ARCADE MODE");

            //StartCoroutine(ShowHint(-1));
        }
    }

    IEnumerator TEXTFADE()
    {
        Menu.SetActive(false);
        RT.SetActive(false);
        yield return new WaitForSeconds(2.45f);
        Title.material.DOFade(1f, 1.4f);
        PlayerSpine.enabled = true;
        if (DEBUG.DoLOG) Debug.Log("Re-enable color");
    }

    public IEnumerator ShowHint(int a)
    {

        if (a < 0)
        {
            if (Random.Range(0f, 10f) < 5f)
            {
                Hint.GetComponent<Text>().text = "Swipe  Right  To   Sprint";
            }

            else
            {
                Hint.GetComponent<Text>().text = "Swipe  Left  To   Slow Mo";
            }
        }

        else if (a == 1)
        {
            Hint.GetComponent<Text>().text = "Swipe  Right  To   Sprint";
        }

        else if (a == 2)
        {
            Hint.GetComponent<Text>().text = "Swipe  Left  To   Slow Mo";
        }

        yield return new WaitForSeconds(0.5f);
        Hint.transform.DOScaleY(1f, 0.4f);
        yield return new WaitForSeconds(3.5f);
        Hint.transform.DOScaleY(0f, 0.2f);
    }
}
