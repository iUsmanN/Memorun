using UnityEngine;
using System.Collections;

public class MoveAI : MonoBehaviour
{
    public Menu menu;
    public Transform Char;
    public float Sp = 7.8f;

    public float T = 0f;
    public float incSp = 100f;
    GameObject[] UpCubes;

    public bool Boosting = false;

    // Use this for initialization
    void Start()
    {/*
        UpCubes = GameObject.FindGameObjectsWithTag("HighCube");
        foreach (GameObject g in UpCubes)
        {
            g.transform.parent = null;
            g.transform.position = new Vector3(g.transform.position.x, 0.95f, g.transform.position.z);
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        if (menu.isAIMode)
        {
            T += Time.deltaTime;
            if (T > 10f)
            {
                if (incSp > 0f)
                {
                    Sp = Mathf.Lerp(Sp, Random.Range(7.8f, 9f), 30f);
                    incSp--;
                }
                T = Random.Range(-2f, 2f);
                Debug.Log("Change Speed");
            }
        }
        if (Char)
            Char.Translate(0f, 0f, Sp * Time.deltaTime);
    }

    public IEnumerator BoostSP(float howlong)
    {
        if (!Boosting)
        {

            if (DEBUG.DoLOG) Debug.LogError("BOOSTING");
            Boosting = true;

            incSp--;
            Sp = Mathf.Lerp(Sp, 8.5f+ Random.Range(-0.1f,0.15f), 2f);
            yield return new WaitForSeconds(howlong);
            Sp = Mathf.Lerp(Sp, 7f, 2f);
            yield return new WaitForSeconds(2f);
            Boosting = false;
        }
    }
}
