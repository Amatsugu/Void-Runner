using UnityEngine;
using System.Collections;

public class Particle : MonoBehaviour {

	public float lifeTime = 3;
	public Vector2 initialVelMin;
	public Vector2 initialVelMax;
	public bool screenOcclude = true;

	private bool _isPaused;
	private float _prePausedTime;
	private float _killAt;
	// Use this for initialization
	void InitMovement () 
	{

		Vector2 vel = new Vector2(Random.Range(initialVelMin.x, initialVelMax.x), Random.Range(initialVelMin.y, initialVelMax.y));
		rigidbody2D.AddForce(vel, ForceMode2D.Impulse);
		_killAt = Time.time + lifeTime;
	}

	void OnEnable()
	{
		//Debug.Log("Enabled");
		rigidbody2D.isKinematic = false;
	}

	void OnDisable()
	{
		rigidbody2D.velocity = Vector2.zero;
		rigidbody2D.angularVelocity = 0;
		rigidbody2D.isKinematic = true;
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
		if(screenOcclude)
		{
			Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
			if(screenPos.x < 0 || screenPos.y < 0)
				gameObject.SetActive(false);
			if(screenPos.x > Screen.width || screenPos.y > Screen.height)
				gameObject.SetActive(false);
		}

	}

	public void SetVel(Vector2 min, Vector2 max)
	{
		initialVelMin = min;
		initialVelMax = max;
		rigidbody2D.isKinematic = false;
		InitMovement();
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
