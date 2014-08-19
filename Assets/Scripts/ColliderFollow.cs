using UnityEngine;
using System.Collections;

public class ColliderFollow : MonoBehaviour {
	
	public Transform objectToFollow;
	
	void Update()
	{
		if(objectToFollow == null)
			return;

		BoxCollider2D col = GetComponent<BoxCollider2D>();
		Vector2 cen = col.center;
		cen.x = objectToFollow.position.x;
		col.center = cen;
	}
	
}
