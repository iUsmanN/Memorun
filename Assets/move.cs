using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class move : MonoBehaviour {
    public Transform Char;
    public float Sp = 8f;

    GameObject[] UpCubes;

	// Use this for initialization
	void Start () {
        if (!Char)
            Char = this.gameObject.transform;
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log("T : " + Time.timeScale);
        Char.Translate(0f, 0f, Sp * Time.deltaTime);
    }
}
