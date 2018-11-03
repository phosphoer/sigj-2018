using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttchmntAnimMngr : MonoBehaviour {

	public int AnimCount = 3;
	public Vector2 PostAnimDelay = new Vector2(.25f,1f);
	public Vector2 AdditionalRandomScale = new Vector2(1f,1f);

	Animator anim;
	bool delaying = false;
	float currentWait = 0f;
	float timer = 0f;

	void Awake(){
		transform.localScale *= Random.Range(AdditionalRandomScale.x,AdditionalRandomScale.y);
		anim = GetComponent<Animator>();
	}

	// Use this for initialization
	void Start () {
		currentWait = Random.Range(PostAnimDelay.x,PostAnimDelay.y);
	}
	
	// Update is called once per frame
	void Update () {
		if(delaying){
			timer+=Time.deltaTime;
			if(timer>=currentWait){
				delaying = false;
				ChooseNewAnim();
				timer = 0f;
			}
		}
		else{
			if(anim.GetCurrentAnimatorStateInfo(0).IsName("HubState")){
				delaying = true;
				currentWait = Random.Range(PostAnimDelay.x,PostAnimDelay.y);
			}
		}
	}

	public void ChooseNewAnim(){
		anim.SetInteger("AnimBranch",Random.Range(0,AnimCount));
	}
}
