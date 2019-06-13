using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//0f, 255/255f, 80/255f fresh green 
//2/255, 206/255, 0 summer green 
//206/255f, 121/255f, 10/255f herbst braun 


public class CubeScript : MonoBehaviour
{
	int count = 16;
	int previousAngle = 0;
	int angle;

	static int numberOfLeaves = 1500;

	GameObject[] leaves = new GameObject[numberOfLeaves];

	Renderer snow; 

	IEnumerator[] coroutines = new IEnumerator[60];

	Vector3[] positions = new Vector3[numberOfLeaves];

	bool[] changed = new bool[60];
	bool tracked = false;
	bool check = false;
	bool cCheck;
	bool running; 
	bool herbstCheck = false;

	Color fruehling = new Color(0f,1f,0.314f);
	Color sommer = new Color(0.008f, 0.808f, 0f);
	Color herbst = new Color(0.808f, 0.475f, 0.039f);

	void Start(){}




	public void changeColor(int angle, Color color, int startPoint = 0){
		angle = (angle - 15) % 90;
		if(startPoint == 0 && angle < 60){
			for(int i = 0; i <= angle; i++){
				if(!changed[i]){
					changed[i] = true;
					colorChangeRoutine(i, color);
				}
			}
		}
		else if(startPoint - 15 > 0){
			if((startPoint - 15) % 90 > 5 && (startPoint - 15) % 90 <= 60){
				for(int i = (startPoint - 15) % 90; i < 60; i++){
					if(changed[i]){
						changed[i] = false;
						colorChangeRoutine(i, color);
					}
				}
			}
		}
	}

	void colorChangeRoutine(int i, Color color){
		for(int j = 0; j <= count; j++){
			try{
				leaves[i + 90 * j].GetComponent<Renderer>().material.color = color;
				if(i < 30){leaves[i + 60 + 90 * j].GetComponent<Renderer>().material.color = color;}
			}
			catch(Exception e){}
		}
	}

	void colorChangeComplete(Color color){
		for(int i = 0; i <= numberOfLeaves; i++){
			try{leaves[i].GetComponent<Renderer>().material.color = color;}
			catch(Exception e){}
		}
	}
		
	public void changePositionComplete(){
		for(int j = 0; j <= numberOfLeaves; j++){
			try{
				leaves[j].GetComponent<Renderer>().enabled = true;
				leaves[j].transform.localPosition = positions[j];
			}
			catch(Exception e){}
		}
	}

	public void changeControlFlag(bool flag){
		for(int i = 0; i < 60; i++){
			changed[i] = flag;
		}
	}

	public void colorCheckComplete(Color color, int activeFlag = 1){
		if(!cCheck && activeFlag == 1){
			colorChangeComplete(color);
			cCheck = true;
		}
		else if(!cCheck && activeFlag == 0){
			changePositionComplete();
			colorChangeComplete(color);
			cCheck = true;
		}

		else if(!cCheck && activeFlag == -1){
			for(int i = 0; i <= numberOfLeaves; i++){
				try{
					leaves[i].GetComponent<Renderer>().enabled= false;
				}
				catch(Exception e){}
			}
			cCheck = true;
		}

		else if(cCheck){cCheck = false;}
	}

	public void fallingLeavesStart(int angle){
		
		if((angle - 15) % 90 <= 60){
			for(int i = 0; i <= (angle - 15) % 90; i++){
				if(!changed[i] && leaves[i] != null){
					if(leaves[i].GetComponent<Renderer>().enabled && !leaves[i].GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("fallingLeave")){
						for(int j = 0; j <= count; j++){
							try{
								leaves[i + 90 * j].GetComponent<Animator>().Play("fallingLeave");
								if(i <= 30){leaves[i + 60 + 90 * j].GetComponent<Animator>().Play("fallingLeave");}
							}
							catch(Exception e){}
						}
						StartCoroutine(coroutines[i]);
					}
				}
			}
		}
	}

	public void changePosition(Color color, int angle, int startPoint = 0, bool deactiveFlag = true){
		angle = (angle - 15) % 90;
		if(startPoint == 0 && angle <= 60){
			for(int j = 0; j < angle; j++){
				try{
					if(deactiveFlag){
						leaves[angle + 90 * j].transform.localPosition = positions[angle + 90 * j];
						leaves[angle + 90 * j].GetComponent<Renderer>().enabled = true;
						if(angle <= 30){
							leaves[angle + 60 + 90 * j].transform.localPosition = positions[angle + 90 * j];
							leaves[angle + 60 + 90 * j].GetComponent<Renderer>().enabled = true;
						}
						if(color != Color.black){
							changeColor(angle, color);
						}
					}
				}
				catch(Exception e){}	
			}
			changed[angle] = true;
		}
		else if((startPoint - 15) % 90 > 5 && (startPoint - 15) % 90 <= 60 && angle <= 60){
			if(changed[angle]){
				for(int j = startPoint % 90; j < 60; j++){
					try{
						//Debug.Log(i + "HASS");
						leaves[angle + 90 * j].GetComponent<Renderer>().enabled = deactiveFlag;
						if(angle <= 30){leaves[angle + 60 + 90 * j].GetComponent<Renderer>().enabled = deactiveFlag;}
					}
					catch(Exception e){}
				}
				changed[angle] = false;
			}
		}
	}




	void Update(){
		tracked = TrackingsScript.tracked;
		if(tracked){
			if(!check){
				
				angle = (int)transform.eulerAngles.y;
				if(angle % 90 >= 15 && angle % 90 < 75){cCheck = true;} 
				string tag;
				for(int i = 0; i < 60; i++){
					coroutines[i] = deactivateLeaves(i);
				}
				for(int i = 1; i <= numberOfLeaves; i++){
					if(i < 10){tag = "blaetter.00" + i;}
					else if(i < 100){tag = "blaetter.0" + i;}
					else{tag = "blaetter." + i;}
					try{
						leaves[i] = GameObject.Find(tag);
						positions[i] = leaves[i].transform.localPosition;
					}
					catch(Exception e){
						//Debug.Log(tag);
					}
				}
				snow = GameObject.Find("Schnee").GetComponent<Renderer>();
				check = true;
			}

			if(check){

				angle = (int)transform.eulerAngles.y;
				//Debug.Log(angle );
				//if(previousAngle > 350 && angle < 20){previousAngle = angle;}

				//Debug.Log(changed[angle%60]);








				Debug.Log(angle);
				if(angle >= 345 || angle < 15){	//Frühling
					Debug.Log("HOLLA" + cCheck);
					if(!cCheck){
						Debug.Log("HOLLA");
						colorCheckComplete(fruehling, 0);
					}
					if((angle) % 30 >= 15){changeControlFlag(true);}
					else if((angle) % 30 < 15){changeControlFlag(false);}
				}




				if(angle >= 15 && angle < 75){   //F->S
					if(cCheck){colorCheckComplete(Color.black, 0);}
					if(previousAngle <= angle){changeColor(angle, sommer);}
					else{changeColor(angle, fruehling, previousAngle);}
				}

				if(angle >= 75 && angle < 105){	//Sommer
					if(!cCheck){colorCheckComplete(sommer);}

					if((angle - 15) % 30 > 15){changeControlFlag(false);}

					else if((angle - 15) % 30 < 15){changeControlFlag(true);}
				}


				if(angle >= 105 && angle < 165){   //S->H
					if(cCheck){colorCheckComplete(Color.black, 0);}
					if(previousAngle <= angle){changeColor(angle, herbst);}
					else{changeColor(angle, sommer, previousAngle);}
				}

				if(angle >= 165 && angle < 195){ //Herbst
					if(!cCheck){colorCheckComplete(herbst, 0);}
					if(!herbstCheck){herbstCheck = true;}
					if((angle - 15) % 30 > 15){changeControlFlag(false);}
					else if((angle - 15) % 30 < 15){changeControlFlag(true);}
				}

				if(angle >= 195 && angle < 255){   //H->W
					if(cCheck){colorCheckComplete(Color.black, 0);}
					if(previousAngle <= angle){
						fallingLeavesStart(angle);
					}
					else{
						changePosition(herbst, angle, previousAngle, true);
					}
				}

				if(angle >= 255 && angle < 285){ //Winter
					if(!cCheck){
						colorCheckComplete(fruehling, -1);
					}
					if((angle - 15) % 30 > 15){
						changeControlFlag(false);
						if(herbstCheck){
							colorChangeComplete(fruehling);
							herbstCheck = false;
						}
					}
					else if((angle - 15) % 30 < 15){
						changeControlFlag(true);
						if(!herbstCheck){
							colorChangeComplete(herbst);
							herbstCheck = true;
						}
					
					}
					if(!snow.enabled){snow.enabled = true;}
				}

				if(angle >= 285 && angle < 345){ //W->F
					if(cCheck){
						Debug.Log("W->F");
						colorCheckComplete(Color.black, 0);}
					if(previousAngle <= angle){
						changePosition(fruehling, angle);
						changeColor(angle, fruehling);}
					else{
						changePosition(Color.black, angle, previousAngle, false);
					}
				}

				if((angle < 254 || angle > 285) && snow.enabled){snow.enabled = false;}
					

				previousAngle = angle;		
			}

		}
	}






	public IEnumerator deactivateLeaves(int number){
		yield return new WaitForSeconds(1.3f);
		changed[number] = true;
		for(int i = 0; i <= count; i++){
			try{
				leaves[number + 90 * i].GetComponent<Renderer>().enabled = false;
				if(number <= 30){leaves[number + 60 + 90 * i].GetComponent<Renderer>().enabled = false;}
			}
			catch(Exception e){}
		}
	}
}























					