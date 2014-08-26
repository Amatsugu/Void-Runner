using UnityEngine;
using System.Collections;

public class Particle : MonoBehaviour {

	public float lifeTime = 3;
	public Vector2 initialVelMin;
	public Vector2 initialVelMax;

	private bool _isPaused;
	private float _prePausedTime;
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
		if(_isPaused)
			return;
		if(Time.time >= _killAt)
		{
			gameObject.SetActive(false);
		}
	}

	public void Pause()
	{
		_isPaused = true;
		_prePausedTime = Time.time;
	}

	public void UnPause()
	{
		_isPaused = false;
		_killAt += Time.time - _prePausedTime;
	}
}
