using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtoAttacher : MonoBehaviour {

	public GameObject[] Attachments = new GameObject[0];
	public Vector2 ScaleMinMax = new Vector2(1.2f,.8f);
	public Vector2 RotateMinMax = new Vector2(0f,90f);


	List<ProtoAttachPoint> points = new List<ProtoAttachPoint>();
	List<GameObject> attachments = new List<GameObject>();

	// Use this for initialization
	void Start () {

		PlaceNewAttachments();

	}


	public void PlaceNewAttachments(){

		if(attachments.Count>0){
			for(int i=0; i<attachments.Count; i++){
				Destroy(attachments[i]);
			}
			attachments.Clear();
		}

		points.Clear();
		points.AddRange(transform.GetComponentsInChildren<ProtoAttachPoint>(false));

		for(int i=0; i<points.Count; i++){
			int choice = Random.Range(0,Attachments.Length-1);
			GameObject temp = Instantiate(Attachments[choice],points[i].transform) as GameObject	;
			temp.transform.localPosition = Vector3.zero;
			temp.transform.localEulerAngles = new Vector3(0f,0f,Random.Range(RotateMinMax.x, RotateMinMax.y));
			temp.transform.localScale  *= Random.Range(ScaleMinMax.x,ScaleMinMax.y);
			attachments.Add(temp);
		}
	}

	// Update is called once per frame
	void Update () {
		
	}
}
