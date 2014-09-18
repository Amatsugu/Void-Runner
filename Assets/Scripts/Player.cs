using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

	public float startSpeed = 1;
	public float accel = 100;
	public float jumpSpeed = 250;
	public int maxJumps = 2;
	public float particleSpawnRate = .5f;
	public GameObject particle;
	public float burstParticeCount = 50;
	public RectTransform healthBar;
	public Vector2[] particleInitialFoces;

	private float _distance;
	private float _curSpeed;
	private Transform _thisTransform;
	private Rigidbody2D _thisRigidbody;
	private int _curJumps;
	private float _nextParticleSpawn;
	private ObjectPoolerWorld _greenParticlePool;
	private ObjectPoolerWorld _whiteParticlePool;
	private ObjectPoolerWorld _redParticlePool;
	private bool _isPaused = false;
	private float _prePausedTime = 0;
	private float _prePauseAngVel;
	private Vector2 _prePauseVel;
	private float _curHealth = 100;
	private bool _willImpact = false;

	public void Pause()
	{
		_isPaused = true;
		_prePausedTime = Time.time;
		_prePauseVel = _thisRigidbody.velocity;
		_prePauseAngVel = _thisRigidbody.angularVelocity;
		_thisRigidbody.velocity = Vector2.zero;
		_thisRigidbody.angularVelocity = 0;
		_thisRigidbody.isKinematic = true;
	}

	public void UnPause()
	{
		_isPaused = false;
		_nextParticleSpawn += Time.time - _prePausedTime;
		_thisRigidbody.isKinematic = false;
		_thisRigidbody.velocity = _prePauseVel;
		_thisRigidbody.angularVelocity = _prePauseAngVel;
	}

	void Start()
	{
		_thisRigidbody = rigidbody2D;
		_thisTransform = transform;
		_curSpeed = startSpeed;
		_greenParticlePool = GameObject.Find("_GreenParticles").GetComponent<ObjectPoolerWorld>();
		_whiteParticlePool = GameObject.Find("_WhiteParticles").GetComponent<ObjectPoolerWorld>();
		_redParticlePool = GameObject.Find("_RedParticles").GetComponent<ObjectPoolerWorld>();
	}
	// Update is called once per frame
	void Update () 
	{
		if(_isPaused)
			return;
		//_thisTransform = new Vector2(_thisTransform.position.x, _thisTransform.position.y);
		_thisRigidbody.velocity = new Vector2(_curSpeed, _thisRigidbody.velocity.y);


		if(_curJumps < maxJumps)
		{
			if(Input.GetKeyDown(KeyCode.Space))
			{
				_thisRigidbody.velocity = new Vector2(_thisRigidbody.velocity.x, jumpSpeed);
				if(_curJumps > 0)
				{
					Vector2 min = particleInitialFoces[4];
					min.x += _thisRigidbody.velocity.x;
					Vector2 max = particleInitialFoces[5];
					max.x += _thisRigidbody.velocity.x;
					for(int i = 0; i < burstParticeCount; i++)
					{
						GameObject particle = _whiteParticlePool.Instantiate(new Vector3(_thisTransform.position.x, _thisTransform.position.y-.4f), Quaternion.identity);
						particle.GetComponent<Particle>().SetVel(min, max);
					}
					if(_willImpact)
						_willImpact = false;
				}
				_curJumps++;
			}
		}
		if(_curJumps > 0 && !_willImpact)
		{
			if(Input.GetKeyDown(KeyCode.S))
			{
				_thisRigidbody.velocity = new Vector2(_thisRigidbody.velocity.x, -2*jumpSpeed);
				_willImpact = true;
			}
		}
		Vector3 scale = healthBar.localScale;
		healthBar.localScale = new Vector3(Mathf.Lerp(scale.x, Screen.width * (_curHealth/100), Time.deltaTime*5f), scale.y, scale.z);
		_curSpeed += accel * Time.deltaTime;
		//Debug.Log(_thisRigidbody.velocity);
	}

	public void Loop()
	{
		_distance += _thisTransform.position.x;
		_thisRigidbody.position = new Vector3(0, _thisTransform.position.y);
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.tag == "Spikes")
		{
			_curHealth -= 10;
		}
		if(_curHealth < 0)
			_curHealth = 0;
	}

	void OnTriggerStay2D(Collider2D col)
	{
		if(col.tag == "Spikes")
		{
			float healthLoss = 10 * Time.deltaTime;
			_curHealth -= healthLoss;
			Debug.Log(Mathf.CeilToInt(healthLoss));
			for(int i = 0; i < Mathf.CeilToInt(healthLoss); i++)
			{
				Vector2 min = particleInitialFoces[0];
				min.x += _thisRigidbody.velocity.x;
				Vector2 max = particleInitialFoces[1];
				max.x += _thisRigidbody.velocity.x;
				GameObject particle = _redParticlePool.Instantiate( new Vector3(_thisTransform.position.x, _thisTransform.position.y-.4f), Quaternion.identity);
				particle.GetComponent<Particle>().SetVel(min, max);
			}
		}
		if(_curHealth < 0)
			_curHealth = 0;
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		if(_isPaused)
			return;

		if(col.collider.tag == "Ground")
		{
			if(Mathf.RoundToInt(rigidbody2D.velocity.y) == 0)
				_curJumps = 0;
			if(!_willImpact)
				return;
			_willImpact = false;
			Vector2 min = particleInitialFoces[0];
			min.x += _thisRigidbody.velocity.x;
			Vector2 max = particleInitialFoces[1];
			max.x += _thisRigidbody.velocity.x;
			for(int i = 0; i < burstParticeCount; i++)
			{
				GameObject particle = _greenParticlePool.Instantiate(new Vector3(_thisTransform.position.x, _thisTransform.position.y-.5f), Quaternion.identity);
				particle.GetComponent<Particle>().SetVel(min, max);
			}
		}
		//Debug.Log("Impact");
	}

	void OnCollisionStay2D(Collision2D col)
	{
		if(_isPaused)
			return;
		if(col.collider.tag == "Ground")
		{
			if(_nextParticleSpawn < Time.time)
			{
				Vector2 min = particleInitialFoces[2];
				min.x += _thisRigidbody.velocity.x;
				Vector2 max = particleInitialFoces[3];
				max.x += _thisRigidbody.velocity.x;
				foreach(ContactPoint2D p in col.contacts)
				{
					GameObject particle = _greenParticlePool.Instantiate( new Vector3(p.point.x, p.point.y), Quaternion.identity);
					particle.GetComponent<Particle>().SetVel(min, max);
				}
				_nextParticleSpawn = Time.time + particleSpawnRate;
			}
		}
	}
}
