using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankRayCast : MonoBehaviour
{
	RaycastHit hit;
	Vector3 direction;
	bool wasHit; 

	// Start is called before the first frame update
	void Start(){
		
	}

	// Update is called once per frame
	void Update(){
		// Does the ray intersect any objects excluding the player layer
		direction = this.transform.position;
		if (Physics.Raycast(this.transform.position, this.transform.TransformDirection(direction), out hit, Mathf.Infinity))
		{
			if(hit.transform.name == "turm" && !wasHit){
				Debug.Log("WICHTIG");
				TankScript.isShooting = true;
				wasHit = true;
				StartCoroutine(HitExit());
			}
		}
		else
		{
			Debug.DrawRay(this.transform.localPosition, this.transform.TransformDirection(direction) * 10000, Color.white);
			Debug.Log("Did not Hit");
			//wasHit = false;
		}
	}
	IEnumerator HitExit(){
		yield return new WaitForSeconds(5f);
		wasHit = false;
	}
}
