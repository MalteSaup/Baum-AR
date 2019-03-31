using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTS : MonoBehaviour
{
	float a = 0F;
	// Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		this.transform.localPosition = new Vector3(0,0,a);
		a += 0.02f;
    }
}
