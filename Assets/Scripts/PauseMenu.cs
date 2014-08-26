using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {

	private bool _isPaused;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKeyUp(KeyCode.Escape))
		{

		}
	}

	public void PauseControl()
	{
		_isPaused = !_isPaused;
		GameObject[] objects = GameObject.FindObjectsOfType<GameObject>();
		foreach(GameObject g in objects)
		{
			if(g == gameObject)
				continue;
			if(!_isPaused)
				g.SendMessage("Pause", SendMessageOptions.DontRequireReceiver);
			else
				g.SendMessage("UnPause", SendMessageOptions.DontRequireReceiver);
		}
	}
}
