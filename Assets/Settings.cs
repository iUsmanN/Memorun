using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour {
    //Quality
    public Button L, M, H;
    public ParticleSystem Particles;
	public AudioSource Music; public Text MusicText;

	public static bool MusicOn = true;

	public void ToggleMusic()
	{
		if (Music.volume == 0f) {
			MusicText.text = "MUSIC :  ON";
			Music.volume = 0.75f;
			MusicOn = true;
		} else if (Music.volume > 0f) {
			MusicText.text = "MUSIC :  OFF";
			Music.volume = 0f;
			MusicOn = false;
		}

		Debug.Log ("MUSIC : " + MusicOn.ToString ());
	}

    void HighlightButton(Button Y, Button N, Button N2)
    {
        //Debug.Log("SettingCol");
        ColorBlock Col = Y.colors;
        Col.normalColor = new Color(1f, 1f, 1f, 2.5f);
        Y.colors = Col;

        ColorBlock Col2 = N.colors;
        Col2.normalColor = new Color(1f, 1f, 1f, 0.15f);
        N.colors = Col2;

        ColorBlock Col3 = N2.colors;
        Col3.normalColor = new Color(1f, 1f, 1f, 0.15f);
        N2.colors = Col2;
    }

	// Use this for initialization
	void Start () {
        if (DEBUG.DoLOG) Debug.LogWarning("SC " + QualitySettings.shadowCascades.ToString());
        
        //High by Def
        if(PlayerPrefs.GetInt("QualityOfGame", 1) == 0)
        {
            SetMedium();

            //Highlight Button
            HighlightButton(M, L, H);
        }

        else if (PlayerPrefs.GetInt("QualityOfGame", 1) == -1)
        {
            SetLow();
            HighlightButton(L, M, H);
        }

        else if(PlayerPrefs.GetInt("QualityOfGame", 1) == 1)
        {
            SetHigh();
            HighlightButton(H, L, M);
        }

        else
        {
            Debug.LogError("Error in Quality");
        }
    }
	
	// Update is called once per frame
	void Update () {
        //Debug.Log("P : " + Particles.particleCount.ToString());
	}

    public void SetLow()
    {
        if (DEBUG.DoLOG) Debug.LogWarning("Low Settings");
        //Buildings
        GameObject[] g = GameObject.FindGameObjectsWithTag("Respawn");

        //Debug.LogWarning("Turning OFf Buildings Shadows");
        foreach(GameObject a in g)
        {
            if(a.GetComponent<buildingscroll>())
            a.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        }
        //Debug.LogWarning("[DONE] Turning OFf Buildings Shadows");
        //

        QualitySettings.SetQualityLevel(0);
        QualitySettings.shadowCascades = 0;

        //Application.targetFrameRate = 30;

        QualitySettings.vSyncCount = 0;

        QualitySettings.antiAliasing = 0;

        PlayerPrefs.SetInt("QualityOfGame", -1);
        HighlightButton(L, M, H);

        ReduceRes(1280,720);
        //int maxp = 15;
        Particles.emissionRate = 3f;
    }

    public void SetMedium()
    {
        Debug.LogWarning("Medium Settings");
        //Buildings
        GameObject[] g = GameObject.FindGameObjectsWithTag("Respawn");

        foreach (GameObject a in g)
        {
            if (a.GetComponent<buildingscroll>())
                a.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        }
        //
        QualitySettings.SetQualityLevel(1);
        //QualitySettings.shadowCascades = 1;

        QualitySettings.vSyncCount = 0;

        QualitySettings.antiAliasing = 0;

        PlayerPrefs.SetInt("QualityOfGame", 0);
        HighlightButton(M, L, H);

        ReduceRes(1280, 720);
        Particles.emissionRate = 9f;
    }

    public void SetHigh()
    {
        if (DEBUG.DoLOG) Debug.LogWarning("High Settings");
        //Buildings
        GameObject[] g = GameObject.FindGameObjectsWithTag("Respawn");

        foreach (GameObject a in g)
        {
            if (a.GetComponent<buildingscroll>())
                a.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        }
        //
        QualitySettings.SetQualityLevel(2);
        //QualitySettings.shadowCascades = 1;

        PlayerPrefs.SetInt("QualityOfGame", 1);
        HighlightButton(H, L, M);

        QualitySettings.vSyncCount = 0;

        QualitySettings.antiAliasing = 0;

        ReduceRes(1920, 1080);
        Particles.emissionRate = 11f;
    }

    public void ReduceRes(int a , int b)
    {
            Screen.SetResolution(a, b, true);
    }
}
