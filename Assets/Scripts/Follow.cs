using UnityEngine;
using System.Collections;

public class Follow : MonoBehaviour {

	public Transform objectToFollow;

	void Update()
	{
		if(objectToFollow == null)
			return;
		Vector3 pos = transform.position;
		pos.x = objectToFollow.position.x;
		transform.position = pos;
	}

}
