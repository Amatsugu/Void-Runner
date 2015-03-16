using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PauseMenu : MonoBehaviour {

	public GameObject pauseMenu;
	public Color dimColor = new Color(0,0,0,.5f);
	public float dimSpeed = 4;

	private bool _isPaused;
	private Image _cr;
	private OptionsMenu _options;
	// Use this for initialization
	void Start () 
	{
		_cr = pauseMenu.GetComponent<Image>();
		_options = GetComponent<OptionsMenu>();
		pauseMenu.SetActive(false);
		_options.LoadPrefs();
		_options.optionsMenu.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKeyUp(KeyCode.Escape))
		{
			PauseControl();
		}
		if(_isPaused)
			_cr.color = Color.Lerp(_cr.color, dimColor, Time.deltaTime * dimSpeed);
		else
			_cr.color = Color.clear;
	}

	public void PauseControl()
	{
		if(_options._isOptions)
		{
			_options._isOptions = false;
			_options.optionsMenu.SetActive(_options._isOptions);
			return;
		}else
		{
			_isPaused = !_isPaused;
			GameObject[] objects = GameObject.FindObjectsOfType<GameObject>();
			foreach(GameObject g in objects)
			{
				if(g == gameObject)
					continue;
				if(_isPaused)
					g.SendMessage("Pause", SendMessageOptions.DontRequireReceiver);
				else
					g.SendMessage("UnPause", SendMessageOptions.DontRequireReceiver);
			}
			pauseMenu.SetActive(_isPaused);
			_options.optionsMenu.SetActive(false);
		}
	}

	public void Quit()
	{
		Application.LoadLevel(0);
	}


}
