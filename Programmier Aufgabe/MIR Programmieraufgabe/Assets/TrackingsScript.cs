using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class TrackingsScript : MonoBehaviour, ITrackableEventHandler {

	private TrackableBehaviour mTrackableBehaviour;
	public static bool tracked = false;

	// Use this for initialization
	void Start () {
		mTrackableBehaviour = this.GetComponent<TrackableBehaviour>();

		if (mTrackableBehaviour)
		{
			mTrackableBehaviour.RegisterTrackableEventHandler(this);
		} 

	}

	// Update is called once per frame
	void Update () {
		//Debug.Log(tracked);
	}

	void ITrackableEventHandler.OnTrackableStateChanged (TrackableBehaviour.Status prevStatus,
		TrackableBehaviour.Status newStatus){
		if (newStatus == TrackableBehaviour.Status.DETECTED || newStatus == TrackableBehaviour.Status.TRACKED || newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED){
			tracked = true; //Marker ist Detected
		}
		else if (prevStatus == TrackableBehaviour.Status.TRACKED && newStatus == TrackableBehaviour.Status.NO_POSE){
			tracked = false; //Marker verloren
		}
		Debug.Log(newStatus);
	}


}