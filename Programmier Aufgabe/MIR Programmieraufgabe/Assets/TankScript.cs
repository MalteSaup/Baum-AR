using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankScript : MonoBehaviour{

	Animator anim;
	int shootHash = Animator.StringToHash("shoot");
	int collapsHash = Animator.StringToHash("collaps");

	int rot;    
	float up;		//Bewegung auf der Z-Achse
	float side;		//Bewegung auf der X-Achse
	float rotation_deg;	//wie weit im Moment gedreht		
	float speed = 0.005f;	//Tempo
	bool drehung; 
	bool tracked; 
	public static bool isShooting;

	int up_mul = 1;
	int side_mul;

	void Start(){
		anim = GetComponent<Animator>();
    	}

    // Update is called once per frame
	void Update(){
		tracked = TrackingsScript.tracked;
		if(!drehung && tracked){					//Positionsanpassung, nur wenn nicht in Drehung und Getracked
			up += speed * up_mul;				
			side += speed * side_mul;			
		}
		if(drehung && !isShooting){					//Drehung, nur wenn in Drehung und nicht geschossen wird
			rotation_deg += 0.9f;					//aktualisieren Drehstand
			if(rotation_deg < 90){
				this.transform.Rotate(0, 0.9f, 0);		//Drehung
			}
			else{
				drehung = false;				//Drehung beenden
				this.transform.Rotate(0, -(this.transform.localEulerAngles.y - rot), 0); //Evtl. Fehler auffangen. Panzer auf richtige Ausrichtung forcen
			}
		}
		if(isShooting && !anim.GetBool(shootHash)){			//Aktiviert Schießen einmal, dafür Abfrage des shoot Boolean Wertes welcher zu aktivierung der Animation genutzt wird
			anim.SetBool(shootHash, true);
			GameObject.Find("turm").GetComponent<Animator>().SetBool(collapsHash, true);	//Aktiviert die Zusammensturzanimation des Turms
			StartCoroutine(AnimationExit());			//Start der Coroutine
		}
		this.transform.localPosition = new Vector3(side, 0, up);	//aktualisieren der Position
	}

	void OnTriggerExit(Collider collider){					//Drehung bei verlassen des Colliders
		rotation_deg = 0;						//Aktuelle Drehung auf 0 setzen
		drehung = true;							//Drehung aktivieren
		rot = (rot + 90) % 360;						//Fahrtrichtung anpassen
		up_mul = (int)Mathf.Cos(rot * Mathf.PI / 180);			//up_mul Multiplikationswert für die Geschwindigkeit die up hinzugefügt wird, kann -1, 0 oder 1 sein
		side_mul = (int)Mathf.Sin(rot * Mathf.PI / 180);		//side_mul Multiplikationswert für die Geschwindigkeit die side hinzugefügt wird, kann -1, 0 oder 1 sein
	} 
		
	IEnumerator AnimationExit(){ 						//Coroutine für das Ausführen und beenden der Animationen
		yield return new WaitForSeconds(0.7f);
		isShooting = false;
		anim.SetBool(shootHash, false);

		yield return new WaitForSeconds(0.2f);
		GameObject.Find("turm").GetComponent<Animator>().SetBool(collapsHash, false);
	}
		
}
	
