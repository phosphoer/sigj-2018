using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingEmission : MonoBehaviour {
	
	Renderer renderer;
    Material mat;
	
	public float Speed;
	public Color baseColor;	

	// Use this for initialization
	void Start () {
		renderer = GetComponent<Renderer> ();
		mat = renderer.material;
	}
	
 void Update () {
 
         float emission = Mathf.PingPong (Time.time * Speed, 1.0f); 
         Color finalColor = baseColor * Mathf.LinearToGammaSpace (emission);
         mat.SetColor ("_EmissionColor", finalColor);
     }
}