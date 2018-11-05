using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidBodySFX : MonoBehaviour {

	public SoundBank SFX;
	public float VelocityCuttoff = 1f;
	public float SFXCoolDown = .5f;

	float cdTimer = 0f;
	bool cding = false;

	
	// Update is called once per frame
	void Update () {
		if(cding){
			cdTimer+=Time.deltaTime;
			if(cdTimer>=SFXCoolDown){
				cdTimer = 0f;
				cding = false;
			}
		}
	}

	void OnCollisionEnter(Collision col){

		//Debug.LogWarning(col.impulse.magnitude);

		if(!cding){
			if(col.impulse.magnitude>=VelocityCuttoff){
				cding = true;
				AudioManager.QueuePlaySoundRoutine(gameObject,SFX);
			}
		}
	}
}
