using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

	public Animator anim;
	public float startSpeed = 1;
	public float accel = 100;
	public float jumpSpeed = 250;
	public int maxJumps = 2;
	public RectTransform healthBar;
	public ParticleSystem impactBurstParticles;
	public ParticleSystem damgeParicles;
	public ParticleSystem groundParticles;
	public ParticleSystem jumpParticles;
	public Text distanceDisplay;

	private float _distance;
	private float _curSpeed;
	private Rigidbody2D _thisRigidbody;
	private Transform _thisTransform;
	private int _curJumps;
	private bool _isPaused = false;
	private float _prePauseAngVel;
	private Vector2 _prePauseVel;
	private float _curHealth = 100;
	private bool _willImpact = false;
	private bool _isGrounded = false;
	public ParticleSystem.Particle[] _particleBuffer;


	//Initializations
	void Start()
	{
		_thisRigidbody = GetComponent<Rigidbody2D>();
		_thisTransform = transform;
		_curSpeed = startSpeed;
		_particleBuffer = new ParticleSystem.Particle[groundParticles.maxParticles];
	}

	//When the game pauses
	public void Pause()
	{
		_isPaused = true;
		_prePauseVel = _thisRigidbody.velocity;
		_prePauseAngVel = _thisRigidbody.angularVelocity;
		_thisRigidbody.velocity = Vector2.zero;
		_thisRigidbody.angularVelocity = 0;
		_thisRigidbody.isKinematic = true;
		damgeParicles.Pause();
		impactBurstParticles.Pause();
		groundParticles.Pause();
		anim.speed = 0;
	}

	//When the game unpauses
	public void UnPause()
	{
		_isPaused = false;
		_thisRigidbody.isKinematic = false;
		_thisRigidbody.velocity = _prePauseVel;
		_thisRigidbody.angularVelocity = _prePauseAngVel;
		damgeParicles.Play();
		impactBurstParticles.Play();
		groundParticles.Play();
		anim.speed = _curSpeed/startSpeed;
	}

	// Update is called once per frame
	void Update () 
	{
		if(_isPaused)
			return;
		_thisRigidbody.velocity = new Vector2(_curSpeed, _thisRigidbody.velocity.y);


		ReadInputs();
		UpdateHUD();
		UpdateAnimations();
		_curSpeed += accel * Time.deltaTime;
		distanceDisplay.text = VoidUtils.Round(_thisTransform.position.x + _distance, 100) + "m";
	}

	//Process animations
	void UpdateAnimations()
	{
		if (anim != null)
		{
			anim.ResetTrigger("Jump");
			anim.SetFloat("VerticalVel", _thisRigidbody.velocity.y);
			anim.SetBool("isGrounded", _isGrounded);
			anim.speed = _curSpeed / startSpeed;
		}
	}

	//Read player inputs
	void ReadInputs()
	{
		if(Application.isMobilePlatform)
		{
			Touch t = Input.GetTouch(0);
			if(t.phase == TouchPhase.Began && t.deltaPosition.y >= 0)
			{
				Jump();
			}
			if(t.phase == TouchPhase.Moved && t.deltaPosition.y < -5)
			{
				Stomp();
			}
		}else
		{ 
			if (Input.GetKeyDown(KeyCode.Space))
			{
				Jump();
			}
			if (Input.GetKeyDown(KeyCode.S))
			{
				Stomp();
			}
		}
	}

	//Change values displayed on the HUD
	void UpdateHUD()
	{
		Vector3 scale = healthBar.localScale;
		healthBar.localScale = new Vector3(Mathf.Lerp(scale.x, Screen.width * (_curHealth / 100), Time.deltaTime * 5f), scale.y, scale.z);
	}


	//Causes the player to stomp to the ground
	void Stomp()
	{
		if (_curJumps > 0 && !_willImpact)
		{
			_thisRigidbody.velocity = new Vector2(_thisRigidbody.velocity.x, -2 * jumpSpeed);
			_willImpact = true;
		}
	}

	//Causes to the player to jump
	void Jump()
	{
		if (_curJumps < maxJumps)
		{
			_thisRigidbody.velocity = new Vector2(_thisRigidbody.velocity.x, jumpSpeed);
			if (_curJumps > 0)
				if (_willImpact)
					_willImpact = false;
			_curJumps++;
			_isGrounded = false;
			anim.SetTrigger("Jump");
			//anim.StopPlayback();
			anim.Play("Jump");
			jumpParticles.Play();
		}
	}

	//Register player enter/stay/exit triggers
	void OnTriggerEnter2D(Collider2D col)
	{
		if(_isPaused)
			return;
		if(col.tag == "Spikes")
		{
			_curHealth -= 10;
			damgeParicles.Play();
		}
		if(_curHealth < 0)
			_curHealth = 0;
	}

	void OnTriggerExit2D(Collider2D col)
	{
		if (_isPaused)
			return;
		if (col.tag == "Spikes")
			damgeParicles.Stop();
	}

	void OnTriggerStay2D(Collider2D col)
	{
		if(_isPaused)
			return;
		if(col.tag == "Spikes")
		{
			float healthLoss = 10 * Time.deltaTime;
			_curHealth -= healthLoss;
		}
		if(_curHealth < 0)
			_curHealth = 0;
	}

	//Register player enter/exit colliders
	void OnCollisionEnter2D(Collision2D col)
	{
		if(_isPaused)
			return;
		if (col.collider.tag == "Ground")
		{
			if (Mathf.RoundToInt(GetComponent<Rigidbody2D>().velocity.y) == 0)
			{
				_curJumps = 0;
				_isGrounded = true;
			}
			anim.Play("Run");
			//Enable gound particles
			groundParticles.Play();
			
			//Tiger Impact Particles
			if (!_willImpact)
				return;
			_willImpact = false;
			impactBurstParticles.Play();
		}
	}

	void OnCollisionExit2D(Collision2D col)
	{
		if (_isPaused)
			return;
		if(col.collider.tag == "Ground")
		{
			//Disable ground particles
			groundParticles.Stop();
		}
	}


	public void Loop()
	{
		Vector3 pos = _thisTransform.position;
		_distance += pos.x;
		float curX = Mathf.Floor(_thisTransform.position.x);
		pos.x = 0;
		transform.position = pos;
		//Particles
		//Burst
		int l = impactBurstParticles.GetParticles(_particleBuffer);
		ShiftParticles(curX, l);
		impactBurstParticles.SetParticles(_particleBuffer, l);
		//Ground
		l = groundParticles.GetParticles(_particleBuffer);
		ShiftParticles(curX, l);
		groundParticles.SetParticles(_particleBuffer, l);
		//Damage
		l = damgeParicles.GetParticles(_particleBuffer);
		ShiftParticles(curX, l);
		damgeParicles.SetParticles(_particleBuffer, l);
		//Jump
		l = jumpParticles.GetParticles(_particleBuffer);
		ShiftParticles(curX, l);
		jumpParticles.SetParticles(_particleBuffer, l);
	}

	void ShiftParticles(float curX, int l)
	{
		for (int i = 0; i < l; i++)
		{
			Vector3 newPos = _particleBuffer[i].position;
			newPos.x = (newPos.x - curX);
			_particleBuffer[i].position = newPos;
		}
	}
}
