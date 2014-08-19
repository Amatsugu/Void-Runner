using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RandomSprite : MonoBehaviour {

	public List<Sprite> sprites =  new List<Sprite>();
	private SpriteRenderer _renderer;

	void Start () 
	{
		_renderer = GetComponent<SpriteRenderer>();
		ReAssignSprite();
	}

	void OnEnable()
	{
		if(_renderer == null)
			_renderer = GetComponent<SpriteRenderer>();
		ReAssignSprite();
	}

	void ReAssignSprite()
	{
		_renderer.sprite = sprites[Random.Range(0,sprites.Count)];
	}
}
