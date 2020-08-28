using UnityEngine;
using DG.Tweening;
using System.Collections;

public class ColorSystem : MonoBehaviour {

    // public AudioSource Sound;

    //public static bool FirstTimeFBLogin = true;
    public float ColorBright = 0.07f;

    public Light WarningLight;

    GameObject[] BC;
    public Camera Cam;
    Color[] col = new Color[6];
    float T=0f;
    int i = 0;
    Color Def;

    GameObject t;
    Color temp;

    // Use this for initialization
    void Start () {
        if(Cam)
        Def = Cam.backgroundColor;

        col[0] = new Color(0.15f, 0.4f, 0.6f);       //blue
        col[1] = new Color(0.15f, 0.5f, 0.3f);       //green
        col[2] = new Color(0.5f, 0.15f, 0.15f);      //red
        col[3] = new Color(0.5f, 0.5f, 0.15f);       //yellow
        col[4] = new Color(0.25f, 0.25f, 0.6f);     //Navy Blue
        col[5] = new Color(0.15f, 0.45f, 0.2f);       //Green

        //StartCoroutine(Colors());

        BC = GameObject.FindGameObjectsWithTag("Respawn");

        NewColorS();
	}
	
    public void DefColor(float a)
    {
        StartCoroutine(DefColCR(a));
    }

    IEnumerator DefColCR(float a)
    {
        t = GameObject.FindGameObjectWithTag("MainLight");
        temp = t.GetComponent<Light>().color;


        t.GetComponent<Light>().DOIntensity(2.5f, 1f);
        t.GetComponent<Light>().DOBlendableColor(Cam.backgroundColor, 1f);
        yield return new WaitForSeconds(2f);
        t.GetComponent<Light>().DOIntensity(0.75f, 1f);
        t.GetComponent<Light>().DOBlendableColor(temp, 1f);
    }

    public void NewColor()
    {
        int var = Random.Range(1, 6);
        Cam.DOColor(col[var] + new Color(ColorBright, ColorBright, ColorBright), 18f);
        WarningLight.DOColor(col[var] + new Color(0.1f, 0.1f, 0.1f), 10f);
    }

    public void NewColorS()
    {
        int var = Random.Range(1, 6);
        Cam.DOColor(col[var] + new Color(ColorBright, ColorBright, ColorBright), 3f);
        WarningLight.DOColor(col[var] + new Color(ColorBright, ColorBright, ColorBright), 3f);
        //RenderSettings.fogColor = Color.Lerp(new Color(0.4f, 0.4f, 0.4f), col[var], 0.05f);
    }
    public void NewColor(int a)
    {
        if (DEBUG.DoLOG) Debug.Log("Changing Color");

        int var = a;// Random.Range(1, 6);

        if(a==-1)
            var = Random.Range(1, 6);

        Cam.DOColor(col[var] + new Color(ColorBright, ColorBright, ColorBright), 18f);
        WarningLight.DOColor(col[var] + new Color(ColorBright, ColorBright, ColorBright), 10f);
    }
    /*
    public void NewColorQuick(int a)
    {
        Debug.Log("Changing Color");

        int var = a;// Random.Range(1, 6);

        if (a == -1)
            var = Random.Range(1, 6);

        Cam.DOColor(col[var], 10f);
        WarningLight.DOColor(col[var], 10f);
    }
    */

    // Update is called once per frame
    void Update () {
        if (Camera.main)
            Cam = Camera.main;
        if(Cam)
        RenderSettings.fogColor = Cam.backgroundColor;

        if(Input.GetKeyDown(KeyCode.A))
        {
            StopCoroutine(Colors());
            StartCoroutine(Colors());
            //Debug.Log("RESTARTED");
        }
    }

    public IEnumerator Colors()
    {
        RenderSettings.fogColor = Def;
        //Def = Cam.backgroundColor;
        //Debug.Log("A");
        //Cam.DOColor(Def,2f);
        while (true)
        {
            yield return new WaitForSeconds(6.3f);
            //Debug.Log("B");
            Cam.DOColor(col[0], 13f);
            WarningLight.DOColor(col[0], 7f);
            yield return new WaitForSeconds(19);
            /*
            foreach (GameObject G in BC)
            {
                G.gameObject.transform.DOScaleY(G.transform.localScale.y * 2.8f, 15f);
            }*/

            if (DEBUG.DoLOG) Debug.Log("C");
            Cam.DOColor(col[1], 18f);
            WarningLight.DOColor(col[1], 9f);
            yield return new WaitForSeconds(20f);

            /*
            foreach (GameObject G in BC)
            {
                G.GetComponent<buildingscroll>().enabled = false;
            }
            */

            yield return new WaitForSeconds(20);
            /*
            foreach (GameObject G in BC)
            {
                G.SetActive(false);
            }*/

            Cam.DOColor(col[3], 18f);
            WarningLight.DOColor(col[3], 9f);
            yield return new WaitForSeconds(30);        //52
            Cam.DOColor(col[2], 18f);
            WarningLight.DOColor(col[2], 9f);
            yield return new WaitForSeconds(5f);
        }
    }

    public void HideBC()
    {
        StartCoroutine(HideBCour());
    }

    IEnumerator HideBCour()
    {
        foreach (GameObject G in BC)
        {
            if (!G.gameObject.GetComponent<Animator>())
                G.transform.DOScaleY(0f, 1f);
        }

        yield return new WaitForSeconds(2f);

        foreach (GameObject G in BC)
        {
            if (!G.gameObject.GetComponent<Animator>())
                G.transform.DOScaleY(Random.Range(23f, 123f), 3f);
        }
        if (DEBUG.DoLOG) Debug.Log("Hid all buildings");
    }

    public void RandomBuilding()
    {
        foreach (GameObject G in BC)
        {
            if (!G.gameObject.GetComponent<Animator>())
                G.transform.DOScaleY(Random.Range(90f, 140f), 5f);
            if (DEBUG.DoLOG) Debug.LogError(G + " scale : " + G.transform.localScale.y);
        }
    }

    public Color32 ToColor(int HexVal)
    {
        byte R = (byte)((HexVal >> 16) & 0xFF);
        byte G = (byte)((HexVal >> 8) & 0xFF);
        byte B = (byte)((HexVal) & 0xFF);
        return new Color32(R, G, B, 255);
    }
}
