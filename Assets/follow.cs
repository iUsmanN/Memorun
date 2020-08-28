using UnityEngine;
using System.Collections;

public class follow : MonoBehaviour {
    public GameObject X;
    // Use this for initialization
    void Start () {
	
	}

    // Update is called once per frame
    void Update()
    {
        if (!X)
            if(Camera.main)
            X = Camera.main.gameObject;
        if(X)
        transform.position = new Vector3(X.transform.position.x, transform.position.y, transform.position.z);
    }

    public void JumpCam()
    {
        Vector3 velocity = Vector3.zero;
        Vector3 needPos = new Vector3(transform.position.x, transform.position.y + 15f, transform.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, needPos, ref velocity, 0.5f);
    }
}
