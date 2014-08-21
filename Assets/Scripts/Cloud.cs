using UnityEngine;
using System.Collections;

public class Cloud : MonoBehaviour {

	public float yMin = 4;

	private GameObject _player;
	private float _speed;
	private Transform _thisTransform;

	void Start()
	{
		_player = GameObject.FindWithTag("Player");
		_speed = Random.Range(0.65f, 0.9f);
		_thisTransform = transform;
	}

	void OnEnable()
	{
		Start();
	}

	void Update()
	{
		if(_player == null)
			Start();
		Vector2 vel = _player.rigidbody2D.velocity;
		vel *= _speed;
		vel.y *= _speed;
		if(_thisTransform.position.y <= yMin)
			vel.y = 0;
		rigidbody2D.velocity = vel;
	}


}
