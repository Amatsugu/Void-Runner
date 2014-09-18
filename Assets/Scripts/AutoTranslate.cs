using UnityEngine;
using System.Collections;

public class AutoTranslate : MonoBehaviour {

	public Vector3 moveVector;
	public bool useRigidbody;

	void Start()
	{
		if(useRigidbody)
			rigidbody2D.velocity = moveVector;
	}

	void Update () 
	{
		if(!useRigidbody)
			transform.Translate(moveVector*Time.time);
	}
}
