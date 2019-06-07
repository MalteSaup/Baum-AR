using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//leaves[i].setActive(true);
//0f, 255/255f, 80/255f fresh green 
//2/255, 206/255, 0 summer green 
//206/255f, 121/255f, 10/255f herbst braun 
//

public class CubeScript : MonoBehaviour
{
	// Start is called before the first frame update
	Renderer c_renderer;
	int angle;
	static int numberOfLeaves = 1500;
	GameObject[] leaves = new GameObject[numberOfLeaves];
	IEnumerator[] coroutines = new IEnumerator[numberOfLeaves];
	Vector3[] positions = new Vector3[numberOfLeaves];

	bool[] changed1 = new bool[90];
	bool[] changed2 = new bool[90];
	bool tracked = false;
	bool check = false;
	bool f1 = false;
	bool f2 = false;

	int count = 17;
	int previousAngle = 0;

	Color fruehling = new Color(0f,1f,0.314f);
	Color sommer = new Color(0.008f, 0.808f, 0f);
	Color herbst = new Color(0.808f, 0.475f, 0.039f);

	void Start(){

	}

	// Update is called once per frame
	void Update(){
		tracked = TrackingsScript.tracked;
		if(tracked){
			if(!check){
				string tag;
				for(int i = 1; i <= numberOfLeaves; i++){
					if(i < 10){tag = "blaetter.00" + i;}
					else if(i < 100){tag = "blaetter.0" + i;}
					else{tag = "blaetter." + i;}
					try{
						leaves[i] = GameObject.Find(tag);
						coroutines[i] = deactivateLeave(i);
						positions[i] = leaves[i].transform.localPosition;
					}
					catch(Exception e){
						Debug.Log(tag);
					}
				}
				check = true;
			}

			if(check){

				angle = (int)transform.eulerAngles.y;
				Debug.Log(angle);
				//if(previousAngle > 350 && angle < 20){previousAngle = angle;}











				if(angle >= 0 && angle < 45){	//Frühling
					if(!f2){checkf1f2(2, fruehling, 2);}
				}




				if(angle >= 45 && angle < 90){   //F->S
					if(previousAngle <= angle){
						changeColor(angle, 1, sommer);
					}
					else{
						changeColor(angle, 1, fruehling, previousAngle);
					}
				}

				if(angle >= 90 && angle < 135){	//Sommer
					if(!f1){checkf1f2(1, sommer);}
				}


				if(angle >= 135 && angle < 180){   //S->H
					if(previousAngle <= angle){
						changeColor(angle, 2, herbst);
					}
					else{
						changeColor(angle, 2, sommer, previousAngle);}
				}

				if(angle >= 180 && angle < 225){ //Herbst
					if(!f2){checkf1f2(2, herbst);}
				}

				if(angle >= 225 && angle < 270){   //H->W
					if(previousAngle <= angle){
						fallingLeavesStart(angle);
					}
					else{
						//bringLeavesBack(angle, herbst);
					}
				}

				if(angle >= 270 && angle < 315){ //Winter
					if(!f1){checkf1f2(1, herbst, 1);}
				}

				if(angle >= 315 && angle < 360){ //W->F
					if(previousAngle <= angle){
						bringLeavesBack(angle, fruehling);
					}
					else{
						bringLeavesBack(angle, fruehling, previousAngle);
					}
				}



























				previousAngle = angle;		
			}

		}
	}
	public void checkf1f2(int fNumb, Color color, int activateNumb = 0){
		if(fNumb == 1){
			f2 = false;
			f1 = true;
		}
		else if(fNumb == 2){
			f1 = false;
			f2 = true;
		}
		if(activateNumb == 0){
			for(int i = 0; i <= numberOfLeaves; i++){
				try{leaves[i].GetComponent<Renderer>().material.color = color;}
				catch(Exception e){

				}
			}
		}
		else if(activateNumb == 1){ 	//Deaktivierung Blätter
			for(int i = 0; i <= numberOfLeaves; i++){
				try{leaves[i].active = false;}
				catch(Exception e){

				}
			}
		}

		else if(activateNumb == 2){	//Aktivierung Blätter + Färbung
			for(int i = 0; i <= numberOfLeaves; i++){
				try{
					//Debug.Log(i + " : " + leaves[i].active);
					leaves[i].active = true;
					//Debug.Log(i + " : " + leaves[i].active);
					leaves[i].transform.localPosition = positions[i];
					leaves[i].GetComponent<Renderer>().material.color = color;
				}
				catch(Exception e){

				}
			}
		}
	}


	public void fallingLeavesStart(int angle){

		for(int i = 0; i <= angle % 45; i++){
			if(!changed1[i]){
				changed2[i] = false;
				changed1[i] = true;
				count = 17;
				for(int j = 0; j <= count; j++){
					try{
						leaves[i + 90 * j].GetComponent<Animator>().Play("fallingLeave");
						leaves[i + 45 + 90 * j].GetComponent<Animator>().Play("fallingLeave");
						StartCoroutine(coroutines[i + 90 * j]);
						StartCoroutine(coroutines[i + 45 + 90 * j]);
					}
					catch(Exception e){}
				}
			}
		}

	}


	public IEnumerator deactivateLeave(int number){
		yield return new WaitForSeconds(1.3f);
		leaves[number].active = false;
		//Debug.Log(number);
	}

	public void bringLeavesBack(int angle, Color color, int startPoint = 0){
		if(startPoint == 0){
			for(int i = 0; i < angle % 45; i++){
				if(!changed2[i]){
					count = 17;
					for(int j = 0; j <= count; j++){
						try{
							if(!leaves[i + 45 * j].active){
								changed2[i] = true;
								changed1[i] = false;
								leaves[i + 90 * j].active = true;
								leaves[i + 45 + 90 * j].active = true;
								leaves[i + 90 * j].transform.localPosition = positions[i + 90 * j];
								leaves[i + 45 + 90 * j].transform.localPosition = positions[i + 45 + 90 * j];
								leaves[i + 90 * j].GetComponent<Renderer>().material.color = color;
								leaves[i + 45 + 90 * j].GetComponent<Renderer>().material.color = color;
							}
						}
						catch(Exception e){}
					}	
				}
			}
		}
		else{
			for(int i = startPoint % 45; i < 45; i++){
				if(changed2[i]){
					count = 17;
					for(int j = 0; j <= count; j++){
						try{
							if(leaves[i].active){
								changed2[i] = false;
								changed1[i] = true;
								leaves[i + 90 * j].active = false;
								leaves[i + 45 + 90 * j].active = false;
							}
						}
						catch(Exception e){}
					}	
				}
			}
		}

	}





























	public void changeColor(int angle, int fNumb, Color color, int startPoint = 0){
		//Debug.Log("sP " + startPoint + " : " + angle + " : " + color);
		if(startPoint == 0){
			//Debug.Log("Vorwarts");
			if(fNumb == 1){
				for(int i = 0; i <= angle % 45; i++){
					if(!changed1[i]){
						changed1[i] = true;
						changed2[i] = false;
						if(i < 60) count = 17;
						else count = 16;
						for(int j = 0; j <= count; j++){
							try{
								leaves[i + 90 * j].GetComponent<Renderer>().material.color = color;
								leaves[i + 45 + 90 * j].GetComponent<Renderer>().material.color = color;
							}
							catch(Exception e){}
						}
					}
				}
			}
			else if(fNumb == 2){
				for(int i = startPoint; i <= angle % 45; i++){
					if(!changed2[i]){
						changed1[i] = false;
						changed2[i] = true;
						if(i < 60) count = 17;
						else count = 16;
						for(int j = 0; j <= count; j++){
							try{
								leaves[i + 90 * j].GetComponent<Renderer>().material.color = color;
								leaves[i + 45 + 90 * j].GetComponent<Renderer>().material.color = color;
							}
							catch(Exception e){}
						}
					}
				}
			}
		}
		else{
			//Debug.Log("Zuruck");
			if(fNumb == 1 && startPoint % 45 > 5){
				//Debug.Log("SP " + startPoint + " Angle: " + angle);
				for(int i = startPoint % 45; i < 45; i++){
					if(changed1[i]){
						//Debug.Log(i);
						changed1[i] = false;
						changed2[i] = true;
						count = 17;
						for(int j = 0; j <= count; j++){
							try{
								leaves[i + 90 * j].GetComponent<Renderer>().material.color = color;
								leaves[i + 45 + 90 * j].GetComponent<Renderer>().material.color = color;
							}
							catch(Exception e){}
						}
					}
				}
			}
			else if(fNumb == 2 && startPoint % 45 > 5){
				for(int i = startPoint % 45; i < 45; i++){
					if(changed2[i]){
						//Debug.Log(i);
						changed1[i] = true;
						changed2[i] = false;
						if(i < 60) count = 17;
						else count = 16;
						for(int j = 0; j <= count; j++){
							try{
								leaves[i + 90 * j].GetComponent<Renderer>().material.color = color;
								leaves[i + 45 + 90 * j].GetComponent<Renderer>().material.color = color;
							}
							catch(Exception e){}
						}
					}
				}
			}
		}
	}
}						