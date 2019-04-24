using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeScript : MonoBehaviour
{
    // Start is called before the first frame update
	Renderer c_renderer;
	float angle;
	GameObject blaetter;
    void Start(){
		this.blaetter = (GameObject)transform.Find("blaetter").gameObject;
		Debug.Log(blaetter);
		c_renderer = this.blaetter.GetComponent<Renderer>();
    }

	// Update is called once per frame
    	void Update(){
			Debug.Log(transform.eulerAngles.y);
			angle = transform.eulerAngles.y * 3.14f / 180.0f;
			if(angle >= 0 && angle < 90){   //Frühling
				c_renderer.material.color = new Color(2 * Mathf.Abs(Mathf.Cos(angle)), 255 - 49 * Mathf.Abs(Mathf.Cos(angle)), 0, 255);
			}
			if(angle >= 90 && angle < 180){   //Sommer
				c_renderer.material.color = new Color(2 + 204 * Mathf.Abs(Mathf.Sin(angle)), 206 - 85 * Mathf.Abs(Mathf.Sin(angle)), 2 * Mathf.Abs(Mathf.Sin(angle)), 255);
			}
			if(angle >= 180 && angle < 270){   //Herbst
				c_renderer.material.color = new Color(206, 121, 2, 255 * Mathf.Abs(Mathf.Sin(angle)));
			}
			if(angle >= 270 && angle < 360){   //Winter
				c_renderer.material.color = new Color(0, 255, 80, 255 * Mathf.Abs(Mathf.Cos(angle)));
			}

    	}
}
//						Color.HSVtoRGB(transform.eulerAngles.y / 360.0f, 0.5f, 0.5f));