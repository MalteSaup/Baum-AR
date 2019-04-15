using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeScript : MonoBehaviour
{
    // Start is called before the first frame update
	Renderer c_renderer;
	float angle;
    	void Start(){
		c_renderer = GetComponent<Renderer>();
    	}

	// Update is called once per frame
    	void Update(){
		Debug.Log(transform.eulerAngles.y);
		angle = transform.eulerAngles.y * Mathf.PI / 180.0f;
		if(angle != 90 && angle != 180){
			c_renderer.material.color = new Color(Mathf.Cos(angle), Mathf.Sin(angle), 0.5f);
		}
		else{
			c_renderer.material.color = new Color(Mathf.Cos(angle), Mathf.Sin(angle), 0.5f);	
		}
    	}
}
//						Color.HSVtoRGB(transform.eulerAngles.y / 360.0f, 0.5f, 0.5f));