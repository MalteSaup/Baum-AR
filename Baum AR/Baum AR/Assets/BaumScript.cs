using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//0f, 255/255f, 80/255f fresh green 
//2/255, 206/255, 0 summer green 
//206/255f, 121/255f, 10/255f herbst braun 


public class BaumScript : MonoBehaviour
{
	int count = 16;								//Veränderungsvariable
	int previousAngle = 0;
	int angle;

	static int numberOfLeaves = 1500;

	GameObject[] leaves = new GameObject[numberOfLeaves];

	Renderer snow; 

	IEnumerator[] coroutines = new IEnumerator[60];
	IEnumerator audioCoroutine;

	Vector3[] positions = new Vector3[numberOfLeaves];

	bool[] changed = new bool[60];
	bool tracked = false;
	bool check = false;
	bool cCheck = false;
	bool herbstCheck = false;
	bool herbstAudioPlaying = false;
	bool audioChanged = false;

	Color fruehling = new Color(0f,1f,0.314f);
	Color sommer = new Color(0.008f, 0.808f, 0f);
	Color herbst = new Color(0.808f, 0.475f, 0.039f);

	public AudioClip[] jahreszeiten = new AudioClip[5];
	public AudioSource backgroundNoise;


	void Start(){}

	public void changeColor(int angle, Color color, int startPoint = 0){  	//Ruft Farbänderung auf und verhindert mit dem changed Array doppelte einfärbung
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

	void colorChangeRoutine(int i, Color color){ 				//Wird von changeColor aufgerufen und führt Färbung durch
		for(int j = 0; j <= count; j++){
			try{
				leaves[i + 90 * j].GetComponent<Renderer>().material.color = color;
				if(i < 30){leaves[i + 60 + 90 * j].GetComponent<Renderer>().material.color = color;}	//Dient dazu damit alle Blätter verändert werden
			}
			catch(Exception e){}
		}
	}

	void colorChangeComplete(Color color){					//Färbt alle Blätter ein und nicht nur ein Bereich
		for(int i = 0; i <= numberOfLeaves; i++){
			try{leaves[i].GetComponent<Renderer>().material.color = color;}
			catch(Exception e){}
		}
	}

	public void changePositionComplete(){					//Macht Blätter sichtbar und verändert ihre Position (durch Animation verändert)
		for(int j = 0; j <= numberOfLeaves; j++){
			try{
				leaves[j].GetComponent<Renderer>().enabled = true;
				leaves[j].transform.localPosition = positions[j];
			}
			catch(Exception e){}
		}
	}

	public void changeControlFlag(bool flag){				//Setzt changed Array zurück 
		for(int i = 0; i < 60; i++){
			changed[i] = flag;
		}
	}

	public void colorCheckComplete(Color color, int activeFlag = 1){	//Ruft je nach Flag colorChangeComplete oder changePositionComplete auf, oder dekativiert alle Blätter. Ändert cCheck Wert
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

	public void fallingLeavesStart(int angle){				//Startet Animation der Blätter zum abfallen

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

						StartCoroutine(coroutines[i]);	//Startet Coroutine
					}
				}
				if(!herbstAudioPlaying){
					herbstAudioPlaying = true;
					backgroundNoise.PlayOneShot(jahreszeiten[4], 0.05f);
					StartCoroutine(soundLeaves());
					Debug.Log("HALLO");
				}
			}
		}
	}

	public void changePosition(Color color, int angle, int startPoint = 0, bool deactiveFlag = true){	//Verändert je nach Flag, Position und Renderstatus der Blätter
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
						leaves[angle + 90 * j].GetComponent<Renderer>().enabled = deactiveFlag;
						if(angle <= 30){leaves[angle + 60 + 90 * j].GetComponent<Renderer>().enabled = deactiveFlag;}
					}
					catch(Exception e){}
				}
				changed[angle] = false;
			}
		}
	}

	public IEnumerator deactivateLeaves(int number){							//Coroutine damit Blätter nach Animation verschwinden
		yield return new WaitForSeconds(1.3f);								//Animation dauert 1s und deaktivierung erfolgt nach 1.3s
		changed[number] = true;
		for(int i = 0; i <= count; i++){
			try{
				leaves[number + 90 * i].GetComponent<Renderer>().enabled = false;
				if(number <= 30){leaves[number + 60 + 90 * i].GetComponent<Renderer>().enabled = false;}
			}
			catch(Exception e){}
		}
	}

	public IEnumerator soundLeaves(){							//Coroutine damit Blätter nach Animation verschwinden
		yield return new WaitForSeconds(3f);								//Animation dauert 1s und deaktivierung erfolgt nach 1.3s
		herbstAudioPlaying = false;
	}


	public void changeVolumne(int angle, AudioClip jahreszeit){
		if(!backgroundNoise.isPlaying){
			Debug.Log("NUN" + jahreszeit + " d");

			backgroundNoise.clip = jahreszeit;
			backgroundNoise.Play();
		}
		else{
			angle = (angle - 15) % 90;
			if(angle < 30){
				if(audioChanged){
					backgroundNoise.Pause();
					backgroundNoise.clip = jahreszeit;
					backgroundNoise.Play();
					audioChanged = false;
				}
				backgroundNoise.volume = 1f - 1f * ((angle % 30f) / 29f);
				//Debug.Log((1f - 1f * ((angle % 30f) / 29f)));
			}
			else if(angle >= 30){
				if(!audioChanged){
					backgroundNoise.Pause();
					backgroundNoise.clip = jahreszeit;
					backgroundNoise.Play();
					audioChanged = true;
				}
				backgroundNoise.volume = 1f * ((angle % 30f) / 29f);
			}
		}
	}

	public void audioChangeCall(int angle, AudioClip actual, AudioClip next){
		if((angle - 15) % 90 < 30){changeVolumne(angle, actual);}
		else{changeVolumne(angle, next);}
	}

	void Update(){
		tracked = TrackingsScript.tracked;
		if(tracked){
			if(!check){							//Wird beim ersten Erkennen des Baums aufgerufen. Füllt Arrays und Variablen
				//backgroundNoise = GameObject.GetComponent<AudioSource>();
				angle = (int)transform.eulerAngles.y;
				if(angle % 90 >= 15 && angle % 90 < 74){
					cCheck = true;
					if((angle - 15) % 90 < 30){audioChanged = true;}
				} //Prüft ob Baum in Jahreszeit oder Übergang ist 
				string tag;
				for(int i = 0; i < 60; i++){
					coroutines[i] = deactivateLeaves(i);		//Versieht Coroutinen mit ihrer Funktion 
				}
				for(int i = 1; i <= numberOfLeaves; i++){
					if(i < 10){tag = "blaetter.00" + i;}
					else if(i < 100){tag = "blaetter.0" + i;}
					else{tag = "blaetter." + i;}
					try{
						leaves[i] = GameObject.Find(tag);
						positions[i] = leaves[i].transform.localPosition;
					}
					catch(Exception e){}
				}
				snow = GameObject.Find("Schnee").GetComponent<Renderer>();
				check = true;
				audioCoroutine = soundLeaves();
			}

			if(check){
				angle = (int)transform.eulerAngles.y;		//Hohlt sich Y-Rotation des Markers
				Debug.Log(backgroundNoise.isPlaying + " : " + audioChanged);

				//Jahreszeiten und Übergänge (fast) immer gleich aufgebaut
				if(angle >= 345 || angle < 15){	//Frühling
					if(!cCheck){colorCheckComplete(fruehling, 0);} 		//Beim Eintritt in Jahreszeit werden mit der colorCheckComplete Funktion die Blätter richtig eingefärbt/(de)aktiviert
					if(audioChanged){audioChanged = false;}
					if(!backgroundNoise.isPlaying){changeVolumne(74, jahreszeiten[0]);}	//Falls beim Aufruf in Jahreszeit, startet die richtige Hintergrund Musik
					if((angle) % 30 >= 15 && !changed[59]){changeControlFlag(true);}	//Setzt beim Verlassen in Vorgänger Übergang das change Array auf false	
					else if((angle) % 30 < 15 && changed[59]){changeControlFlag(false);}	//Setzt beim Verlassen in Nachfolgendem Übergang das change Array auf true
				}										//Im Frühling Reihenfolge des change Arrays veränderung umgedreht durch Position am Wechsel von 345° auf 15°

				if(angle >= 15 && angle < 75){   //Frühling zu Sommer
					audioChangeCall(angle, jahreszeiten[0], jahreszeiten[1]);
					if(cCheck){colorCheckComplete(Color.black, 0);}				//Setzt cCheck zurück auf false
					if(previousAngle <= angle){changeColor(angle, sommer);}			//Wenn Vorwärts gedreht werden Blätter in den Farben/Aktivierungszustand der darauf folgenden Jahreszeit dargestellt
					else{changeColor(angle, fruehling, previousAngle);}			//Wenn Rückwärts gedreht werden Blätter in den Farben/Aktivierungszustand der darauf vorherigen Jahreszeit dargestellt
				}

				if(angle >= 75 && angle < 105){	//Sommer
					if(!cCheck){colorCheckComplete(sommer);}
					if(audioChanged){audioChanged = false;}
					if(!backgroundNoise.isPlaying){changeVolumne(74, jahreszeiten[1]);}	//Falls beim Aufruf in Jahreszeit, startet die richtige Hintergrund Musik
					if((angle - 15) % 30 > 15){changeControlFlag(false);}
					else if((angle - 15) % 30 < 15){changeControlFlag(true);}
				}


				if(angle >= 105 && angle < 165){   //Sommer zu Herbst
					audioChangeCall(angle, jahreszeiten[1], jahreszeiten[2]);
					if(cCheck){colorCheckComplete(Color.black, 0);}
					if(herbstCheck){herbstCheck = false;}					//Verändert herbstCheck
					if(previousAngle <= angle){changeColor(angle, herbst);}
					else{changeColor(angle, sommer, previousAngle);}
				}

				if(angle >= 165 && angle < 195){ //Herbst
					if(!cCheck){colorCheckComplete(herbst, 0);}
					if(audioChanged){audioChanged = false;}
					if(!backgroundNoise.isPlaying){changeVolumne(74, jahreszeiten[2]);}	//Falls beim Aufruf in Jahreszeit, startet die richtige Hintergrund Musik
					if(!herbstCheck){herbstCheck = true;} 					//Verändert herbstCheck
					if((angle - 15) % 30 > 15){changeControlFlag(false);}
					else if((angle - 15) % 30 < 15){changeControlFlag(true);}
				}

				if(angle >= 195 && angle < 255){   //Herbst zu Winter
					audioChangeCall(angle, jahreszeiten[2], jahreszeiten[3]);
					if(cCheck){colorCheckComplete(Color.black, 0);}
					if(previousAngle <= angle){
						fallingLeavesStart(angle);					//Startet beim Vorwärts drehen die Blätterfallanimation und die Coroutine
					}
					else{
						changePosition(herbst, angle, previousAngle, true);		//Reaktiviert beim Rückwärts drehen die Blätter
					}
				}

				if(angle >= 255 && angle < 285){ //Winter
					if(!cCheck){colorCheckComplete(fruehling, -1);				}
					if(audioChanged){audioChanged = false;}
					if(!backgroundNoise.isPlaying){changeVolumne(74, jahreszeiten[3]);}	//Falls beim Aufruf in Jahreszeit, startet die richtige Hintergrund Musik
					if((angle - 15) % 30 > 15){
						changeControlFlag(false);
						if(herbstCheck){						//Färbt Blätter in Farbe herbst ein
							colorChangeComplete(fruehling);				//Dient zur Prävention wenn Baum Rückwärts gedreht wird das Blätter die Falsche Farbe besitzen bei der Reaktivierung
							herbstCheck = false;
						}
					}
					else if((angle - 15) % 30 < 15){
						changeControlFlag(true);
						if(!herbstCheck){						//Färbt Blätter in Farbe fruehling ein
							colorChangeComplete(herbst);
							herbstCheck = true;
						}

					}
					if(!snow.enabled){snow.enabled = true;}					//Aktiviert Schneepartikeleffekt
				}

				if(angle >= 285 && angle < 345){ //Winter zu Frühling
					audioChangeCall(angle, jahreszeiten[3], jahreszeiten[0]);
					if(cCheck){colorCheckComplete(Color.black, 0);}
					if(previousAngle <= angle){
						changePosition(fruehling, angle);
						changeColor(angle, fruehling);}
					else{
						changePosition(Color.black, angle, previousAngle, false);
					}
				}

				if((angle < 254 || angle > 285) && snow.enabled){snow.enabled = false;}		//Deaktiviert Schneepartikeleffekt


				previousAngle = angle;								//Speichert aktuellen angle in previousAngle zum prüfen der Drehrichtung		
			}

		}
	}
}























