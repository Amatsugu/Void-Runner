using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public float speed = 1;
	public float accel = 100;
	public float jumpSpeed = 250;
	public float maxJumps = 2;

	private Vector2 thisTransform;
	
	// Update is called once per frame
	void Update () 
	{
		thisTransform = new Vector2(transform.position.x, transform.position.y);
		if(rigidbody2D.velocity.x > speed)
		{
			rigidbody2D.velocity = new Vector2(speed, rigidbody2D.velocity.y);
		}else
			rigidbody2D.AddForce(new Vector2(accel,0));

		Debug.DrawLine(transform.position, new Ray(transform.position, transform.up*-1).GetPoint(1f), Color.red);
		RaycastHit2D ray = Physics2D.Raycast(thisTransform, new Vector2(0,-1), 1f);
		//Debug.Log(ray.collider);
		if(ray.collider != collider)
		{
			if(Input.GetKeyDown(KeyCode.Space))
			{
				rigidbody2D.AddForce(new Vector2(0,jumpSpeed));
			}
		}

	}

	void OnCollisionStay2D(Collision2D col)
	{

	}
}
