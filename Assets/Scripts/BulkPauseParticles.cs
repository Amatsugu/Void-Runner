using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulkPauseParticles : MonoBehaviour {

	public List<ParticleSystem> systems = new List<ParticleSystem>();

	public void Pause()
	{
		foreach(ParticleSystem p in systems)
		{
			p.Pause();
		}
	}

	public void UnPause()
	{
		foreach(ParticleSystem p in systems)
		{
			p.Play();
		}
	}
}
