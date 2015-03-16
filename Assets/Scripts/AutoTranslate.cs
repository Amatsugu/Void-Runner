using UnityEngine;
using System.Collections;

public class AutoTranslate : MonoBehaviour {

	public Vector3 moveVector;
	public bool useRigidbody;

	void Start()
	{
		if(useRigidbody)
			GetComponent<Rigidbody2D>().velocity = moveVector;
	}

	void Update () 
	{
		if(!useRigidbody)
			transform.Translate(moveVector*Time.time);
	}
}
