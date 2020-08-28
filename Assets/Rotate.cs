using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour {
    public float x, y, z;
    float ya;
    // Use this for initialization
    void Start () {
        //y = Mathf.Abs(y);// * -1f;
        //ya = Random.Range(y - 15f, y + 15f);
        ya = y;
        ya *= Random.Range(2f, 4f);
    }
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(new Vector3(x * Time.deltaTime, ya * Time.deltaTime, z * Time.deltaTime));
	}
}
