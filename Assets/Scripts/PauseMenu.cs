using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {

	public GameObject pauseMenu;
	public GameObject optionsMenu;

	private bool _isPaused;
	private bool _isOptions;
	private CanvasRenderer _cr;
	private float _musicVol;
	private float _effectVol;

	// Use this for initialization
	void Start () 
	{
		_cr = pauseMenu.GetComponent<CanvasRenderer>();
		pauseMenu.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKeyUp(KeyCode.Escape))
		{
			PauseControl();
		}
	}

	public void PauseControl()
	{
		if(_isOptions)
		{
			_isOptions = false;
			return;
		}
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
		pauseMenu.SetActive(!_isPaused);
		optionsMenu.SetActive(false);
	}

	public void ToggleOptions()
	{
		_isOptions = _isOptions;
		optionsMenu.SetActive(!_isOptions);
	}

	public void Quit()
	{

	}

	public void SetEffectVol(float vol)
	{
		_effectVol = vol;
	}

	public void SetMusicVol(float vol)
	{
		_musicVol = vol;
	}
}
