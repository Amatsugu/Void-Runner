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
		GetComponent<Rigidbody2D>().AddForce(vel, ForceMode2D.Impulse);
		_killAt = Time.time + lifeTime;
	}

	void OnEnable()
	{
		//Debug.Log("Enabled");
		GetComponent<Rigidbody2D>().isKinematic = false;
	}

	void OnDisable()
	{
		GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		GetComponent<Rigidbody2D>().angularVelocity = 0;
		GetComponent<Rigidbody2D>().isKinematic = true;
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

	public void Init(Color col, int layer, int order)
	{
		SpriteRenderer r = GetComponent<SpriteRenderer>();
		r.color = col;
		r.sortingLayerID = layer;
		r.sortingOrder = order;
	}

	public void Init(Color col, int layer, int order, Vector2 minVel, Vector2 maxVel)
	{
		SpriteRenderer r = GetComponent<SpriteRenderer>();
		r.color = col;
		r.sortingLayerID = layer;
		r.sortingOrder = order;
		SetVel(minVel, maxVel);
	}

	public void Init(Color col, string layer, int order)
	{
		SpriteRenderer r = GetComponent<SpriteRenderer>();
		r.color = col;
		r.sortingLayerName = layer;
		r.sortingOrder = order;
	}
	
	public void Init(Color col, string layer, int order, Vector2 minVel, Vector2 maxVel)
	{
		SpriteRenderer r = GetComponent<SpriteRenderer>();
		r.color = col;
		r.sortingLayerName = layer;
		r.sortingOrder = order;
		SetVel(minVel, maxVel);
	}

	public void SetVel(Vector2 min, Vector2 max)
	{
		initialVelMin = min;
		initialVelMax = max;
		GetComponent<Rigidbody2D>().isKinematic = false;
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
