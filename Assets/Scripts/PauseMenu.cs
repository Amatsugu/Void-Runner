using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PauseMenu : MonoBehaviour {

	public GameObject pauseMenu;
	public GameObject optionsMenu;
	public Text _musicVolNum;
	public Text _effectVolNum;
	public Text _masterVolNum;

	private bool _isPaused;
	private bool _isOptions;
	private bool _VSync;
	private Image _cr;
	private float _masterVol;
	private float _musicVol;
	private float _effectVol;

	// Use this for initialization
	void Start () 
	{
		_cr = pauseMenu.GetComponent<Image>();

		pauseMenu.SetActive(false);
		optionsMenu.SetActive(false);
		LoadPrefs();
	}

	void LoadPrefs()
	{
		_masterVol = PlayerPrefs.GetFloat("MasterVol", 100);
		_masterVolNum.text = _masterVol.ToString();
		_effectVol = PlayerPrefs.GetFloat("EffectVol", 100);
		_effectVolNum.text = _effectVol.ToString();
		_musicVol = PlayerPrefs.GetFloat("MusicVol", 100);
		_effectVolNum.text = _effectVol.ToString();
		int syncCount = PlayerPrefs.GetInt("VSync", 1);
		if(syncCount == 0)
			_VSync = false;
		else
			_VSync = true;
		ApplyOptions();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKeyUp(KeyCode.Escape))
		{
			PauseControl();
		}
	}

	void ApplyOptions()
	{

	}

	public void PauseControl()
	{
		if(_isOptions)
		{
			_isOptions = false;
			optionsMenu.SetActive(_isOptions);
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
			optionsMenu.SetActive(false);
		}
	}

	public void ToggleOptions()
	{
		_isOptions = !_isOptions;
		optionsMenu.SetActive(_isOptions);
	}

	public void OptionsCancel()
	{
		LoadPrefs();
		ApplyOptions();
		ToggleOptions();
	}

	public void OptionsApply()
	{
		ApplyOptions();
		ToggleOptions();
	}

	public void Quit()
	{

	}

	public void SetMasterVol(float vol)
	{
		AudioListener.volume = 100/_masterVol;
		_masterVol = vol;
		_masterVolNum.text = vol.ToString();
	}

	public void SetEffectVol(float vol)
	{
		_effectVol = vol;
		_effectVolNum.text = vol.ToString();
	}

	public void SetMusicVol(float vol)
	{
		_musicVol = vol;
		_effectVolNum.text = vol.ToString();
	}

	public void SetVSync(bool sync)
	{
		_VSync = sync;
	}
}
