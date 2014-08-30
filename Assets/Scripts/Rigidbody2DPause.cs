using UnityEngine;
using System.Collections;

public class Rigidbody2DPause : MonoBehaviour {

	public bool isEnabled = true;

	//private bool _isPaused = false;
	private float _prePauseAngVel;
	private Vector2 _prePauseVel;
	private Rigidbody2D _thisRigidbody;

	void Start()
	{
		if(rigidbody2D == null)
		{
			enabled = false;
			return;
		}
		_thisRigidbody = rigidbody2D;
	}

	public void Pause()
	{
		if(!isEnabled)
			return;
		//_isPaused = true;
		_prePauseVel = _thisRigidbody.velocity;
		_prePauseAngVel = _thisRigidbody.angularVelocity;
		_thisRigidbody.velocity = Vector2.zero;
		_thisRigidbody.angularVelocity = 0;
		_thisRigidbody.isKinematic = true;
	}
	
	public void UnPause()
	{
		if(!isEnabled)
			return;
		//_isPaused = false;
		_thisRigidbody.isKinematic = false;
		_thisRigidbody.velocity = _prePauseVel;
		_thisRigidbody.angularVelocity = _prePauseAngVel;
	}
}
