using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankScript : MonoBehaviour{

	Animator anim;
	int shootHash = Animator.StringToHash("shoot");
	int collapsHash = Animator.StringToHash("collaps");

	int rot;    
	float up;
	float side;
	float te;
	float speed = 0.005f;
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
		Debug.Log(this.transform.localPosition);
		//Debug.Log(this);
		if(!drehung && tracked){
			up += speed * up_mul;
			side += speed * side_mul;
		}
		if(drehung && !isShooting){
			te += 0.9f;
			//Debug.Log(te);
			if(te < 90){
				this.transform.Rotate(0, 0.9f, 0);	
			}
			else{
				rot = (rot) % 360;
				drehung = false;
				this.transform.Rotate(0, -(this.transform.localEulerAngles.y - rot), 0);
			}
		}
		if(isShooting && !anim.GetBool(shootHash)){
			anim.SetBool(shootHash, true);
			GameObject.Find("turm").GetComponent<Animator>().SetBool(collapsHash, true);
			StartCoroutine(AnimationExit());
		}
		this.transform.localPosition = new Vector3(side, 0, up);
		//Debug.Log(side + " : " + up);
	}

	void OnTriggerExit(Collider collider){
		Debug.Log("nu");
		te = 0;
		drehung = true;
		rot = (rot + 90) % 360;
		up_mul = (int)Mathf.Cos(rot * Mathf.PI / 180);
		side_mul = (int)Mathf.Sin(rot * Mathf.PI / 180);
	} 
		
	IEnumerator AnimationExit(){
		yield return new WaitForSeconds(0.7f);
		Debug.Log("H");
		isShooting = false;
		anim.SetBool(shootHash, false);

		yield return new WaitForSeconds(0.2f);
		GameObject.Find("turm").GetComponent<Animator>().SetBool(collapsHash, false);
	}
		
}
	
