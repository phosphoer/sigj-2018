using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sandwich : MonoBehaviour {

	public Renderer[] SandwichBits = new Renderer[0];
	public float CrumbEmitForceThreshold = .25f;
	public ParticleSystem CrumbsFX;

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


	public void SetSandwichLevel(int newLvl){
		if(sandwichLvl>0) SandwichBits[sandwichLvl].enabled = false;

		sandwichLvl = newLvl;
		SandwichBits[sandwichLvl].enabled = true;
		CrumbsFX.Play();
	}

	void OnCollisionEnter(){
		if(rb.velocity.magnitude >= CrumbEmitForceThreshold){
			CrumbsFX.Play();
		}
	}
}
