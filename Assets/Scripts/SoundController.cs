using UnityEngine;
using System.Collections;

[RequireComponent (typeof (AudioSource))]
public class SoundController : MonoBehaviour {

	public Sound SoundType;
	public AudioClip soundClip;
	public bool willPause;
	public bool loop;
	public bool autoPlay;
	public bool stopOnDestroy;

	private AudioSource _as;
	// Use this for initialization
	void Awake () 
	{
		_as = GetComponent<AudioSource>();
		Init();
		_as.clip = soundClip;
		_as.playOnAwake = autoPlay;
		_as.loop = loop;
	}

	void OnEnable()
	{
		Awake();
	}

	void OnDisable()
	{
		if(stopOnDestroy)
			Stop();
	}

	void Init()
	{
		if(SoundType == Sound.Music)
			_as.volume = GameRegistry.MUSIC_VOL;
		else if(SoundType == Sound.Effect)
			_as.volume = GameRegistry.EFFECT_VOL;
	}



	public void Stop()
	{
		_as.Stop();
	}

	public void Play()
	{
		_as.Play();
	}

	public void PlayOnce()
	{
		if(SoundType == Sound.Effect)
			_as.PlayOneShot(soundClip, GameRegistry.EFFECT_VOL);
		else if(SoundType == Sound.Music)
			_as.PlayOneShot(soundClip, GameRegistry.MUSIC_VOL);
	}

	public void Play(float delay)
	{
		_as.PlayDelayed(delay);
	}

	public void Pause()
	{
		_as.Pause();
	}

	public void UnPause()
	{
		Play();
	}
	
	public void UpdateVolumes()
	{
		Init();
	}
}
