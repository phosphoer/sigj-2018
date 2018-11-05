using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Sandwich : MonoBehaviour, IPointerClickHandler
{

	public Renderer[] SandwichBits = new Renderer[0];
	public float CrumbEmitForceThreshold = .25f;
	public ParticleSystem CrumbsFX;
	public float CritterEatCoolDown = 2f;
  public int SandwichTotalHealth = 100;

  private int _sandwichclicks = 0;
  private bool _isClickingEnabled = false;

	bool cding = false;
	float timer = 0f;
	int sandwichLvl = -1;
	Rigidbody rb;

	void Awake(){
    _sandwichclicks = 0;
    rb = GetComponent<Rigidbody>();
	}

  public void SetIsSandwichClickable()
  {
    _isClickingEnabled = true;
  }

	// Use this for initialization
	void Start () {
		for(int i=0; i<SandwichBits.Length; i++) SandwichBits[i].enabled=false;
		SetSandwichLevel(0);
	}

	void Update(){
		if(cding && !_isClickingEnabled) {
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
		else {
      if (_isClickingEnabled) {
        // This will cleanup the sandwich and end lunch early
        GameStateManager.Instance.SetGameStage(GameStateManager.GameStage.Afternoon);
      }
      else {
        Destroy(gameObject);
      }
    }
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

  public void OnPointerClick(PointerEventData eventData)
  {
    if (_isClickingEnabled) {
      _sandwichclicks++;
      int newSandwidthLevel = System.Math.Max((_sandwichclicks * SandwichBits.Length) / SandwichTotalHealth, 0);
      SetSandwichLevel(newSandwidthLevel);
    }
  }
}
