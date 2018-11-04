using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateConstantly : MonoBehaviour {

public float xspeed = 0;
public float yspeed = 50;
public float zspeed = 0;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		transform.Rotate (xspeed,yspeed,zspeed*Time.deltaTime);
	}
}
