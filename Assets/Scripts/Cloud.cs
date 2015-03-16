using UnityEngine;
using System.Collections;

public class Cloud : MonoBehaviour {

	public float yMin = 4;

	private GameObject _player;
	private float _speed;
	private Rigidbody2D _thisRigidbody;
	private bool _isPaused = false;
	//private float _prePausedTime = 0;
	private float _prePauseAngVel;
	private Vector2 _prePauseVel;
	
	public void Pause()
	{
		_isPaused = true;
		//_prePausedTime = Time.time;
		_prePauseVel = _thisRigidbody.velocity;
		_prePauseAngVel = _thisRigidbody.angularVelocity;
		_thisRigidbody.velocity = Vector2.zero;
		_thisRigidbody.angularVelocity = 0;
		_thisRigidbody.isKinematic = true;
	}
	
	public void UnPause()
	{
		_isPaused = false;
		_thisRigidbody.isKinematic = false;
		_thisRigidbody.velocity = _prePauseVel;
		_thisRigidbody.angularVelocity = _prePauseAngVel;
	}


	void Start()
	{
		_player = GameObject.FindWithTag("Player");
		_speed = Random.Range(0.65f, 0.9f);
		_thisRigidbody = GetComponent<Rigidbody2D>();
	}

	void OnEnable()
	{
		Start();
	}

	void Update()
	{
		if(_isPaused)
			return;
		if(_player == null)
			Start();
		Vector2 vel = _player.GetComponent<Rigidbody2D>().velocity;
		vel *= _speed;
		//vel.y *= _speed;
		//if(_thisTransform.position.y <= yMin)
		vel.y = 0;
		_thisRigidbody.velocity = vel;
	}


}
