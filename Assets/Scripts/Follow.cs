using UnityEngine;
using System.Collections;

public class Follow : MonoBehaviour {

	public Transform objectToFollow;
	public bool bothAxis = false;
	public Vector2 offset;

	void Update()
	{
		if(objectToFollow == null)
			return;
		Vector3 pos = transform.position;
		pos.x = objectToFollow.position.x + offset.x; 
		if(bothAxis)
			pos.y = objectToFollow.position.y + offset.y;
		transform.position = pos;
	}

}
