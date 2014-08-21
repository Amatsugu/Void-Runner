using UnityEngine;
using System.Collections;

public class Particle : MonoBehaviour {

	public float lifeTime = 3;
	public Vector2 initialVelMin;
	public Vector2 initialVelMax;

	private float _killAt;
	// Use this for initialization
	void Start () 
	{
		rigidbody2D.velocity = Vector2.zero;
		rigidbody2D.angularVelocity = 0;
		Vector2 vel = new Vector2(Random.Range(initialVelMin.x, initialVelMax.x), Random.Range(initialVelMin.y, initialVelMax.y));
		rigidbody2D.AddForce(vel);
		_killAt = Time.time + lifeTime;
	}

	void OnEnable()
	{
		Start();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Time.time >= _killAt)
		{
			gameObject.SetActive(false);
		}
	}
}
