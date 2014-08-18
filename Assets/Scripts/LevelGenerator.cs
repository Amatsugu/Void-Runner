using UnityEngine;
using System.Collections;

public class LevelGenerator : MonoBehaviour {
	public float width;
	public float startPos;
	public GameObject ground;

	public bool reGen = false;

	void Start () 
	{
		for(int i = 0; i < 64; i++)
		{
			GameObject obj = Instantiate(ground, new Vector3((i * width)+startPos, 0), Quaternion.identity) as GameObject;
			obj.GetComponent<SpriteRenderer>();
		}
	}

	void Update()
	{
		if(!reGen)
			return;
		reGen = false;
		Start();
	}

}
