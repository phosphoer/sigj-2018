using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sandwich : MonoBehaviour {

	public Renderer[] SandwichBits = new Renderer[0];
	public float CrumbEmitForceThreshold = .25f;
	public ParticleSystem CrumbsFX;
	public float CritterEatCoolDown = 2f;

	bool cding = false;
	float timer = 0f;
	int sandwichLvl = -1;
	Rigidbody rb;

	void Awake(){
		rb = GetComponent<Rigidbody>();
	}

	// Use this for initialization
	void Start () {
		for(int i=0; i<SandwichBits.Length; i++) SandwichBits[i].enabled=false;
		SetSandwichLevel(0);
	}

	void Update(){
		if(cding){
			timer+=Time.deltaTime;
			if(timer>=CritterEatCoolDown){
				cding = false;
				timer = 0f;
			}
		}
	}

	public void SetSandwichLevel(int newLvl){
		CrumbsFX.Play();
		if(newLvl<SandwichBits.Length){
			if(sandwichLvl>=0) SandwichBits[sandwichLvl].enabled = false;
				
			sandwichLvl = newLvl;
			SandwichBits[sandwichLvl].enabled = true;
		}
		else Destroy(gameObject);
	}

	void OnCollisionEnter(Collision col){
		if(rb.velocity.magnitude >= CrumbEmitForceThreshold){
			CrumbsFX.Play();
		}

		CritterController hitCritter = col.transform.GetComponentInParent<CritterController>();
		//if(!hitCritter) Debug.LogWarning("No critter controller found on object: " + col.transform.parent.parent.name);

		if(!cding && hitCritter){
			//Debug.LogWarning("Critter fed!");
			SetSandwichLevel(sandwichLvl+1);
			cding = true;
		}
	}
}
