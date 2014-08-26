using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public float speed = 1;
	public float accel = 100;
	public float jumpSpeed = 250;
	public int maxJumps = 2;
	public float particleSpawnRate = .5f;
	public GameObject particle;

	private float _distance;
	private Transform _thisTransform;
	private Rigidbody2D _thisRigidbody;
	private int _curJumps;
	private float _nextParticleSpawn;
	private ObjectPoolerWorld _greenParticlePool;
	private bool _isPaused = false;
	private float _prePausedTime = 0;
	private float _prePauseAngVel;
	private Vector2 _prePauseVel;

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
		_greenParticlePool = GameObject.Find("_GreenParticles").GetComponent<ObjectPoolerWorld>();
	}
	// Update is called once per frame
	void Update () 
	{
		if(_isPaused)
			return;
		//_thisTransform = new Vector2(_thisTransform.position.x, _thisTransform.position.y);
		if(_thisRigidbody.velocity.x > speed)
		{
			_thisRigidbody.velocity = new Vector2(speed, _thisRigidbody.velocity.y);
		}else
			_thisRigidbody.AddForce(new Vector2(accel,0));

		if(_curJumps < maxJumps)
		{
			if(Input.GetKeyDown(KeyCode.Space))
			{
				_thisRigidbody.AddForce(new Vector2(0,jumpSpeed));
				_curJumps++;
			}
		}
		//speed += Time.deltaTime;

	}

	public void Loop()
	{
		_distance += _thisTransform.position.x;
		_thisRigidbody.position = new Vector3(0, _thisTransform.position.y);
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		if(_isPaused)
			return;
		if(col.collider.tag == "Ground")
		{
			if(Mathf.RoundToInt(rigidbody2D.velocity.y) == 0)
				_curJumps = 0;
			if(_nextParticleSpawn < Time.time)
			{
				foreach(ContactPoint2D p in col.contacts)
				{
					Instantiate(particle, new Vector3(p.point.x, p.point.y), Quaternion.identity);
				}
				_nextParticleSpawn = Time.time + particleSpawnRate;
			}
		}
	}

	void OnCollisionStay2D(Collision2D col)
	{
		if(_isPaused)
			return;
		if(col.collider.tag == "Ground")
		{
			if(_nextParticleSpawn < Time.time)
			{
				foreach(ContactPoint2D p in col.contacts)
				{
					_greenParticlePool.Instantiate( new Vector3(p.point.x, p.point.y), Quaternion.identity);
				}
				_nextParticleSpawn = Time.time + particleSpawnRate;
			}
		}
	}
}
