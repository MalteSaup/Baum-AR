using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeScript : MonoBehaviour
{
    // Start is called before the first frame update
	Renderer c_renderer;
	int angle;
	GameObject blaetter;
	GameObject[] leaves = new GameObject[2000];
	bool[] changed1 = new bool[90];
	bool[] changed2 = new bool[90];
	bool tracked = false;
	bool check = false;
	bool f1 = false;
	bool f2 = false;
	protected int count = 20;
	void Start(){
		
    	}

	// Update is called once per frame
    	void Update(){
		//Debug.Log(leaves);
		tracked = TrackingsScript.tracked;
		if(tracked){
			if(!check){
				string tag;
				for(int i = 1; i <= 2000; i++){
					if(i < 10){
						tag = "blaetter.00" + i;
					}
					else if(i < 100){
						tag = "blaetter.0" + i;
					}
					else{
						tag = "blaetter." + i;
					}
					try{
						leaves[i] = GameObject.Find(tag);	
					}
					catch(Exception e){
						//Debug.Log(tag);
					}

					//Debug.Log(tag);
				}
				for(int i = 0; i < 90; i++){
					//Debug.Log(changed1[i]);
				}
				//Debug.Log(leaves.Length + "HOLLA");
				check = true;
			}
			//Debug.Log(transform.eulerAngles.y);
			if(check){
				//Debug.Log("!!! " + transform.eulerAngles.y);
				angle = (int)transform.eulerAngles.y;
				if(angle >= 0 && angle < 90){   //Frühling changed1
					if(!f1){
						for(int i = 0; i < 2000; i++){
							try{leaves[i].GetComponent<Renderer>().material.color = new Color(0, 255, 80);}
							catch(Exception e){


							}
						}
						f2 = false;
						f1 = true;
					}
					for(int i = 0; i <= angle; i++){
						//Debug.Log(i + " " + changed1[i]);
						if(!changed1[i]){
							changed2[i] = false;
							changed1[i] = true;
							if(i < 21) count = 21;
							else count = 20;
							for(int j = 0; j <= count; j++){
								

								try{leaves[i + 90 * j].GetComponent<Renderer>().material.color = new Color(2, 206, 0);}
								catch(Exception e){
								      Debug.Log(e + " " + i + 90 * j);

								}
							}
						}
					}
					//c_renderer.material.color = new Color(2 * Mathf.Abs(Mathf.Cos(angle)), 255 - 49 * Mathf.Abs(Mathf.Cos(angle)), 0, 255);
				}
				if(angle >= 90 && angle < 180){   //Sommer changed2
					if(!f2){
						for(int i = 0; i < 2000; i++){
							try{leaves[i].GetComponent<Renderer>().material.color = new Color(2, 206, 0);}
							catch(Exception e){
								
							}
						}
						f1 = false;
						f2 = true;
					}
					for(int i = 0; i <= angle % 90; i++){
						//Debug.Log(i + " " + changed1[i]);
						if(!changed2[i]){
							changed1[i] = false;
							changed2[i] = true;
							if(i < 21) count = 21;
							else count = 20;
							for(int j = 0; j <= count; j++){
								try{leaves[i + 90 * j].GetComponent<Renderer>().material.color = new Color(206, 121, 100);}
								catch(Exception e){Debug.Log(i + 90 * j);}

							}
						}
					}
					//c_renderer.material.color = new Color(2 + 204 * Mathf.Abs(Mathf.Sin(angle)), 206 - 85 * Mathf.Abs(Mathf.Sin(angle)), 2 * Mathf.Abs(Mathf.Sin(angle)), 255);
				}
				if(angle >= 180 && angle < 270){   //Herbst changed1
					if(!f1){
						for(int i = 0; i < 2000; i++){
							try{leaves[i].GetComponent<Renderer>().material.color = new Color(206, 121, 2);}
							catch(Exception e){
								
							}
						}
						f2 = false;
						f1 = true;
					}
					for(int i = 0; i <= angle % 90; i++){
						//Debug.Log(i + " " + changed1[i]);
						if(!changed1[i]){
							changed2[i] = false;
							changed1[i] = true;
							if(i < 21) count = 21;
							else count = 20;
							for(int j = 0; j <= count; j++){
								//Debug.Log(i + 90 * j);
								try{leaves[i + 90 * j].GetComponent<Renderer>().material.color = new Color(206, 121, 2);}
								catch(Exception e){Debug.Log(i + 90 * j);}
							}
						}
					}
					//c_renderer.material.color = new Color(206, 121, 2, 255 * Mathf.Abs(Mathf.Sin(angle)));
				}
				if(angle >= 270 && angle < 360){   //Winter changed2
					if(!f2){
						for(int i = 0; i < 2000; i++){
							try{leaves[i].GetComponent<Renderer>().material.color = new Color(206, 121, 2);}
							catch(Exception e){
										

							}
						}
						f1 = false;
						f2 = true;
					}
					for(int i = 0; i <= angle % 90; i++){
						//Debug.Log(i + " " + changed1[i]);
						if(!changed2[i]){
							changed1[i] = false;
							changed2[i] = true;
							if(i < 21) count = 21;
							else count = 20;
							for(int j = 0; j <= count; j++){
								//Debug.Log(i + 90 * j);
								try{leaves[i + 90 * j].GetComponent<Renderer>().material.color = new Color(0, 255, 80);}
								catch(Exception e){Debug.Log(i + 90 * j);}
							}
						}
					}
					//c_renderer.material.color = new Color(0, 255, 80, 255 * Mathf.Abs(Mathf.Cos(angle)));
				}
			
		    	}
		}
	}
}
//						Color.HSVtoRGB(transform.eulerAngles.y / 360.0f, 0.5f, 0.5f));