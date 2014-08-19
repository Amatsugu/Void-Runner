using UnityEngine;
using System.Collections;

public class Cloud : MonoBehaviour {

	private GameObject _player;
	private float _speed;

	void Start()
	{
		_player = GameObject.FindWithTag("Player");
		_speed = Random.Range(0.2f, 1f);
	}

	void Update()
	{
		Vector2 move = _player.rigidbody2D.velocity;
		move.x *= _speed;
		move.y *= _speed;
		move *= Time.deltaTime;
		transform.Translate(move.x, move.y, 0);
	}


}
