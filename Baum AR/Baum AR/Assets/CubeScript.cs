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

	IEnumerator[] coroutines = new IEnumerator[45];

	Vector3[] positions = new Vector3[numberOfLeaves];

	bool[] changed = new bool[45];
	bool tracked = false;
	bool check = false;
	bool cCheck;
	bool running; 

	Color fruehling = new Color(0f,1f,0.314f);
	Color sommer = new Color(0.008f, 0.808f, 0f);
	Color herbst = new Color(0.808f, 0.475f, 0.039f);

	void Start(){}




	public void changeColor(int angle, Color color, int startPoint = 0){
		if(startPoint == 0){
			for(int i = 0; i <= angle % 45; i++){
				if(!changed[i]){
					//Debug.Log(i + " : " + color);
					changed[i] = true;
					colorChangeRoutine(i, color);
				}
			}
		}
		else{
			if(startPoint % 45 > 5){
				for(int i = startPoint % 45; i < 45; i++){
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
				leaves[i + 45 + 90 * j].GetComponent<Renderer>().material.color = color;
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
		for(int i = 0; i < 45; i++){
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
		for(int i = 0; i <= angle % 45; i++){
			if(!changed[i] && leaves[i] != null){
				if(leaves[i].GetComponent<Renderer>().enabled && !leaves[i].GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("fallingLeave")){
					for(int j = 0; j <= count; j++){
						try{
							leaves[i + 90 * j].GetComponent<Animator>().Play("fallingLeave");
							leaves[i + 45 + 90 * j].GetComponent<Animator>().Play("fallingLeave");

						}
						catch(Exception e){}
					}
					StartCoroutine(coroutines[i]);

				}
			}
		}

	}

	public void changePosition(Color color, int i, int startPoint = 0, bool deactiveFlag = true){
		if(startPoint == 0){
			for(int j = 0; j <= i % 45; j++){
				try{
					if(deactiveFlag){
						leaves[i + 90 * j].transform.localPosition = positions[i + 90 * j];
						leaves[i + 45 + 90 * j].transform.localPosition = positions[i + 90 * j];
						leaves[i + 90 * j].GetComponent<Renderer>().enabled = true;
						leaves[i + 45 + 90 * j].GetComponent<Renderer>().enabled = true;
						if(color != Color.black){
							changeColor(i, color);
						}
					}
				}
				catch(Exception e){}	
			}
			changed[i % 45] = true;
		}
		else if(startPoint > 5){
			if(startPoint % 45 > 5){
				//Debug.Log("DF: " + deactiveFlag + " Changed: " + changed[i % 45]);
				if(changed[i % 45]){
					//Debug.Log(startPoint % 45 + " SP");
					for(int j = startPoint % 45; j < 45; j++){
						try{
							//Debug.Log(i + "HASS");
							leaves[i + 90 * j].GetComponent<Renderer>().enabled = deactiveFlag;
							leaves[i + 45 + 90 * j].GetComponent<Renderer>().enabled = deactiveFlag;
						}
						catch(Exception e){}
					}
					changed[i % 45] = false;
				}
			}
		}
	}




	void Update(){
		tracked = TrackingsScript.tracked;
		if(tracked){
			if(!check){
				angle = (int)transform.eulerAngles.y;
				if(angle % 90 >= 45){cCheck = true;}
				string tag;
				for(int i = 0; i < 45; i++){
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
				check = true;
			}

			if(check){

				angle = (int)transform.eulerAngles.y;
				//Debug.Log(angle );
				//if(previousAngle > 350 && angle < 20){previousAngle = angle;}











				if(angle >= 0 && angle < 45){	//Frühling
					if(!cCheck){colorCheckComplete(fruehling, 0);}
					if(angle % 45 > 22){changeControlFlag(false);}
					else if(angle % 45 < 22){changeControlFlag(true);}
				}




				if(angle >= 45 && angle < 90){   //F->S
					if(cCheck){colorCheckComplete(Color.black, 0);}
					if(previousAngle <= angle){changeColor(angle, sommer);}
					else{changeColor(angle, fruehling, previousAngle);}
				}

				if(angle >= 90 && angle < 135){	//Sommer
					if(!cCheck){colorCheckComplete(sommer);}

					if(angle % 45 > 22){changeControlFlag(false);}

					else if(angle % 45 < 22){changeControlFlag(true);}
				}


				if(angle >= 135 && angle < 180){   //S->H
					if(cCheck){colorCheckComplete(Color.black, 0);}
					if(previousAngle <= angle){changeColor(angle, herbst);}
					else{changeColor(angle, sommer, previousAngle);}}

				if(angle >= 180 && angle < 225){ //Herbst
					if(!cCheck){
						colorCheckComplete(herbst, 0);
					}
					if(angle % 45 > 22){changeControlFlag(false);}
					else if(angle % 45 < 22){changeControlFlag(true);}
				}

				if(angle >= 225 && angle < 270){   //H->W
					if(cCheck){colorCheckComplete(Color.black, 0);}
					if(previousAngle <= angle){
						fallingLeavesStart(angle);
					}
					else{
						changePosition(herbst, angle, previousAngle, true);
					}
				}

				if(angle >= 270 && angle < 315){ //Winter
					if(!cCheck){
						colorCheckComplete(fruehling, -1);
					}
					if(angle % 45 > 22){changeControlFlag(false);}
					else if(angle % 45 < 22){changeControlFlag(true);}
				}

				if(angle >= 315 && angle < 360){ //W->F
					if(cCheck){colorCheckComplete(Color.black, 0);}
					if(previousAngle <= angle){
						changePosition(fruehling, angle);
						changeColor(angle, fruehling);}
					else{
						changePosition(Color.black, angle, previousAngle, false);
					}
				}
					

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
				leaves[number + 45 + 90 * i].GetComponent<Renderer>().enabled = false;
			}
			catch(Exception e){}
		}
	}
}























					