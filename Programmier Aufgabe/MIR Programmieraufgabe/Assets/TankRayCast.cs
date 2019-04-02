using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankRayCast : MonoBehaviour				//Raycast in Z-Richtung vom Panzer aus
{
	RaycastHit hit;
	Vector3 direction;
	bool wasHit; 							//Boolean ob "gehitted" werden kann

	// Start is called before the first frame update
	void Start(){
		
	}

	// Update is called once per frame
	void Update(){
		// Does the ray intersect any objects excluding the player layer
		direction = this.transform.position;
		if (Physics.Raycast(this.transform.position, this.transform.TransformDirection(direction), out hit, Mathf.Infinity))
		{
			if(hit.transform.name == "turm" && !wasHit){	//Hit nur wenn "Turm" Objekt getroffen und wasHit false ist
				TankScript.isShooting = true;		//schießen auslösen 
				wasHit = true;				//Verhindern eines erneuten Hiterkennens
				StartCoroutine(HitExit());		//Startet Coroutine um das "hitten" 5 Sekunden lang zu verhindern, damit Animationen richtig ablaufen, auch bei kleineren Bewegungen
			}
		}
		else
		{
			Debug.DrawRay(this.transform.localPosition, this.transform.TransformDirection(direction) * 10000, Color.white);
		}
	}
	IEnumerator HitExit(){
		yield return new WaitForSeconds(5f);
		wasHit = false;						//wasHit zurücksetzen
	}
}
